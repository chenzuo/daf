using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Data
{
    public interface ITransactionManager : IDisposable
    {
        void BeginTransaction();
        void Commit(bool disposeTransaction = true);
        void Rollback();

        bool IsInTransaction { get; }
        bool TransactionCommited { get; }
    }
}
