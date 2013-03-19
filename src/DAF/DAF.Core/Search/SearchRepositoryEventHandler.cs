using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;

namespace DAF.Core.Search
{
    public class SearchRepositoryEventHandler<T> : IRepositoryEventHandler<T> where T : class
    {
        private IIndex indexer;

        public SearchRepositoryEventHandler(ISearchProvider searchProvider)
        {
            this.indexer = searchProvider.CreateIndex(typeof(T).Name);
            InitHandlers();
        }

        private void InitHandlers()
        {
            if (CanHandle())
            {
                InsertedHandler = delegate(EntityEventArgs<T> args)
                {
                    var values = args.Entity.ToDictionary();
                    if (values != null && values.Count > 0)
                    {
                        indexer.AddDocument(values);
                        indexer.Save();
                    }
                };
                UpdatedHandler = delegate(EntityEventArgs<T> args)
                {
                    var values = args.Entity.ToDictionary();
                    if (values != null && values.Count > 0)
                    {
                        indexer.UpdateDocument(values);
                        indexer.Save();
                    }
                };
                DeletedHandler = delegate(EntityEventArgs<T> args)
                {
                    var values = args.OriginalEntity.ToDictionary();
                    if (values != null && values.Count > 0)
                    {
                        indexer.RemoveDocument(values);
                        indexer.Save();
                    }
                };
                BatchInsertedHandler = delegate(EntitiesEventArgs<T> args)
                {
                    foreach (var entity in args.Entities)
                    {
                        var values = entity.ToDictionary();
                        if (values != null && values.Count > 0)
                        {
                            indexer.AddDocument(values);
                        }
                    }
                    indexer.Save();
                };
                BatchUpdatedHandler = delegate(EntitiesEventArgs<T> args)
                {
                    foreach (var entity in args.Entities)
                    {
                        var values = entity.ToDictionary();
                        if (values != null && values.Count > 0)
                        {
                            indexer.UpdateDocument(values);
                        }
                    }
                    indexer.Save();
                };
                BatchDeletedHandler = delegate(EntitiesEventArgs<T> args)
                {
                    foreach (var entity in args.Entities)
                    {
                        var values = entity.ToDictionary();
                        if (values != null && values.Count > 0)
                        {
                            indexer.RemoveDocument(values);
                        }
                    }
                    indexer.Save();
                };
            }
        }

        public bool CanHandle()
        {
            return AttributedTypes.SearchTypes.ContainsKey(typeof(T).Name);
        }

        public EntityEventHandler<T> InsertingHandler { get; set; }
        public EntityEventHandler<T> InsertedHandler { get; set; }
        public EntityEventHandler<T> UpdatingHandler { get; set; }
        public EntityEventHandler<T> UpdatedHandler { get; set; }
        public EntityEventHandler<T> DeletingHandler { get; set; }
        public EntityEventHandler<T> DeletedHandler { get; set; }

        public BatchEntityEventHandler<T> BatchInsertingHandler { get; set; }
        public BatchEntityEventHandler<T> BatchInsertedHandler { get; set; }
        public BatchEntityEventHandler<T> BatchUpdatingHandler { get; set; }
        public BatchEntityEventHandler<T> BatchUpdatedHandler { get; set; }
        public BatchEntityEventHandler<T> BatchDeletingHandler { get; set; }
        public BatchEntityEventHandler<T> BatchDeletedHandler { get; set; }
    }
}
