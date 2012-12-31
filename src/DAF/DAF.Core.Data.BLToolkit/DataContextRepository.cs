using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using BLToolkit;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Data.Linq;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using DAF.Core.Data;
using DAF.Core.Security;

namespace DAF.Core.Data.BLToolkit
{
    public class DataContextRepository<T> : IRepository<T>
           where T : class, new()
    {
        protected IDataContext db;

        public DataContextRepository(IDataContext db)
        {
            this.db = db;
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            IQueryable<T> query = db.GetTable<T>();
            if (predicate != null)
                query = query.Where(predicate);
            if (orderby != null)
                query = orderby(query);
            if (pageIndex >= 0 && pageSize > 0)
            {
                totalRecords = query.Count();
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            return query;
        }

        public virtual T New()
        {
            return new T();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = db.GetTable<T>();
            if (predicate != null)
                query = query.Where(predicate);
            return query.FirstOrDefault();
        }

        public virtual bool Insert(T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null)
        {
            if (insertingHandler != null)
                insertingHandler(new EntityEventArgs<T>() { Action = DataOperation.Insert, OriginalEntity = null, Entity = obj, Repository = this });

            int ret = db.Insert<T>(obj);

            if (ret > 0 && insertedHandler != null)
                insertedHandler(new EntityEventArgs<T>() { Action = DataOperation.Insert, OriginalEntity = null, Entity = obj, Repository = this });

            return ret > 0;
        }

        public virtual bool Update(T obj, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null)
        {
            if (updatingHandler != null)
                updatingHandler(new EntityEventArgs<T>() { Action = DataOperation.Update, OriginalEntity = obj, Entity = obj, Repository = this });

            int ret = db.Update<T>(obj);

            if (ret > 0 && updatedHandler != null)
                updatedHandler(new EntityEventArgs<T>() { Action = DataOperation.Update, OriginalEntity = obj, Entity = obj, Repository = this });

            return ret > 0;
        }

        public virtual bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
        {
            if (deletingHandler != null)
                deletingHandler(new EntityEventArgs<T>() { Action = DataOperation.Delete, OriginalEntity = obj, Entity = null, Repository = this });

            int ret = db.Delete<T>(obj);

            if (ret > 0 && deletedHandler != null)
                deletedHandler(new EntityEventArgs<T>() { Action = DataOperation.Delete, OriginalEntity = obj, Entity = null, Repository = this });

            return ret > 0;
        }

        public virtual bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter)
        {
            int ret = db.GetTable<T>().Update(predicate, setter);
            return ret > 0;
        }

        public virtual bool DeleteBatch(Expression<Func<T, bool>> predicate)
        {
            int ret = db.GetTable<T>().Delete(predicate);
            return ret > 0;
        }

        public void Dispose()
        {
            if (db == null)
                return;
            db.Dispose();
            db = null;
            GC.SuppressFinalize(this);
        }
    }
}
