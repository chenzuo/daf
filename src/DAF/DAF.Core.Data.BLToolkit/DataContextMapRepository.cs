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
using DAF.Core.Map;

namespace DAF.Core.Data.BLToolkit
{
    public class DataContextMapRepository<U, T> : IMapRepository<U, T>
        where T : class, new()
        where U : class, T, new()
    {
        protected IDataContext db;
        protected IMapProvider mapper;

        public DataContextMapRepository(IDataContext db, IMapProvider mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            IQueryable<T> query = db.GetTable<U>();
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
            return new U();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = db.GetTable<U>();
            if (predicate != null)
                query = query.Where(predicate);
            return query.FirstOrDefault();
        }

        public virtual bool Insert(T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null)
        {
            U entity = new U();
            entity = mapper.Map<T, U>(obj, () => entity);

            if (insertingHandler != null)
                insertingHandler(new EntityEventArgs<T>() { Action = DataOperation.Insert, OriginalEntity = null, Entity = obj, Repository = this });

            int ret = db.Insert<U>(entity);

            if (ret > 0 && insertedHandler != null)
                insertedHandler(new EntityEventArgs<T>() { Action = DataOperation.Insert, OriginalEntity = null, Entity = obj, Repository = this });

            return ret > 0;
        }

        public virtual bool Update(T obj, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null)
        {
            U entity = new U();
            entity = mapper.Map<T, U>(obj, () => entity);

            if (updatingHandler != null)
                updatingHandler(new EntityEventArgs<T>() { Action = DataOperation.Update, OriginalEntity = obj, Entity = obj, Repository = this });

            int ret = db.Update<U>(entity);

            if (ret > 0 && updatedHandler != null)
                updatedHandler(new EntityEventArgs<T>() { Action = DataOperation.Update, OriginalEntity = obj, Entity = obj, Repository = this });
            
            return ret > 0;
        }

        public virtual bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
        {
            U entity = new U();
            entity = mapper.Map<T, U>(obj, () => entity);

            if (deletingHandler != null)
                deletingHandler(new EntityEventArgs<T>() { Action = DataOperation.Delete, OriginalEntity = obj, Entity = null, Repository = this });

            int ret = db.Delete<U>(entity);

            if (ret > 0 && deletedHandler != null)
                deletedHandler(new EntityEventArgs<T>() { Action = DataOperation.Delete, OriginalEntity = obj, Entity = null, Repository = this });

            return ret > 0;
        }

        public virtual bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter)
        {
            int ret = db.GetTable<U>().Update(predicate, setter);
            return ret > 0;
        }

        public virtual bool DeleteBatch(Expression<Func<T, bool>> predicate)
        {
            int ret = db.GetTable<U>().Delete(predicate);
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
