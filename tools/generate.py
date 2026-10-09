#!/usr/bin/env python3
"""
Generates the bexio client (models, endpoints, facade, search field constants,
operation manifest and endpoint docs) from the official bexio OpenAPI description.

    python3 tools/generate.py

Input : docs/openapi/bexio-openapi.json  (OpenAPI of docs.bexio.com, api 2.0 and 3.0)
Output: bexio-api/bexio-lib/Generated/**, docs/openapi/operations.json, docs/endpoints.md

The output is committed. CI runs this script and fails if the result differs
(see .github/workflows/ci.yml), so the generated code always matches the spec.
"""
import collections
import json
import pathlib
import re
import sys

ROOT = pathlib.Path(__file__).resolve().parent.parent
SPEC = ROOT / "docs/openapi/bexio-openapi.json"
OUT = ROOT / "bexio-api/bexio-lib/Generated"
MANIFEST = ROOT / "docs/openapi/operations.json"
ENDPOINT_DOC = ROOT / "docs/endpoints.md"

METHODS = ("get", "post", "put", "patch", "delete")
HTTP = {"get": "Get", "post": "Post", "put": "Put", "patch": "Patch", "delete": "Delete"}
VERSION_NS = {"2.0": "", "3.0": ".V3"}
VERSION_FACADE = {"2.0": "V2", "3.0": "V3"}

CS_KEYWORDS = set("""abstract as base bool break byte case catch char checked class const continue decimal default delegate do
double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock
long namespace new null object operator out override params private protected public readonly ref return sbyte sealed short
sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using virtual void
volatile while""".split())

spec = json.loads(SPEC.read_text(encoding="utf-8"))
paths = spec["paths"]


# ----------------------------------------------------------------------------- helpers
def pascal(text):
    parts = re.split(r"[^0-9A-Za-z]+", text)
    parts = [p for p in parts if p]
    return "".join(p[0].upper() + p[1:] for p in parts)


def camel(text):
    p = pascal(text)
    return p[0].lower() + p[1:] if p else p


def ident(name):
    return "@" + name if name in CS_KEYWORDS else name


def doc(text, limit=240):
    if not text:
        return ""
    text = re.sub(r"\[([^\]]*)\]\([^)]*\)", r"\1", text)      # markdown links
    text = re.sub(r"[`*]{1,3}", "", text)
    text = re.sub(r"<[^>]+>", "", text)
    text = re.sub(r"\s+", " ", text).strip()
    if len(text) > limit:
        text = text[: limit - 1].rstrip() + "…"
    return text.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")


def summary_lines(text, indent):
    t = doc(text)
    return f"{indent}/// <summary>{t}</summary>\n" if t else ""


def strip_version(name):
    name = re.sub(r"^v\d+(?=[A-Z])", "", name)
    name = re.sub(r"^RMark", "Mark", name)
    return name[0].upper() + name[1:]


# ----------------------------------------------------------------------------- models
def example_of(schema):
    """Example value of a property schema (None if the spec has none)."""
    s = merge(schema or {})
    if "example" in s:
        return s["example"]
    if s.get("type") == "array" and "items" in s:
        e = example_of(s["items"])
        return [e] if e is not None else None
    return None


class Models:
    def __init__(self):
        self.by_key = {}
        self.names = {}
        self.defs = []

    def register(self, schema, hint, version, role=None):
        base = schema.get("title") or hint
        base = re.sub(r"^v\d+(?=[A-Z])", "", base)
        child_hint = pascal(base)
        props = []
        for pname, pschema in schema["properties"].items():
            props.append((pname, cs_type(pschema, child_hint + pascal(pname), version, self), pschema.get("description"), example_of(pschema)))
        key = json.dumps(sorted((p[0], p[1]) for p in props))
        if key in self.by_key:
            return self.by_key[key]
        name = "Bexio" + pascal(base)
        candidates = [name]
        if role:
            candidates.append(name + role)
        candidates.append(name + ("V3" if version == "3.0" else ""))
        if role:
            candidates.append(name + role + ("V3" if version == "3.0" else ""))
        candidate = next((c for c in candidates if c not in self.names), None)
        n = 2
        while candidate is None:
            cand = f"{name}{n}"
            candidate = cand if cand not in self.names else None
            n += 1
        self.names[candidate] = key
        self.by_key[key] = candidate
        self.defs.append((candidate, props, schema.get("description"), schema.get("title")))
        return candidate


