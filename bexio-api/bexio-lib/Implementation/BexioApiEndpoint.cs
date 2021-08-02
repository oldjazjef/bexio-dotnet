using bexio_lib.Interfaces;
using bexio_lib.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation
{
    public abstract class BexioApiEndpoint : IBexioApiEndpoint
    {
        public IBexioApi API { get; init; }
        public string ENDPOINT { get; init; }

        public BexioApiEndpoint(IBexioApi api, string endpoint)
        {
            this.API = api;
            this.ENDPOINT = $"{this.API.API_URL}/{endpoint}";
        }
    }

    public abstract class BexioApiFullEndpoint<TEntity> : BexioApiEndpoint, IBexioApiFullEndpoint<TEntity>
    {

        public BexioApiFullEndpoint(IBexioApi api, string endpoint) : base(api, endpoint)
        {

        }

        #region DEFAULTS

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="requestParameter"></param>
        /// <returns></returns>
        public ICollection<TEntity> GetAll(BexioRequestFilter requestParameter = null)
        {
            var request = new RestRequest(this.ENDPOINT, DataFormat.Json)
                .AddRequestData(requestParameter);

            var result = this.API.Get(request)
                .DeserializeRequestResult<ICollection<TEntity>>();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(int id)
        {
            var request = new RestRequest($"{this.ENDPOINT}/{id}", DataFormat.Json);

            var result = this.API.Get(request)
                .DeserializeRequestResult<TEntity>();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParameter"></param>
        /// <returns></returns>
        public ICollection<TEntity> Search(BexioRequestFilter requestParameter = null)
        {
            var request = new RestRequest($"{this.ENDPOINT}/search", DataFormat.Json)
                .AddRequestData(requestParameter);

            var result = this.API.Post(request)
                .DeserializeRequestResult<ICollection<TEntity>>();

            return result;
        }

        #endregion
    }

}
