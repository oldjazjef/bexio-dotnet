using bexio_lib.Interfaces;
using bexio_lib.Models;
using RestSharp;
using System.Collections.Generic;
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

        protected RestRequest NewRequest(string suffix = null)
        {
            var resource = string.IsNullOrEmpty(suffix) ? this.ENDPOINT : $"{this.ENDPOINT}/{suffix}";
            return new RestRequest(resource, DataFormat.Json);
        }

        protected T Send<T>(RestRequest request, Method method)
            => this.API.Execute(request, method).DeserializeRequestResult<T>();

        protected async Task<T> SendAsync<T>(RestRequest request, Method method, CancellationToken ct)
            => (await this.API.ExecuteAsync(request, method, ct).ConfigureAwait(false)).DeserializeRequestResult<T>();

        protected void Send(RestRequest request, Method method)
            => this.API.Execute(request, method).EnsureSuccess();

        protected async Task SendAsync(RestRequest request, Method method, CancellationToken ct)
            => (await this.API.ExecuteAsync(request, method, ct).ConfigureAwait(false)).EnsureSuccess();
    }

    public abstract class BexioApiFullEndpoint<TEntity> : BexioApiEndpoint, IBexioApiFullEndpoint<TEntity>
    {
        public BexioApiFullEndpoint(IBexioApi api, string version, string endpoint) : base(api, version, endpoint)
        {
        }

        #region DEFAULTS

        public ICollection<TEntity> GetAll(BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<TEntity>>(this.NewRequest().AddRequestData(requestParameter), Method.GET);

        public TEntity GetById(int id)
            => this.Send<TEntity>(this.NewRequest(id.ToString()), Method.GET);

        public ICollection<TEntity> Search(BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<TEntity>>(this.NewRequest("search").AddSearchData(requestParameter), Method.POST);

        public Task<ICollection<TEntity>> GetAllAsync(BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<TEntity>>(this.NewRequest().AddRequestData(requestParameter), Method.GET, cancellationToken);

        public Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest(id.ToString()), Method.GET, cancellationToken);

        public Task<ICollection<TEntity>> SearchAsync(BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<TEntity>>(this.NewRequest("search").AddSearchData(requestParameter), Method.POST, cancellationToken);

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
        protected virtual Method UpdateMethod => Method.POST;

        public TEntity Create(TEntity entity)
            => this.Send<TEntity>(this.NewRequest().AddRequestBodyData(entity), Method.POST);

        public TEntity Update(int id, TEntity entity)
            => this.Send<TEntity>(this.NewRequest(id.ToString()).AddRequestBodyData(entity), this.UpdateMethod);

        public bool Delete(int id)
            => this.API.Execute(this.NewRequest(id.ToString()), Method.DELETE).ToDeleteResult();

        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest().AddRequestBodyData(entity), Method.POST, cancellationToken);

        public Task<TEntity> UpdateAsync(int id, TEntity entity, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest(id.ToString()).AddRequestBodyData(entity), this.UpdateMethod, cancellationToken);

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
            => (await this.API.ExecuteAsync(this.NewRequest(id.ToString()), Method.DELETE, cancellationToken).ConfigureAwait(false)).ToDeleteResult();
    }
}