def merge(schema):
    if "allOf" not in schema:
        return schema
    merged = {k: v for k, v in schema.items() if k != "allOf"}
    props = {}
    title = schema.get("title")
    for part in schema["allOf"]:
        m = merge(part)
        props.update(m.get("properties", {}))
        title = title or m.get("title")
        for k in ("type", "items", "format"):
            if k in m and k not in merged:
                merged[k] = m[k]
    props.update(schema.get("properties", {}))
    merged["properties"] = props
    if props:
        merged["type"] = "object"
    if title:
        merged["title"] = title
    return merged


def cs_type(schema, hint, version, models, role=None):
    s = merge(schema or {})
    if "anyOf" in s or "oneOf" in s:
        return "JToken"
    t = s.get("type")
    if isinstance(t, list):
        t = next((x for x in t if x != "null"), None)
    fmt = s.get("format")
    if t == "object" or "properties" in s:
        if not s.get("properties"):
            return "JObject"
        return models.register(s, hint, version, role)
    if t == "array":
        return f"List<{cs_type(s.get('items', {}), hint, version, models, role)}>"
    ex = s.get("example")
    if t in ("integer", "number", "boolean") and isinstance(ex, str) and not re.fullmatch(r"-?\d+(\.\d+)?|true|false", ex):
        # the spec declares a number / boolean but documents a text (e.g. invoice payment "title"): trust the example
        print(f"warning: {hint}: declared {t} but example is {ex!r}, using string", file=sys.stderr)
        return "string"
    if t == "integer":
        return "long?" if fmt == "int64" else "int?"
    if t == "number":
        return "double?"
    if t == "boolean":
        return "bool?"
    if t == "string":
        return "byte[]" if fmt == "byte" else "string"
    if t == "date":
        return "string"
    return "JToken"


def emit_models(models):
    out = ["// <auto-generated>",
           "// Generated by tools/generate.py from docs/openapi/bexio-openapi.json. Do not edit.",
           "// </auto-generated>",
           "#nullable disable",
           "using Newtonsoft.Json;",
           "using Newtonsoft.Json.Linq;",
           "using System.Collections.Generic;",
           "",
           "namespace bexio_lib.Models",
           "{"]
    for name, props, description, title in sorted(models.defs, key=lambda d: d[0]):
        out.append(summary_lines(description or title, "    ").rstrip("\n") or "    /// <summary>bexio entity.</summary>")
        out.append(f"    public class {name}")
        out.append("    {")
        used = set()
        for pname, ptype, pdesc, _example in props:
            cs = re.sub(r"[^0-9A-Za-z_]", "_", pname)
            if not cs or cs[0].isdigit():
                cs = "_" + cs
            while cs in used or cs == name:
                cs += "_"
            used.add(cs)
            d = doc(pdesc, 160)
            if d:
                out.append(f"        /// <summary>{d}</summary>")
            if cs != pname:
                out.append(f'        [JsonProperty("{pname}")]')
            out.append(f"        public {ptype} {ident(cs)} {{ get; set; }}")
        out.append("    }")
        out.append("")
    out.append("}")
    return "\n".join(out) + "\n"


# ----------------------------------------------------------------------------- operations
class Param:
    def __init__(self, name, ctype, required, location, orig, description=None):
        self.name, self.ctype, self.required, self.location, self.orig, self.description = name, ctype, required, location, orig, description


class Op:
    pass


