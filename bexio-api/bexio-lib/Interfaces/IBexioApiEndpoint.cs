using bexio_lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    public interface IBexioApiEndpoint
    {
        string ENDPOINT { get; init; }
        IBexioApi API { get; init; }
    }

    public interface IBexioApiFullEndpoint<TEntity> : IBexioApiEndpoint
    {
        TEntity GetById(int id);
        ICollection<TEntity> GetAll(BexioRequestFilter requestParameter = null);
        ICollection<TEntity> Search(BexioRequestFilter requestParameter = null);

    }
}
