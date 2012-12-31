using System;
using System.Web.Http.Filters;
using DAF.Core.Data;

namespace DAF.Web.Api.Filters
{
    public class TransactionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ITransactionManager trans;

        public TransactionFilterAttribute(ITransactionManager trans)
        {
            this.trans = trans;
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            this.trans.Rollback();
        }
    }
}
