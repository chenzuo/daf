using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DAF.Core.Data;
using DAF.Core.DataMonitor;

namespace DAF.Core.Search.Lucene
{
    public abstract class LuceneDataChangeHandler<T> : IDataChangeHandler where T : class
    {
        protected IRepository<T> repository;
        protected ISearchProvider searchProvider;

        public LuceneDataChangeHandler(string connStringName, IRepository<T> repository, ISearchProvider searchProvider)
        {
            this.ConnectionStringName = connStringName;
            this.repository = repository;
            this.searchProvider = searchProvider;
        }

        public string ConnectionStringName { get; protected set; }
        public abstract string TableName { get; }

        public virtual void HandleChange(IEnumerable<ChangeInfo> changedInfo)
        {
            var indexWriter = searchProvider.CreateIndex(typeof(T).Name);
            if (indexWriter == null)
                return;
            foreach (var ci in changedInfo)
            {
                var exp = GetKeyPrediction(ci.Key);
                var obj = repository.Get(exp);
                var dic = obj.ToDictionary();
                switch (ci.Operation)
                {
                    case DataOperation.Insert:
                        indexWriter.AddDocument(dic);
                        break;
                    case DataOperation.Update:
                        indexWriter.UpdateDocument(dic);
                        break;
                    case DataOperation.Delete:
                        indexWriter.RemoveDocument(dic);
                        break;
                    default:
                        break;
                }
            }
        }

        protected abstract Expression<Func<T, bool>> GetKeyPrediction(string key);
    }
}
