using bexio_lib.Interfaces;
using bexio_lib.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    public abstract class BexioApiEndpoint : IBexioApiEndpoint
    {
        public IBexioApi API { get; init; }
        public string ENDPOINT { get; init; }

        /// <param name="api">Api client</param>
        /// <param name="version">Api version, see <see cref="bexio_lib.Data.BexioApiVersion"/></param>
        /// <param name="endpoint">Resource path without version, e.g. "contact"</param>
        public BexioApiEndpoint(IBexioApi api, string version, string endpoint)
        {
            this.API = api;
            this.ENDPOINT = $"{version}/{endpoint}";
        }

        protected BexioRequest NewRequest(string suffix = null)
        {
            var resource = string.IsNullOrEmpty(suffix) ? this.ENDPOINT : $"{this.ENDPOINT}/{suffix}";
            return new BexioRequest(resource);
        }

        /// <summary>Request for a complete path including the api version, e.g. "2.0/contact/5".</summary>
        protected BexioRequest NewRequestFor(string path) => new BexioRequest(path);

        /// <summary>Adds a query parameter. null is skipped, bool is sent lower case, collections comma separated.</summary>
        protected void AddQuery(BexioRequest request, string name, object value)
        {
            if (value == null)
            {
                return;
            }
            string text;
            if (value is bool flag)
            {
                text = flag ? "true" : "false";
            }
            else if (value is System.Collections.IEnumerable list && !(value is string))
            {
                text = string.Join(",", list.Cast<object>().Select(x => System.Convert.ToString(x, CultureInfo.InvariantCulture)));
            }
            else
            {
                text = System.Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            request.AddQueryParameter(name, text);
        }

        protected bool SendSuccess(BexioRequest request, HttpMethod method)
            => this.API.Send(request, method).ToSuccessResult();

        protected async Task<bool> SendSuccessAsync(BexioRequest request, HttpMethod method, CancellationToken ct)
            => (await this.API.SendAsync(request, method, ct).ConfigureAwait(false)).ToSuccessResult();

        protected byte[] SendBytes(BexioRequest request, HttpMethod method)
        {
            request.Accept = "*/*";
            return this.API.Send(request, method).EnsureSuccess().RawBytes;
        }

        protected async Task<byte[]> SendBytesAsync(BexioRequest request, HttpMethod method, CancellationToken ct)
        {
            request.Accept = "*/*";
            return (await this.API.SendAsync(request, method, ct).ConfigureAwait(false)).EnsureSuccess().RawBytes;
        }

        /// <summary>Reads all pages of a list / search operation (limit and offset are set per page).</summary>
        protected async IAsyncEnumerable<T> Pages<T>(
            System.Func<BexioRequestFilter, CancellationToken, Task<ICollection<T>>> fetch,
            BexioRequestFilter filter,
            int pageSize,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var offset = 0;
            while (true)
            {
                var page = new BexioRequestFilter { limit = pageSize, offset = offset, order_by = filter?.order_by };
                if (filter != null)
                {
                    foreach (var instruction in filter.Filters)
                    {
                        page.Filters.Add(instruction);
                    }
                }
                var items = await fetch(page, cancellationToken).ConfigureAwait(false);
                if (items == null || items.Count == 0)
                {
                    yield break;
                }
                foreach (var item in items)
                {
                    yield return item;
                }
                if (items.Count < pageSize)
                {
                    yield break;
                }
                offset += items.Count;
            }
        }

        protected T Send<T>(BexioRequest request, HttpMethod method)
            => this.API.Send(request, method).DeserializeRequestResult<T>();

        protected async Task<T> SendAsync<T>(BexioRequest request, HttpMethod method, CancellationToken ct)
            => (await this.API.SendAsync(request, method, ct).ConfigureAwait(false)).DeserializeRequestResult<T>();

        protected void Send(BexioRequest request, HttpMethod method)
            => this.API.Send(request, method).EnsureSuccess();

        protected async Task SendAsync(BexioRequest request, HttpMethod method, CancellationToken ct)
            => (await this.API.SendAsync(request, method, ct).ConfigureAwait(false)).EnsureSuccess();
    }

    public abstract class BexioApiFullEndpoint<TEntity> : BexioApiEndpoint, IBexioApiFullEndpoint<TEntity>
    {
        public BexioApiFullEndpoint(IBexioApi api, string version, string endpoint) : base(api, version, endpoint)
        {
        }

        #region DEFAULTS

        public ICollection<TEntity> GetAll(BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<TEntity>>(this.NewRequest().AddRequestData(requestParameter), HttpMethod.Get);

        public TEntity GetById(int id)
            => this.Send<TEntity>(this.NewRequest(id.ToString()), HttpMethod.Get);

        public ICollection<TEntity> Search(BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<TEntity>>(this.NewRequest("search").AddSearchData(requestParameter), HttpMethod.Post);

        public Task<ICollection<TEntity>> GetAllAsync(BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<TEntity>>(this.NewRequest().AddRequestData(requestParameter), HttpMethod.Get, cancellationToken);

        public Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest(id.ToString()), HttpMethod.Get, cancellationToken);

        public Task<ICollection<TEntity>> SearchAsync(BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<TEntity>>(this.NewRequest("search").AddSearchData(requestParameter), HttpMethod.Post, cancellationToken);

        #endregion
    }

    public abstract class BexioApiCrudEndpoint<TEntity> : BexioApiFullEndpoint<TEntity>, IBexioApiCrudEndpoint<TEntity>
    {
        public BexioApiCrudEndpoint(IBexioApi api, string version, string endpoint) : base(api, version, endpoint)
        {
        }

        /// <summary>
        /// Http method used to edit an entity. Bexio 2.0 edits with POST, 3.0 uses PUT.
        /// </summary>
        protected virtual HttpMethod UpdateMethod => HttpMethod.Post;

        public TEntity Create(TEntity entity)
            => this.Send<TEntity>(this.NewRequest().AddRequestBodyData(entity), HttpMethod.Post);

        public TEntity Update(int id, TEntity entity)
            => this.Send<TEntity>(this.NewRequest(id.ToString()).AddRequestBodyData(entity), this.UpdateMethod);

        public bool Delete(int id)
            => this.API.Send(this.NewRequest(id.ToString()), HttpMethod.Delete).ToDeleteResult();

        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest().AddRequestBodyData(entity), HttpMethod.Post, cancellationToken);

        public Task<TEntity> UpdateAsync(int id, TEntity entity, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest(id.ToString()).AddRequestBodyData(entity), this.UpdateMethod, cancellationToken);

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
            => (await this.API.SendAsync(this.NewRequest(id.ToString()), HttpMethod.Delete, cancellationToken).ConfigureAwait(false)).ToDeleteResult();
    }
}
