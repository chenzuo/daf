using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAF.Core.Data
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> Query(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords);
        T New();
        T Get(Expression<Func<T, bool>> predicate);
        bool Insert(T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null);
        bool Update(T obj, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null);
        bool Delete(T obj, EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null);

        bool UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> setter);
        bool DeleteBatch(Expression<Func<T, bool>> predicate);
    }

    public static class IRepositoryExtensions
    {
        public static IQueryable<T> Query<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IQueryable<T>> orderby = null) where T : class
        {
            int totalRecords = 0;
            return repository.Query(predicate, orderby, 0, 0, out totalRecords);
        }

        public static IQueryable<T> Query<T>(this IRepository<T> repository, int pageIndex, int pageSize, out int totalRecords) where T : class
        {
            return repository.Query(null, null, 0, 0, out totalRecords);
        }

        public static IQueryable<T> Query<T>(this IRepository<T> repository, string predicate, object[] values, Func<IQueryable<T>, IQueryable<T>> orderby = null) where T : class
        {
            int totalRecords = 0;
            return Query(repository, predicate, values, orderby, 0, 0, out totalRecords);
        }

        public static IQueryable<T> Query<T>(this IRepository<T> repository, string predicate, object[] values, Func<IQueryable<T>, IQueryable<T>> orderby, int pageIndex, int pageSize, out int totalRecords) where T : class
        {
            Expression<Func<T, bool>> exp = null;
            if (!string.IsNullOrWhiteSpace(predicate))
                exp = DAF.Core.DynamicExpression.ParseLambda<T, bool>(predicate, values);
            totalRecords = 0;
            return repository.Query(exp, orderby, pageIndex, pageSize, out totalRecords);
        }

        public static bool Save<T>(this IRepository<T> repository, Expression<Func<T, bool>> predicate, T obj, EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null, EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null) where T : class
        {
            if (repository.Query(predicate).Any())
            {
                return repository.Update(obj, updatingHandler, updatedHandler);
            }
            else
            {
                return repository.Insert(obj, insertingHandler, insertedHandler);
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ITransactionManager trans, IEnumerable<T> original, IEnumerable<T> current, Func<T, T, bool> exists
            , bool deleteIfNoFound = true, bool notUpdateIfFound = false
            , EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null
            , EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null
            , EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
            where T : class
        {
            if (deleteIfNoFound)
            {
                if (notUpdateIfFound)
                {
                    return SaveAll(repository, trans, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        null,
                        (repo, o) => { repo.Delete(o, deletingHandler, deletedHandler); });
                }
                else
                {
                    return SaveAll(repository, trans, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        (repo, o, c) => { repo.Update(c, updatingHandler, updatedHandler); },
                        (repo, o) => { repo.Delete(o, deletingHandler, deletedHandler); });
                }
            }
            else
            {
                if (notUpdateIfFound)
                {
                    return SaveAll(repository, trans, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        null,
                        null);
                }
                else
                {
                    return SaveAll(repository, trans, original, current, exists,
                        (repo, o) => { repo.Insert(o, insertingHandler, insertedHandler); },
                        (repo, o, c) => { repo.Update(c, updatingHandler, updatedHandler); },
                        null);
                }
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ITransactionManager trans, IEnumerable<T> original, IEnumerable<T> current, Func<T, T, bool> exists, Action<IRepository<T>, T> actionOriginalNotFound, Action<IRepository<T>, T, T> actionFound, Action<IRepository<T>, T> actionCurrentNotFound)
            where T : class
        {
            try
            {
                trans.BeginTransaction();
                if (original == null || original.Count() <= 0)
                {
                    if (current == null || current.Count() <= 0)
                        return false;

                    if (actionOriginalNotFound != null)
                    {
                        foreach (T obj in current)
                        {
                            actionOriginalNotFound(repository, obj);
                        }
                    }
                    trans.Commit();
                    return true;
                }
                else
                {
                    if (current == null || current.Count() <= 0)
                    {
                        if (actionCurrentNotFound != null)
                        {
                            foreach (T obj in original)
                            {
                                actionCurrentNotFound(repository, obj);
                            }
                        }
                        trans.Commit();
                        return true;
                    }
                    else
                    {
                        if (actionOriginalNotFound != null)
                        {
                            var insertedObjs = current.Where(o => original.Any(c => exists(c, o)) == false);
                            foreach (var obj in insertedObjs)
                            {
                                actionOriginalNotFound(repository, obj);
                            }
                        }

                        if (actionFound != null)
                        {
                            var updatedObjs = current.Where(o => original.Any(c => exists(c, o)) == true);
                            foreach (var obj in updatedObjs)
                            {
                                actionFound(repository, original.First(c => exists(c, obj)), obj);
                            }
                        }

                        if (actionCurrentNotFound != null)
                        {
                            var deletedObjs = original.Where(o => current.Any(c => exists(c, o)) == false);
                            foreach (var obj in deletedObjs)
                            {
                                actionCurrentNotFound(repository, obj);
                            }
                        }
                        trans.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public static bool SaveAll<T>(this IRepository<T> repository, ITransactionManager trans, IEnumerable<T> newItems, IEnumerable<T> modifiedItems, IEnumerable<T> deletedItems
            , EntityEventHandler<T> insertingHandler = null, EntityEventHandler<T> insertedHandler = null
            , EntityEventHandler<T> updatingHandler = null, EntityEventHandler<T> updatedHandler = null
            , EntityEventHandler<T> deletingHandler = null, EntityEventHandler<T> deletedHandler = null)
            where T : class
        {
            try
            {
                trans.BeginTransaction();
                newItems.ForEach(o => repository.Insert(o, insertingHandler, insertedHandler));
                modifiedItems.ForEach(o => repository.Update(o, updatingHandler, updatedHandler));
                deletedItems.ForEach(o => repository.Delete(o, deletingHandler, deletedHandler));
                trans.Commit();
                return true;
            }
            catch(Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }
    }
}
