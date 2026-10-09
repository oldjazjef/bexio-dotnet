using bexio_lib.Interfaces;
using bexio_lib.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace bexio_lib.Implementation
{
    public static class BexioEndpointExtensions
    {
        /// <summary>
        /// Reads every entity page by page (bexio returns 500 entries by default).
        /// </summary>
        public static async IAsyncEnumerable<TEntity> GetAllPagesAsync<TEntity>(
            this IBexioApiFullEndpoint<TEntity> endpoint,
            int pageSize = 500,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var offset = 0;
            while (true)
            {
                var page = await endpoint.GetAllAsync(new BexioRequestFilter { limit = pageSize, offset = offset }, cancellationToken).ConfigureAwait(false);
                if (page == null || page.Count == 0) yield break;
                foreach (var entity in page) yield return entity;
                if (page.Count < pageSize) yield break;
                offset += page.Count;
            }
        }
    }
}