def collect_ops(models):
    ops = []
    for path in sorted(paths):
        item = paths[path]
        version = path.strip("/").split("/")[0]
        if version not in VERSION_NS:
            continue
        for method in METHODS:
            raw = item.get(method)
            if not raw:
                continue
            op = Op()
            op.method = method
            op.path = path.lstrip("/")
            op.version = version
            op.operation_id = raw["operationId"]
            op.tag = (raw.get("tags") or ["Other"])[0]
            op.summary = raw.get("summary") or ""
            op.description = raw.get("description") or ""
            op.scopes = sorted({s for sec in raw.get("security", []) for scopes in sec.values() for s in scopes})
            hint = pascal(op.tag) + strip_version(op.operation_id)

            params = list(item.get("parameters", [])) + list(raw.get("parameters", []))
            op.path_params, op.query_params = [], []
            for p in params:
                loc = p["in"]
                if loc not in ("path", "query"):
                    continue
                sch = p.get("schema", {})
                t = sch.get("type")
                if loc == "path":
                    ctype = "int" if t == "integer" else "string"
                else:
                    ctype = {"integer": "int?", "boolean": "bool?"}.get(t, "string")
                prm = Param(camel(p["name"]), ctype, p.get("required", loc == "path"), loc, p["name"], p.get("description"))
                (op.path_params if loc == "path" else op.query_params).append(prm)
            # path params in template order; the spec forgets to declare some of them, derive these from the path
            order = re.findall(r"\{([^}]+)\}", op.path)
            declared = {x.orig for x in op.path_params}
            for placeholder in order:
                if placeholder not in declared:
                    print(f"warning: {op.operation_id} does not declare path parameter {placeholder}, derived from path", file=sys.stderr)
                    op.path_params.append(Param(camel(placeholder), "int" if placeholder.endswith("_id") else "string", True, "path", placeholder))
            op.path_params.sort(key=lambda x: order.index(x.orig) if x.orig in order else 99)

            qnames = {q.orig for q in op.query_params}
            op.paging = bool(qnames & {"limit", "offset", "order_by"})
            op.query_params = [q for q in op.query_params if q.orig not in ("limit", "offset", "order_by")]

            # body
            op.body = None  # (kind, ctype)
            rb = raw.get("requestBody")
            if rb:
                content = rb.get("content", {})
                if "multipart/form-data" in content:
                    op.body = ("file", None)
                else:
                    sch = merge(next(iter(content.values())).get("schema", {}))
                    items = merge(sch.get("items", {})) if sch.get("type") == "array" else {}
                    if sch.get("type") == "array" and (items.get("title") in ("v2SearchField", "SearchField")):
                        op.body = ("search", "BexioRequestFilter")
                        op.paging = True
                    else:
                        ctype = cs_type(sch, hint + "Request", version, models, "Request")
                        ctype = re.sub(r"^List<", "ICollection<", ctype)
                        op.body = ("json", ctype)
            # response
            op.ret = ("void", None)
            for code in sorted(raw.get("responses", {})):
                if not code.startswith("2"):
                    continue
                content = raw["responses"][code].get("content", {})
                if not content:
                    continue
                sch = merge(next(iter(content.values())).get("schema", {}))
                pr = sch.get("properties", {})
                if sch.get("type") == "string" and sch.get("format") == "binary":
                    op.ret = ("bytes", "byte[]")
                elif sch.get("title") in ("EntryDeleted", "SuccessResponse") or (set(pr) == {"success"}):
                    op.ret = ("bool", "bool")
                else:
                    ctype = cs_type(sch, hint + "Response", version, models, "Response")
                    ctype = re.sub(r"^List<", "ICollection<", ctype)
                    op.ret = ("model", ctype)
                break
            op.name = strip_version(op.operation_id)
            ops.append(op)
    return ops


def path_expr(op):
    expr = op.path
    for p in op.path_params:
        if p.ctype == "string":
            expr = expr.replace("{" + p.orig + "}", "{Uri.EscapeDataString(" + ident(p.name) + ")}")
        else:
            expr = expr.replace("{" + p.orig + "}", "{" + ident(p.name) + "}")
    return '$"' + expr + '"'


