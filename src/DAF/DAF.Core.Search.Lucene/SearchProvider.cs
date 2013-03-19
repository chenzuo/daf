using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LN = Lucene.Net;
using DAF.Core;

namespace DAF.Core.Search.Lucene
{
    public class SearchProvider : ISearchProvider
    {
        private string directory;
        private ConcurrentDictionary<string, IIndex> indexers;
        private ConcurrentDictionary<string, ISearch> searchers;
        private LN.Util.Version version = LN.Util.Version.LUCENE_30;

        public SearchProvider(string directory)
        {
            this.directory = directory;
            indexers = new ConcurrentDictionary<string, IIndex>();
            searchers = new ConcurrentDictionary<string, ISearch>();
        }

        public void Initialize()
        {
            foreach (var kvp in AttributedTypes.SearchTypes)
            {
                var docAttr = kvp.Value;
                var docBuilder = Activator.CreateInstance(docAttr.DocumentBuilder) as IDocumentBuilder;
                var pathBuilder = Activator.CreateInstance(docAttr.IndexPathBuilder) as IIndexPathBuilder;
                var fieldNameProvider = Activator.CreateInstance(docAttr.FacetFieldNameProvider) as IFacetFieldNameProvider;

                var indexer = new LuceneIndex(kvp.Key, directory, docBuilder, pathBuilder, version);
                indexer.Initialize();
                indexers.AddOrUpdate(kvp.Key, indexer, (k, os) => os);

                var searcher = new LuceneSearch(kvp.Key, directory, docBuilder, pathBuilder, fieldNameProvider, version);
                searcher.Initialize();
                searchers.AddOrUpdate(kvp.Key, searcher, (k, os) => os);
            }
        }

        public IIndex CreateIndex(string typeName)
        {
            if (indexers.ContainsKey(typeName))
            {
                return indexers.GetOrAdd(typeName, (IIndex)null);
            }
            return null;
        }

        public ISearch CreateSearch(string typeName)
        {
            if (searchers.ContainsKey(typeName))
            {
                return searchers.GetOrAdd(typeName, (ISearch)null);
            }
            return null;
        }

        public void Close()
        {
            indexers.Values.ForEach(o => o.Close());
            searchers.Values.ForEach(o => o.Close());
        }
    }
}
