using bexio_lib.Models;

namespace bexio_lib.Interfaces.V4
{
    // Ids of api 4.0 resources are strings (uuids); the generic endpoint accepts them through the int overloads only for
    // numeric ids, so these interfaces expose string based access via the V4 endpoint base.
    public interface IBexioApiV4Endpoint<TEntity> : IBexioApiEndpoint
    {
        System.Collections.Generic.ICollection<TEntity> GetAll(BexioRequestFilter requestParameter = null);
        TEntity GetById(string id);
        TEntity Create(TEntity entity);
        TEntity Update(string id, TEntity entity);
        bool Delete(string id);
        System.Threading.Tasks.Task<System.Collections.Generic.ICollection<TEntity>> GetAllAsync(BexioRequestFilter requestParameter = null, System.Threading.CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task<TEntity> GetByIdAsync(string id, System.Threading.CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task<TEntity> CreateAsync(TEntity entity, System.Threading.CancellationToken cancellationToken = default);
    }

    public interface IBexioApiBillEndpoint : IBexioApiV4Endpoint<BexioBill> { }
    public interface IBexioApiExpenseEndpoint : IBexioApiV4Endpoint<BexioExpense> { }
    public interface IBexioApiOutgoingPaymentEndpoint : IBexioApiV4Endpoint<BexioOutgoingPayment> { }
    public interface IBexioApiEmployeeEndpoint : IBexioApiV4Endpoint<BexioEmployee> { }
    public interface IBexioApiAbsenceEndpoint : IBexioApiV4Endpoint<BexioAbsence> { }
}
