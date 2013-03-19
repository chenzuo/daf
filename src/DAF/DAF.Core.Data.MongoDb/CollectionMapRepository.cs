using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DAF.Core.Data;
using DAF.Core.Map;

namespace DAF.Core.Data.MongoDb
{
    public class CollectionMapRepository<U, T> : IMapRepository<U, T>
        where T : class, new()
        where U : class, T, new()
    {
        protected MongoCollection repository;
        protected IMapProvider mapper;
        protected IEnumerable<IRepositoryEventHandler<T>> events;

        public CollectionMapRepository(MongoDatabase db, IMapProvider mapper, IEnumerable<IRepositoryEventHandler<T>> events)
        {
            repository = db.GetCollection<T>(typeof(T).Name);
            this.mapper = mapper;
            this.events = events;
        }

        private IMongoQuery GetKeyQuery(T obj)
        {
            IMongoQuery query = null;
            var keyValues = obj.GetObjectKey();
            if (keyValues != null)
            {
                foreach (var k in keyValues)
                {
                    var q = MongoDB.Driver.Builders.Query.EQ(k.Key, BsonValue.Create(k.Value));
                    if (query == null)
                        query = q;
                    else
                        query = MongoDB.Driver.Builders.Query.And(query, q);
                }
            }
            return query;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            var query = repository.AsQueryable<T>();
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
            IQueryable<T> query = repository.AsQueryable<T>();
            if (predicate != null)
                query = query.Where(predicate);
            return query.FirstOrDefault();
        }

        public virtual bool Insert(T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null)
        {
            obj = this.HandleEvents(events, es => es.InsertingHandler, DataOperation.Insert, null, obj);
            obj = this.HandleEvent(insertingHandler, DataOperation.Insert, null, obj);

            U entity = new U();
            entity = mapper.Map<T, U>(obj, () => entity);

            var ret = repository.Insert<U>(entity);

            if (ret.DocumentsAffected > 0)
            {
                this.HandleEvent(insertedHandler, DataOperation.Insert, null, obj);
                this.HandleEvents(events, es => es.InsertedHandler, DataOperation.Insert, null, obj);
            }

            return ret.DocumentsAffected > 0;
        }

        public virtual bool Update(T obj, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null)
        {
            obj = this.HandleEvents(events, es => es.UpdatingHandler, DataOperation.Update, obj, obj);
            obj = this.HandleEvent(updatingHandler, DataOperation.Update, obj, obj);

            U entity = new U();
            entity = mapper.Map<T, U>(obj, () => entity);

            var ret = repository.Save<U>(entity);

            if (ret.DocumentsAffected > 0)
            {
                this.HandleEvent(updatedHandler, DataOperation.Update, obj, obj);
                this.HandleEvents(events, es => es.UpdatedHandler, DataOperation.Update, obj, obj);
            }

            return ret.DocumentsAffected > 0;
        }

        public virtual bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
        {
            obj = this.HandleEvents(events, es => es.DeletingHandler, DataOperation.Delete, obj, null);
            obj = this.HandleEvent(deletingHandler, DataOperation.Delete, obj, null);

            var ret = repository.Remove(GetKeyQuery(obj));

            if (ret.DocumentsAffected > 0)
            {
                this.HandleEvent(deletedHandler, DataOperation.Delete, obj, null);
                this.HandleEvents(events, es => es.DeletedHandler, DataOperation.Delete, obj, null);
            }

            return ret.DocumentsAffected > 0;
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
                var ret = repository.Insert<U>(entity);
                totals += (int)ret.DocumentsAffected;
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
            var query = repository.AsQueryable<T>();
            if (predicate != null)
                query = query.Where(predicate);
            var objs = query.AsEnumerable();
            objs = this.HandleBatchEvents(events, es => es.BatchUpdatingHandler, DataOperation.Update, objs);
            objs = this.HandleBatchEvent(batchUpdatingHandler, DataOperation.Update, objs);

            long totals = 0;
            var func = setter.Compile();
            foreach (var obj in objs)
            {
                var nobj = func(obj);
                var ret = repository.Save(nobj);
                totals += ret.DocumentsAffected;
            }

            if (totals > 0)
            {
                this.HandleBatchEvent(batchUpdatedHandler, DataOperation.Update, objs);
                this.HandleBatchEvents(events, es => es.BatchUpdatedHandler, DataOperation.Update, objs);
            }

            return totals > 0;
        }

        public virtual bool DeleteBatch(Expression<Func<T, bool>> predicate, BatchEntityEventHandler<T> batchDeletingHandler = null, BatchEntityEventHandler<T> batchDeletedHandler = null)
        {
            var query = repository.AsQueryable<T>();
            if (predicate != null)
                query = query.Where(predicate);
            var objs = query.AsEnumerable();
            objs = this.HandleBatchEvents(events, es => es.BatchDeletingHandler, DataOperation.Delete, objs);
            objs = this.HandleBatchEvent(batchDeletingHandler, DataOperation.Delete, objs);

            long totals = 0;
            foreach (var obj in query)
            {
                var ret = repository.Remove(GetKeyQuery(obj));
                totals += ret.DocumentsAffected;
            }

            if (totals > 0)
            {
                this.HandleBatchEvent(batchDeletedHandler, DataOperation.Delete, objs);
                this.HandleBatchEvents(events, es => es.BatchDeletedHandler, DataOperation.Delete, objs);
            }

            return totals > 0;
        }

        public void Dispose()
        {
            if (repository == null)
                return;
            repository = null;
            GC.SuppressFinalize(this);
        }
    }
}
