using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using DAF.Core.Data;

namespace DAF.Core.Data.EF
{
    public class DbContextRepository<T> : IRepository<T> where T : class, new()
    {
        protected DbContext db;

        public DbContextRepository(DbContext db)
        {
            this.db = db;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            IQueryable<T> query = db.Set<T>();
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
            IQueryable<T> query = db.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            return query.FirstOrDefault();
        }

        public virtual bool Insert(T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null)
        {
            if (insertingHandler != null)
                insertingHandler(new EntityEventArgs<T>() { Action = DataOperation.Insert, OriginalEntity = null, Entity = obj, Repository = this });

            var tbl = db.Set<T>();
            obj = tbl.Add(obj);

            int ret = db.SaveChanges();

            if (ret > 0 && insertedHandler != null)
                insertedHandler(new EntityEventArgs<T>() { Action = DataOperation.Insert, OriginalEntity = null, Entity = obj, Repository = this });

            return ret > 0;
        }

        public virtual bool Update(T obj, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null)
        {
            if (updatingHandler != null)
                updatingHandler(new EntityEventArgs<T>() { Action = DataOperation.Update, OriginalEntity = obj, Entity = obj, Repository = this });

            var tbl = db.Set<T>();
            if (!tbl.Local.Contains(obj))
            {
                db.Entry<T>(obj).State = EntityState.Modified;
            }
            else
            {
                db.Entry<T>(obj).CurrentValues.SetValues(obj);
            }

            int ret = db.SaveChanges();

            if (ret > 0 && updatedHandler != null)
                updatedHandler(new EntityEventArgs<T>() { Action = DataOperation.Update, OriginalEntity = obj, Entity = obj, Repository = this });

            return ret > 0;
        }

        public virtual bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
        {
            if (deletingHandler != null)
                deletingHandler(new EntityEventArgs<T>() { Action = DataOperation.Delete, OriginalEntity = obj, Entity = null, Repository = this });

            var tbl = db.Set<T>();
            if (!tbl.Local.Contains(obj))
            {
                db.Entry(obj).State = EntityState.Deleted;
            }
            else
            {
                db.Set<T>().Remove(obj);
            }

            int ret = db.SaveChanges();

            if (ret > 0 && deletedHandler != null)
                deletedHandler(new EntityEventArgs<T>() { Action = DataOperation.Delete, OriginalEntity = obj, Entity = null, Repository = this });

            return ret > 0;
        }

        public virtual bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter)
        {
            IQueryable<T> query = db.Set<T>();
            if (predicate != null)
                query = query.Where(predicate);
            foreach (var entity in query)
                setter.Compile().Invoke(entity);

            int ret = db.SaveChanges();

            return ret > 0;
        }

        public virtual bool DeleteBatch(Expression<Func<T, bool>> predicate)
        {
            var tbl = db.Set<T>();
            IQueryable<T> query = tbl;
            if (predicate != null)
                query = query.Where(predicate);
            foreach (var entity in query)
                tbl.Remove(entity);

            int ret = db.SaveChanges();

            return ret > 0;
        }

        public void Dispose()
        {
            //if (db == null)
            //    return;
            //db.Dispose();
            //db = null;
            //GC.SuppressFinalize(this);
        }
    }
}