def signature_params(op):
    """Ordered parameters: path, body, required query, filter, optional query."""
    sig = []
    for p in op.path_params:
        sig.append((p.ctype, ident(p.name), None))
    if op.body:
        kind, ctype = op.body
        if kind == "file":
            sig.append(("string", "fileName", None))
            sig.append(("byte[]", "content", None))
        elif kind == "json":
            sig.append((ctype, "body", None))
    for q in op.query_params:
        if q.required:
            sig.append((q.ctype, ident(q.name), None))
    if op.paging:
        sig.append(("BexioRequestFilter", "requestParameter", "null"))
    for q in op.query_params:
        if not q.required:
            sig.append((q.ctype, ident(q.name), "null"))
    return sig


def render_params(sig, extra=None):
    items = [f"{t} {n}" + (f" = {d}" if d else "") for t, n, d in sig]
    if extra:
        items.append(extra)
    return ", ".join(items)


def op_doc(op, indent):
    lines = []
    text = op.summary or op.name
    lines.append(f"{indent}/// <summary>{doc(text)}</summary>")
    remarks = []
    if op.scopes:
        remarks.append("Scope: " + ", ".join(op.scopes))
    remarks.append(f"{op.method.upper()} /{op.path}")
    lines.append(f"{indent}/// <remarks>{doc('. '.join(remarks), 300)}</remarks>")
    return "\n".join(lines) + "\n"


def method_bodies(op):
    ret_kind, ret_type = op.ret
    sig = signature_params(op)
    pnames = ", ".join(n for _, n, _ in sig)
    lines = [f"var request = this.NewRequestFor({path_expr(op)});"]
    if op.paging:
        lines.append("request.AddSearchData(requestParameter);" if op.body and op.body[0] == "search" else "request.AddRequestData(requestParameter);")
    for q in op.query_params:
        lines.append(f'this.AddQuery(request, "{q.orig}", {ident(q.name)});')
    if op.body and op.body[0] == "json":
        lines.append("request.AddRequestBodyData(body);")
    if op.body and op.body[0] == "file":
        lines.append('request.AddFile("file", fileName, content);')
    verb = f"HttpMethod.{HTTP[op.method]}"
    if ret_kind == "void":
        sync = f"this.Send(request, {verb});"
        asy = f"this.SendAsync(request, {verb}, cancellationToken);"
    elif ret_kind == "bool":
        sync = f"return this.SendSuccess(request, {verb});"
        asy = f"this.SendSuccessAsync(request, {verb}, cancellationToken);"
    elif ret_kind == "bytes":
        sync = f"return this.SendBytes(request, {verb});"
        asy = f"this.SendBytesAsync(request, {verb}, cancellationToken);"
    else:
        sync = f"return this.Send<{ret_type}>(request, {verb});"
        asy = f"this.SendAsync<{ret_type}>(request, {verb}, cancellationToken);"
    return lines, sync, ("return " + asy if not asy.startswith("return") else asy)


def sync_ret(op):
    return {"void": "void", "bool": "bool", "bytes": "byte[]"}.get(op.ret[0]) or op.ret[1]


def async_ret(op):
    r = sync_ret(op)
    return "Task" if r == "void" else f"Task<{r}>"


def element_type(op):
    m = re.match(r"ICollection<(.+)>$", op.ret[1] or "")
    return m.group(1) if m else None


class EndpointClass:
    def __init__(self, version, tag):
        self.version, self.tag, self.ops = version, tag, []
        self.name = "BexioApi" + pascal(tag) + "Endpoint"
        self.iface = "I" + self.name
        self.prop = pascal(tag)

    @property
    def ns(self):
        return "bexio_lib.Implementation.Endpoints" + VERSION_NS[self.version]

    @property
    def ins(self):
        return "bexio_lib.Interfaces" + VERSION_NS[self.version]


