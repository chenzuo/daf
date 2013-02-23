﻿using System;
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
        protected IEnumerable<IRepositoryEventHandler<T>> events;

        public DataContextMapRepository(IDataContext db, IMapProvider mapper, IEnumerable<IRepositoryEventHandler<T>> events)
        {
            this.db = db;
            this.mapper = mapper;
            this.events = events;
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

            obj = this.HandleEvents(events, es => es.InsertingHandler, DataOperation.Insert, null, obj);
            obj = this.HandleEvent(insertingHandler, DataOperation.Insert, null, obj);

            int ret = db.Insert<U>(entity);

            if (ret > 0)
            {
                this.HandleEvent(insertedHandler, DataOperation.Insert, null, obj);
                this.HandleEvents(events, es => es.InsertedHandler, DataOperation.Insert, null, obj);
            }

            return ret > 0;
        }

        public virtual bool Update(T obj, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null)
        {
            U entity = new U();
            entity = mapper.Map<T, U>(obj, () => entity);

            obj = this.HandleEvents(events, es => es.UpdatingHandler, DataOperation.Update, obj, obj);
            obj = this.HandleEvent(updatingHandler, DataOperation.Update, obj, obj);

            int ret = db.Update<U>(entity);

            if (ret > 0)
            {
                this.HandleEvent(updatedHandler, DataOperation.Update, obj, obj);
                this.HandleEvents(events, es => es.UpdatedHandler, DataOperation.Update, obj, obj);
            }
            
            return ret > 0;
        }

        public virtual bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
        {
            U entity = new U();
            entity = mapper.Map<T, U>(obj, () => entity);

            obj = this.HandleEvents(events, es => es.DeletingHandler, DataOperation.Delete, obj, null);
            obj = this.HandleEvent(deletingHandler, DataOperation.Delete, obj, null);

            int ret = db.Delete<U>(entity);

            if (ret > 0)
            {
                this.HandleEvent(deletedHandler, DataOperation.Delete, obj, null);
                this.HandleEvents(events, es => es.DeletedHandler, DataOperation.Delete, obj, null);
            }

            return ret > 0;
        }

        public virtual bool InsertBatch(IEnumerable<T> objs, BatchEntityEventHandler<T> batchInsertingHandler = null, BatchEntityEventHandler<T> batchInsertedHandler = null)
        {            
            objs = this.HandleBatchEvents(events, es => es.BatchInsertingHandler, DataOperation.Insert, objs);
            objs = this.HandleBatchEvent(batchInsertingHandler, DataOperation.Insert, objs);

            int totals = 0;
            foreach (var obj in objs)
            {
                U entity = new U();
                entity = mapper.Map<T, U>(obj, () => entity);
                totals += db.Insert<U>(entity);
            }

            if (totals > 0)
            {
                this.HandleBatchEvent(batchInsertedHandler, DataOperation.Insert, objs);
                this.HandleBatchEvents(events, es => es.BatchInsertedHandler, DataOperation.Insert, objs);
            }

            return totals > 0;
        }

        public virtual bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter, BatchEntityEventHandler<T> batchUpdatingHandler = null, BatchEntityEventHandler<T> batchUpdatedHandler = null)
        {
            bool hasHandle = events != null && events.Any(e => e.CanHandle()) || batchUpdatingHandler != null || batchUpdatedHandler != null;
            int totals = 0;
            if (hasHandle)
            {
                var objs = db.GetTable<U>().Where(predicate).AsEnumerable();
                objs = this.HandleBatchEvents(events, es => es.BatchUpdatingHandler, DataOperation.Update, objs);
                objs = this.HandleBatchEvent(batchUpdatingHandler, DataOperation.Update, objs);

                var func = setter.Compile();
                foreach (var obj in objs)
                {
                    var nobj = func(obj);
                    U entity = new U();
                    entity = mapper.Map<T, U>(nobj, () => entity);
                    totals += db.Update<U>(entity);
                }
                if (totals > 0)
                {
                    this.HandleBatchEvent(batchUpdatedHandler, DataOperation.Update, objs);
                    this.HandleBatchEvents(events, es => es.BatchUpdatedHandler, DataOperation.Update, objs);
                }
            }
            else
            {
                totals = db.GetTable<U>().Update(predicate, setter);
            }

            return totals > 0;
        }

        public virtual bool DeleteBatch(Expression<Func<T, bool>> predicate, BatchEntityEventHandler<T> batchDeletingHandler = null, BatchEntityEventHandler<T> batchDeletedHandler = null)
        {
            bool hasHandle = events != null && events.Any(e => e.CanHandle()) || batchDeletingHandler != null || batchDeletedHandler != null;
            int totals = 0;
            if (hasHandle)
            {
                var objs = db.GetTable<U>().Where(predicate).AsEnumerable();
                objs = this.HandleBatchEvents(events, es => es.BatchDeletingHandler, DataOperation.Delete, objs);
                objs = this.HandleBatchEvent(batchDeletingHandler, DataOperation.Delete, objs);

                foreach (var obj in objs)
                {
                    U entity = new U();
                    entity = mapper.Map<T, U>(obj, () => entity);
                    totals += db.Delete<U>(entity);
                }
                if (totals > 0)
                {
                    this.HandleBatchEvent(batchDeletedHandler, DataOperation.Delete, objs);
                    this.HandleBatchEvents(events, es => es.BatchDeletedHandler, DataOperation.Delete, objs);
                }
            }
            else
            {

                totals = db.GetTable<U>().Delete(predicate);
            }

            return totals > 0;
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
