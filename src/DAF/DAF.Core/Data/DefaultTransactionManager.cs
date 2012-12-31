using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using DAF.Core.Logging;

namespace DAF.Core.Data
{
    public class DefaultTransactionManager : ITransactionManager
    {
        private TransactionScope scope;

        public DefaultTransactionManager()
        {
        }

        public void Dispose()
        {
            if (scope != null)
            {
                scope.Dispose();
                scope = null;
            }
        }

        public void BeginTransaction()
        {
            if (scope == null)
            {
                scope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted
                    });

                TransactionCommited = false;
            }
        }

        public void Commit(bool disposeTransaction = true)
        {
            if (scope != null)
            {
                scope.Complete();
                TransactionCommited = true;
                if (disposeTransaction)
                    Dispose();
            }
        }

        public void Rollback()
        {
            if (scope != null)
            {
                scope.Dispose();
                scope = null;
                TransactionCommited = false;
            }
        }

        public bool IsInTransaction { get { return scope != null && !TransactionCommited; } }

        public bool TransactionCommited { get; private set; }
    }
}