def build_classes(ops):
    classes = collections.OrderedDict()
    for op in ops:
        key = (op.version, op.tag)
        classes.setdefault(key, EndpointClass(*key)).ops.append(op)
    # unique method names per class
    for c in classes.values():
        seen = collections.Counter()
        for op in c.ops:
            seen[op.name] += 1
        dup = {n for n, k in seen.items() if k > 1}
        for op in c.ops:
            if op.name in dup:
                op.name = op.name + "_" + op.method.capitalize()
                print(f"warning: duplicate method name in {c.name}, renamed to {op.name}", file=sys.stderr)
    return list(classes.values())


def primary_root(c):
    plain = [o.path for o in c.ops if "{" not in o.path]
    if not plain:
        return None
    plain.sort(key=lambda p: (p.count("/"), p))
    return plain[0]


def aliases(c):
    """Short, stable names for the main collection: GetAll, GetById, Search, Create, Update, Delete."""
    root = primary_root(c)
    if not root:
        return []
    res = []
    for op in c.ops:
        item_path = len(op.path_params) == 1 and op.path == f"{root}/{{{op.path_params[0].orig}}}"
        int_id = item_path and op.path_params[0].ctype == "int"
        has_json_body = bool(op.body and op.body[0] == "json")
        if op.method == "get" and op.path == root and op.ret[0] == "model" and op.paging and not any(q.required for q in op.query_params):
            res.append(("GetAll", op, [("BexioRequestFilter", "requestParameter", "null")], ["requestParameter"]))
        elif op.method == "get" and int_id and op.ret[0] == "model" and not any(q.required for q in op.query_params):
            res.append(("GetById", op, [("int", "id", None)], ["id"]))
        elif op.method == "post" and op.path == f"{root}/search" and op.body and op.body[0] == "search":
            res.append(("Search", op, [("BexioRequestFilter", "requestParameter", "null")], ["requestParameter"]))
        elif op.method == "post" and op.path == root and has_json_body and not op.path_params:
            res.append(("Create", op, [(op.body[1], "body", None)], ["body"]))
        elif int_id and op.method in ("post", "put", "patch") and has_json_body:
            res.append(("Update", op, [("int", "id", None), (op.body[1], "body", None)], ["id", "body"]))
        elif int_id and op.method == "delete" and op.ret[0] in ("bool", "void"):
            res.append(("Delete", op, [("int", "id", None)], ["id"]))
    taken = {o.name for o in c.ops} | {o.name + "Async" for o in c.ops}
    return [a for a in res if a[0] not in taken and a[0] + "Async" not in taken]


def model_examples(models):
    """Example JSON per model built from the examples of the spec (used by the tests)."""
    by_name = {d[0]: d for d in models.defs}
    cache = {}

    def build(name):
        if name in cache:
            return cache[name]
        cache[name] = {}
        obj = {}
        for pname, ptype, _desc, example in by_name[name][1]:
            inner = re.match(r"List<(.+)>$", ptype)
            target = inner.group(1) if inner else ptype
            if target in by_name:
                nested = build(target)
                if nested:
                    obj[pname] = [nested] if inner else nested
            elif example is not None and ptype not in ("JToken", "JObject"):
                obj[pname] = example
        cache[name] = obj
        return obj

    return {n: build(n) for n in sorted(by_name)}


