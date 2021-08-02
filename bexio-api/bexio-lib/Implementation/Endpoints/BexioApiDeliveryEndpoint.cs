using bexio_lib.Interfaces;
using bexio_lib.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints
{
    public class BexioApiDeliveryEndpoint : BexioApiFullEndpoint<BexioDelivery>, IBexioApiDeliveryEndpoint
    {
        public BexioApiDeliveryEndpoint(IBexioApi api): base(api, "kb_delivery")
        {

        }

        public void Issue(int deliveryId)
        {
            var request = new RestRequest($"{this.ENDPOINT}/{deliveryId}/issue", DataFormat.Json);

            this.API.Post(request);
        }
    }
}
