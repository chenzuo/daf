using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Data
{
    public class NullTransactionManager : ITransactionManager
    {
        private bool isInTransaction = false;
        private bool transactionCommitted = false;

        public void BeginTransaction()
        {
            isInTransaction = true;
            transactionCommitted = false;
        }

        public void Commit(bool disposeTransaction = true)
        {
            isInTransaction = false;
            transactionCommitted = true;
        }

        public void Rollback()
        {
            isInTransaction = false;
            transactionCommitted = false;
        }

        public bool IsInTransaction
        {
            get { return isInTransaction; }
        }

        public bool TransactionCommited
        {
            get { return transactionCommitted; }
        }

        public void Dispose()
        {
        }
    }
}