def emit_class(c):
    out = ["// <auto-generated>",
           "// Generated by tools/generate.py from docs/openapi/bexio-openapi.json. Do not edit.",
           "// </auto-generated>",
           "#nullable disable",
           "using bexio_lib.Data;",
           "using bexio_lib.Implementation;",
           "using bexio_lib.Interfaces;",
           "using bexio_lib.Models;",
           "using Newtonsoft.Json.Linq;",
           "using System;",
           "using System.Collections.Generic;",
           "using System.Net.Http;",
           "using System.Threading;",
           "using System.Threading.Tasks;",
           "",
           f"namespace {c.ins}",
           "{",
           f"    /// <summary>{doc(c.tag)} (api {c.version}).</summary>",
           f"    public partial interface {c.iface} : IBexioApiEndpoint",
           "    {"]
    als = aliases(c)
    impl = []
    for op in c.ops:
        sig = signature_params(op)
        out.append(op_doc(op, "        ").rstrip("\n"))
        out.append(f"        {sync_ret(op)} {op.name}({render_params(sig)});")
        out.append(op_doc(op, "        ").rstrip("\n"))
        out.append(f"        {async_ret(op)} {op.name}Async({render_params(sig, 'CancellationToken cancellationToken = default')});")
        if op.paging and element_type(op):
            psig = [(t, n, d) for t, n, d in sig if n in {x.name for x in op.path_params} or n == "requestParameter"]
            psig = [(t, n, d) for t, n, d in psig if True]
            out.append(f"        /// <summary>{doc(op.summary or op.name).rstrip(".")}. Reads all pages.</summary>")
            out.append(f"        IAsyncEnumerable<{element_type(op)}> {op.name}PagesAsync({render_params(psig, 'int pageSize = 500, CancellationToken cancellationToken = default')});")
    for alias, op, asig, anames in als:
        out.append(f"        /// <summary>Short form of <see cref=\"{op.name}\"/>.</summary>")
        out.append(f"        {sync_ret(op)} {alias}({render_params(asig)});")
        out.append(f"        /// <summary>Short form of <see cref=\"{op.name}Async\"/>.</summary>")
        out.append(f"        {async_ret(op)} {alias}Async({render_params(asig, 'CancellationToken cancellationToken = default')});")
    out.append("    }")
    out.append("}")
    out.append("")
    out.append(f"namespace {c.ns}")
    out.append("{")
    out.append(f"    /// <summary>{doc(c.tag)} (api {c.version}).</summary>")
    out.append(f"    public partial class {c.name} : BexioApiEndpoint, {c.ins}.{c.iface}")
    out.append("    {")
    root = primary_root(c) or c.ops[0].path.split("{")[0].rstrip("/")
    endpoint = "/".join(root.split("/")[1:])
    out.append(f'        public {c.name}(IBexioApi api) : base(api, "{c.version}", "{endpoint}") {{ }}')
    out.append("")
    for op in c.ops:
        sig = signature_params(op)
        lines, sync, asy = method_bodies(op)
        out.append("        /// <inheritdoc />")
        out.append(f"        public {sync_ret(op)} {op.name}({render_params(sig)})")
        out.append("        {")
        out.extend("            " + l for l in lines)
        out.append("            " + sync)
        out.append("        }")
        out.append("")
        out.append("        /// <inheritdoc />")
        out.append(f"        public {async_ret(op)} {op.name}Async({render_params(sig, 'CancellationToken cancellationToken = default')})")
        out.append("        {")
        out.extend("            " + l for l in lines)
        out.append("            " + asy)
        out.append("        }")
        out.append("")
        if op.paging and element_type(op):
            psig = [(t, n, d) for t, n, d in sig if n in {x.name for x in op.path_params} or n == "requestParameter"]
            path_args = ", ".join(ident(p.name) for p in op.path_params)
            call_args = (path_args + ", " if path_args else "") + "requestParameter: f, cancellationToken: c"
            out.append("        /// <inheritdoc />")
            out.append(f"        public IAsyncEnumerable<{element_type(op)}> {op.name}PagesAsync({render_params(psig, 'int pageSize = 500, CancellationToken cancellationToken = default')})")
            out.append(f"            => this.Pages<{element_type(op)}>((f, c) => this.{op.name}Async({call_args}), requestParameter, pageSize, cancellationToken);")
            out.append("")
    for alias, op, asig, anames in als:
        args = ", ".join(anames)
        out.append("        /// <inheritdoc />")
        out.append(f"        public {sync_ret(op)} {alias}({render_params(asig)}) => this.{op.name}({alias_call(op, anames)});")
        out.append("        /// <inheritdoc />")
        out.append(f"        public {async_ret(op)} {alias}Async({render_params(asig, 'CancellationToken cancellationToken = default')}) => this.{op.name}Async({alias_call(op, anames, True)});")
        out.append("")
    out.append("    }")
    out.append("}")
    return "\n".join(out) + "\n"


