using bexio_lib.Data;
using bexio_lib.Interfaces.V4;
using bexio_lib.Interfaces;
using bexio_lib.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace bexio_lib.Implementation.Endpoints.V4
{
    /// <summary>
    /// Base for api 4.0 resources: string (uuid) ids, edit with PUT.
    /// </summary>
    public abstract class BexioApiV4Endpoint<TEntity> : BexioApiEndpoint, IBexioApiV4Endpoint<TEntity>
    {
        protected BexioApiV4Endpoint(IBexioApi api, string endpoint) : base(api, BexioApiVersion.V4, endpoint) { }

        public ICollection<TEntity> GetAll(BexioRequestFilter requestParameter = null)
            => this.Send<ICollection<TEntity>>(this.NewRequest().AddRequestData(requestParameter), HttpMethod.Get);

        public TEntity GetById(string id) => this.Send<TEntity>(this.NewRequest(id), HttpMethod.Get);

        public TEntity Create(TEntity entity)
            => this.Send<TEntity>(this.NewRequest().AddRequestBodyData(entity), HttpMethod.Post);

        public TEntity Update(string id, TEntity entity)
            => this.Send<TEntity>(this.NewRequest(id).AddRequestBodyData(entity), HttpMethod.Put);

        public bool Delete(string id)
            => this.API.Send(this.NewRequest(id), HttpMethod.Delete).ToDeleteResult();

        public Task<ICollection<TEntity>> GetAllAsync(BexioRequestFilter requestParameter = null, CancellationToken cancellationToken = default)
            => this.SendAsync<ICollection<TEntity>>(this.NewRequest().AddRequestData(requestParameter), HttpMethod.Get, cancellationToken);

        public Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest(id), HttpMethod.Get, cancellationToken);

        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
            => this.SendAsync<TEntity>(this.NewRequest().AddRequestBodyData(entity), HttpMethod.Post, cancellationToken);
    }

    public class BexioApiBillEndpoint : BexioApiV4Endpoint<BexioBill>, IBexioApiBillEndpoint
    {
        public BexioApiBillEndpoint(IBexioApi api) : base(api, "purchase/bills") { }
    }
    public class BexioApiExpenseEndpoint : BexioApiV4Endpoint<BexioExpense>, IBexioApiExpenseEndpoint
    {
        public BexioApiExpenseEndpoint(IBexioApi api) : base(api, "expenses") { }
    }
    public class BexioApiOutgoingPaymentEndpoint : BexioApiV4Endpoint<BexioOutgoingPayment>, IBexioApiOutgoingPaymentEndpoint
    {
        public BexioApiOutgoingPaymentEndpoint(IBexioApi api) : base(api, "purchase/outgoing-payments") { }
    }
    public class BexioApiEmployeeEndpoint : BexioApiV4Endpoint<BexioEmployee>, IBexioApiEmployeeEndpoint
    {
        public BexioApiEmployeeEndpoint(IBexioApi api) : base(api, "payroll/employees") { }
    }
    public class BexioApiAbsenceEndpoint : BexioApiV4Endpoint<BexioAbsence>, IBexioApiAbsenceEndpoint
    {
        public BexioApiAbsenceEndpoint(IBexioApi api) : base(api, "payroll/absences") { }
    }
}
