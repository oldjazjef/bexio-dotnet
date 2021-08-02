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
    public abstract class BexioApiEndopint : IBexioApiEndpoint
    {
        public IBexioApi API { get; init; }
        public string ENDPOINT { get; init; }

        public BexioApiEndopint(IBexioApi api, string endpoint)
        {
            this.API = api;
            this.ENDPOINT = $"{this.API.API_URL}/{endpoint}";
        }
    }

    public abstract class BexioApiFullEndpoint : BexioApiEndopint
    {

        public BexioApiFullEndpoint(IBexioApi api) : base(api, "kb_order")
        {

        }

        #region DEFAULTS

        ICollection<T> GetAll<T>(BexioRequestFilter requestParameter)
            where T : BexioOrder
        {
            var request = new RestRequest(this.ENDPOINT, DataFormat.Json)
                .AddRequestData(requestParameter);

            var result = this.API.CLIENT.Get(request)
                .DeserializeRequestResult<ICollection<T>>();

            return result;
        }

        T GetById<T>(int id)
            where T : BexioOrder
        {
            var request = new RestRequest($"{this.ENDPOINT}/{id}", DataFormat.Json);

            var result = this.API.CLIENT.Get(request)
                .DeserializeRequestResult<T>();

            return result;
        }

        ICollection<T> Search<T>(BexioRequestFilter requestParameter)
            where T : BexioOrder
        {
            var request = new RestRequest($"{this.ENDPOINT}/search", DataFormat.Json)
                .AddRequestData(requestParameter);

            var result = this.API.CLIENT.Post(request)
                .DeserializeRequestResult<ICollection<T>>();

            return result;
        }

        #endregion
    }

}