def alias_call(op, anames, is_async=False):
    """Map alias arguments onto the full signature using named arguments."""
    sig = signature_params(op)
    parts = []
    path_names = [ident(p.name) for p in op.path_params]
    for n in anames:
        if n == "id" and path_names:
            parts.append(f"{path_names[0]}: id")
        elif n == "body":
            parts.append("body: body")
        elif n == "requestParameter":
            parts.append("requestParameter: requestParameter")
    if is_async:
        parts.append("cancellationToken: cancellationToken")
    return ", ".join(parts)


def emit_facade(classes):
    out = ["// <auto-generated>",
           "// Generated by tools/generate.py from docs/openapi/bexio-openapi.json. Do not edit.",
           "// </auto-generated>",
           "#nullable disable",
           "using bexio_lib.Interfaces;",
           "",
           "namespace bexio_lib.Implementation",
           "{"]
    for ver in ("2.0", "3.0"):
        cls = sorted([c for c in classes if c.version == ver], key=lambda c: c.prop)
        v = VERSION_FACADE[ver]
        props = collections.OrderedDict()
        for c in cls:
            p = c.prop
            assert p not in props, f"duplicate facade property {p}"
            props[p] = c
        out.append(f"    /// <summary>Endpoints of api {ver}.</summary>")
        out.append(f"    public interface IBexio{v}")
        out.append("    {")
        for p, c in props.items():
            out.append(f"        /// <summary>{doc(c.tag)}</summary>")
            out.append(f"        {c.ins}.{c.iface} {p} {{ get; }}")
        out.append("    }")
        out.append("")
        out.append(f"    public class Bexio{v} : IBexio{v}")
        out.append("    {")
        out.append("        private readonly IBexioApi _api;")
        for p, c in props.items():
            out.append(f"        private {c.ins}.{c.iface} _{p};")
        out.append("")
        out.append(f"        public Bexio{v}(IBexioApi api)")
        out.append("        {")
        out.append("            this._api = api;")
        out.append("        }")
        out.append("")
        for p, c in props.items():
            out.append(f"        public {c.ins}.{c.iface} {p} => this._{p} ??= new {c.ns}.{c.name}(this._api);")
        out.append("    }")
        out.append("")
    out.append("}")
    return "\n".join(out) + "\n"


def emit_search_fields(ops):
    out = ["// <auto-generated>",
           "// Generated by tools/generate.py from docs/openapi/bexio-openapi.json. Do not edit.",
           "// </auto-generated>",
           "#nullable disable",
           "namespace bexio_lib.Data",
           "{",
           "    /// <summary>Field names accepted by the search endpoints, per resource (from the bexio documentation).</summary>",
           "    public static class BexioSearchFields",
           "    {"]
    seen = set()
    for path in sorted(paths):
        raw = paths[path].get("post")
        if not raw or not path.endswith("/search") or path.strip("/").split("/")[0] not in VERSION_NS:
            continue
        description = raw.get("description", "")
        values = re.findall(r"^\*\s+`([^`]+)`", description, flags=re.M)
        if not values:
            continue
        segs = [x for x in path.strip("/").split("/")[1:-1] if not x.startswith("{")]
        name = pascal("_".join(segs)) or "Search"
        if name in seen:
            name += "V3" if path.startswith("/3.0/") else "2"
        seen.add(name)
        out.append(f"        /// <summary>Search fields of /{'/'.join(path.strip('/').split('/')[:-1])}.</summary>")
        out.append(f"        public static class {name}")
        out.append("        {")
        used = set()
        for v in values:
            cs = re.sub(r"[^0-9A-Za-z_]", "_", str(v))
            if not cs or cs[0].isdigit():
                cs = "_" + cs
            while cs in used or cs == name:
                cs += "_"
            used.add(cs)
            out.append(f'            public const string {ident(cs)} = "{v}";')
        out.append("        }")
    out.append("    }")
    out.append("}")
    return "\n".join(out) + "\n"


