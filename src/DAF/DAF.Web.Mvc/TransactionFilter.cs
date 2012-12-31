using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAF.Core.Data;

namespace DAF.Web.Mvc
{
    public class TransactionFilter : IExceptionFilter, IMvcFilter
    {
        private readonly ITransactionManager trans;

        public TransactionFilter(ITransactionManager trans)
        {
            this.trans = trans;
        }
        
        public void OnException(ExceptionContext filterContext)
        {
            this.trans.Rollback();
        }

        public bool AllowMultiple
        {
            get { return true; }
        }

        public int Order
        {
            get { return (int)FilterScope.Global; }
        }
    }
}
