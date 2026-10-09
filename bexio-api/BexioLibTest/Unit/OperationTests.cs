using bexio_lib.Implementation;
using bexio_lib.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Xunit;

namespace BexioLibTest.Unit
{
    /// <summary>
    /// Data driven checks against the official bexio OpenAPI description (docs/openapi/bexio-openapi.json):
    /// every operation has a generated method that sends the documented http method to the documented path.
    /// </summary>
    public class OperationTests
    {
        private static readonly string[] HttpMethods = { "get", "post", "put", "patch", "delete" };

        private static JToken Load(string name) => JToken.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "openapi", name)));

        public static IEnumerable<object[]> Operations()
        {
            foreach (var entry in (JArray)Load("operations.json"))
            {
                yield return new object[] { (string)entry["operationId"] };
            }
        }

        private static JToken Entry(string operationId)
            => ((JArray)Load("operations.json")).First(e => (string)e["operationId"] == operationId);

        [Fact]
        public void Every_operation_of_the_spec_is_implemented()
        {
            var spec = (JObject)Load("bexio-openapi.json");
            var specOps = spec["paths"].Cast<JProperty>()
                .SelectMany(p => ((JObject)p.Value).Properties().Where(m => HttpMethods.Contains(m.Name)).Select(m => (string)m.Value["operationId"]))
                .OrderBy(x => x).ToList();
            var implemented = ((JArray)Load("operations.json")).Select(e => (string)e["operationId"]).OrderBy(x => x).ToList();

            Assert.Equal(specOps, implemented);
            Assert.Equal(272, implemented.Count);
        }

        [Theory]
        [MemberData(nameof(Operations))]
        public async System.Threading.Tasks.Task Operation_sends_the_documented_request(string operationId)
        {
            var entry = Entry(operationId);
            var api = new FakeBexioApi();
            var client = new BexioClient(api);
            var group = (string)entry["version"] == "2.0" ? (object)client.V2 : client.V3;
            var endpoint = group.GetType().GetProperties()
                .Select(p => p.GetValue(group))
                .First(e => e.GetType().FullName == (string)entry["class"]);

            var method = endpoint.GetType().GetMethod((string)entry["method"]);
            Assert.NotNull(method);

            var args = new List<object>();
            var pathValues = new Dictionary<string, string>();
            var pathParams = ((JArray)entry["pathParams"]).Select(x => (string)x).ToList();
            foreach (var p in method.GetParameters())
            {
                object value;
                var index = pathParams.IndexOf(p.Name);
                if (index >= 0)
                {
                    value = p.ParameterType == typeof(int) ? (object)(100 + index) : "s" + index;
                    pathValues[p.Name] = Convert.ToString(value);
                }
                else if (p.HasDefaultValue)
                {
                    value = p.DefaultValue;
                }
                else if (p.ParameterType == typeof(byte[]))
                {
                    value = new byte[] { 1 };
                }
                else if (p.ParameterType.IsInterface && p.ParameterType.IsGenericType)
                {
                    value = Activator.CreateInstance(typeof(List<>).MakeGenericType(p.ParameterType.GetGenericArguments()));
                }
                else if (typeof(JToken).IsAssignableFrom(p.ParameterType))
                {
                    value = new JObject();
                }
                else if (p.ParameterType == typeof(string))
                {
                    value = "x";
                }
                else
                {
                    value = Activator.CreateInstance(p.ParameterType);
                }
                args.Add(value);
            }

            method.Invoke(endpoint, args.ToArray());

            var template = (string)entry["path"];
            var expected = Regex.Replace(template, @"\{([^}]+)\}", m =>
            {
                var name = pathParams[Regex.Matches(template.Substring(0, m.Index), @"\{").Count];
                return pathValues[name];
            });
            Assert.Equal(((string)entry["http"]).ToUpperInvariant(), api.LastMethod.Method.ToUpperInvariant());
            Assert.Equal(expected, api.Resource);

            // the async variant sends the same request
            var asyncMethod = endpoint.GetType().GetMethod((string)entry["method"] + "Async");
            Assert.NotNull(asyncMethod);
            var asyncArgs = asyncMethod.GetParameters().Select((p, i) => i < args.Count ? args[i] : (object)CancellationToken.None).ToArray();
            await (System.Threading.Tasks.Task)asyncMethod.Invoke(endpoint, asyncArgs);
            Assert.Equal(expected, api.Resource);
        }
    }
}