def emit_manifest(classes):
    entries = []
    for c in classes:
        als = {id(a[1]): a[0] for a in aliases(c)}
        for op in c.ops:
            entries.append({
                "operationId": op.operation_id,
                "http": op.method.upper(),
                "path": op.path,
                "version": op.version,
                "interface": f"{c.ins}.{c.iface}",
                "class": f"{c.ns}.{c.name}",
                "method": op.name,
                "alias": als.get(id(op)),
                "pathParams": [p.name for p in op.path_params],
                "bodyKind": op.body[0] if op.body else None,
                "returns": op.ret[0],
                "scopes": op.scopes,
            })
    return json.dumps(entries, indent=1, ensure_ascii=False) + "\n"


def emit_doc(classes):
    out = ["# Endpoint reference", "",
           "Generated from the official bexio OpenAPI description by `tools/generate.py`. Do not edit by hand.", "",
           "Every operation of the bexio API 2.0 and 3.0 is available through `IBexioClient`: `client.V2.<Group>` and `client.V3.<Group>`.",
           "Each operation exists as `Name(...)` and `NameAsync(..., CancellationToken)`. List and search operations with `limit` / `offset` also offer `NamePagesAsync(...)`, which reads all pages.",
           "The method names are the operation ids of the bexio documentation. Groups with a main collection additionally offer the short forms",
           "`GetAll`, `GetById`, `Search`, `Create`, `Update` and `Delete`.",
           "",
           f"Total: {sum(len(c.ops) for c in classes)} operations in {len(classes)} groups.",
           ""]
    for ver in ("2.0", "3.0"):
        out.append(f"## API {ver}")
        out.append("")
        for c in sorted([c for c in classes if c.version == ver], key=lambda c: c.prop):
            out.append(f"### `client.{VERSION_FACADE[ver]}.{c.prop}`")
            out.append("")
            out.append(f"Interface `{c.iface}`, {len(c.ops)} operations.")
            out.append("")
            out.append("| Method | Request | Scope |")
            out.append("|---|---|---|")
            als = {id(a[1]): a[0] for a in aliases(c)}
            for op in c.ops:
                name = f"`{op.name}`" + (f" / `{als[id(op)]}`" if id(op) in als else "")
                out.append(f"| {name} | `{op.method.upper()} /{op.path}` | {', '.join(op.scopes) or '-'} |")
            out.append("")
    return "\n".join(out) + "\n"


def write(path, text):
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(text, encoding="utf-8", newline="\n")


def main():
    models = Models()
    ops = collect_ops(models)
    classes = build_classes(ops)

    # clean generated output
    if OUT.exists():
        for f in OUT.rglob("*.g.cs"):
            f.unlink()
    write(OUT / "Models.g.cs", emit_models(models))
    write(OUT / "SearchFields.g.cs", emit_search_fields(ops))
    for c in classes:
        write(OUT / ("V3" if c.version == "3.0" else "V2") / f"{c.name}.g.cs", emit_class(c))
    write(OUT / "BexioClientVersions.g.cs", emit_facade(classes))
    write(MANIFEST, emit_manifest(classes))
    write(ROOT / "bexio-api/BexioLibTest/Generated/model-examples.json", json.dumps(model_examples(models), indent=1, ensure_ascii=False) + "\n")
    write(ENDPOINT_DOC, emit_doc(classes))
    print(f"generated {len(ops)} operations, {len(classes)} endpoint classes, {len(models.defs)} models")


if __name__ == "__main__":
    main()
