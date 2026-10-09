using bexio_lib.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Interfaces
{
    public interface IBexioApiEndpoint
    {
        /// <summary>
        /// Resource path relative to the api base url, including the version, e.g. "2.0/contact"
        /// </summary>
        string ENDPOINT { get; init; }
        IBexioApi API { get; init; }
    }

    /// <summary>
    /// Read only endpoint: get by id, get all and search
    /// </summary>
    public interface IBexioApiFullEndpoint<TEntity> : IBexioApiEndpoint
    {
        TEntity GetById(int id);
        ICollection<TEntity> GetAll(BexioRequestFilter requestParameter = null);
        ICollection<TEntity> Search(BexioRequestFilter requestParameter = null);

        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ICollection<TEntity>> GetAllAsync(BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default);
        Task<ICollection<TEntity>> SearchAsync(BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Endpoint that additionally supports creating, updating and deleting
    /// </summary>
    public interface IBexioApiCrudEndpoint<TEntity> : IBexioApiFullEndpoint<TEntity>
    {
        TEntity Create(TEntity entity);
        TEntity Update(int id, TEntity entity);
        bool Delete(int id);

        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(int id, TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
