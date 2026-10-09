using bexio_lib.Interfaces;

namespace bexio_lib.Implementation
{
    /// <summary>
    /// Entry point to the bexio api. Groups all endpoints by api version:
    /// <code>await client.V2.Contacts.GetAllAsync(); await client.V3.Currencies.GetAllAsync();</code>
    /// </summary>
    public interface IBexioClient
    {
        IBexioApi Api { get; }
        IBexioV2 V2 { get; }
        IBexioV3 V3 { get; }
    }

    public class BexioClient : IBexioClient
    {
        public BexioClient(IBexioApi api)
        {
            this.Api = api;
            this.V2 = new BexioV2(api);
            this.V3 = new BexioV3(api);
        }

        public IBexioApi Api { get; }
        public IBexioV2 V2 { get; }
        public IBexioV3 V3 { get; }
    }
}
