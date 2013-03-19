using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Lucene;
using LN = Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search.Spans;
using Lucene.Net.Util;
using DAF.Core;
using DAF.Core.Search;

namespace DAF.Core.Search.Lucene
{
    public class LuceneSearch : ISearch
    {
        private static string[] SpecialCharacters = new string[] {"\\", "+", "-", "&&", "||", "!", "(", ")", "{", "}", "[", "]", "^", "\"", "~", "*", "?", ":" };
        private Analyzer analyzer;
        private IFacetFieldNameProvider facetFieldNameProvider;
        private Dictionary<string, LuceneIndexPath> indexPaths;
        private LN.Util.Version version;
        private List<string> searchPaths;
        private BooleanQuery query;
        private List<SortField> sortFields;
        //private List<Filter> filters;
        private List<string> facetFields;

        public LuceneSearch(string typeName, string indexDir, IDocumentBuilder docBuilder, IIndexPathBuilder pathBuilder, IFacetFieldNameProvider facetFieldNameProvider, LN.Util.Version version)
        {
            this.TypeName = typeName;
            this.Directory = indexDir;
            this.version = version;
            this.DocumentBuilder = docBuilder;
            this.IndexPathBuilder = pathBuilder;
            this.facetFieldNameProvider = facetFieldNameProvider;
            indexPaths = new Dictionary<string, LuceneIndexPath>();
        }

        public string TypeName { get; private set; }

        public string Directory { get; private set; }

        public IDocumentBuilder DocumentBuilder { get; private set; }

        public IIndexPathBuilder IndexPathBuilder { get; private set; }

        private void EnsureAnalyzer()
        {
            if (analyzer == null)
            {
                string analyzerName = DocumentBuilder.GetAnalyzer(TypeName);
                analyzer = LuceneAnalyzers.CreateAnalyzer(analyzerName, version);
            }
        }

        private void EnsureIndexPaths()
        {
            if (indexPaths.Count <= 0)
            {
                EnsureAnalyzer();
                var dirs = IndexPathBuilder.GetAllPath(Path.Combine(Directory, TypeName));
                dirs.ForEach(d =>
                    {
                        var p = new LuceneIndexPath(analyzer, d);
                        indexPaths.Add(d.ToLower(), p);
                    });
            }
        }

        public void Initialize()
        {
        }

        public void NewQuery()
        {
            EnsureIndexPaths();
            query = new BooleanQuery();
            searchPaths = new List<string>();
            sortFields = new List<SortField>();
            //filters = new List<Filter>();
            facetFields = new List<string>();
        }

        public void Scope(IDictionary<string, object> values)
        {
            var paths = IndexPathBuilder.BuildSearchPaths(values);
            foreach (var p in paths)
            {
                if(indexPaths.ContainsKey(p.ToLower()))
                    searchPaths.Add(Path.Combine(Directory, p));
            }
        }

        public void Where(string fieldName, string word, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f)
        {
            QueryParser qp = new QueryParser(version, fieldName, analyzer);
            var q = qp.Parse(word);
            q.Boost = boost;
            query.Add(q, (Occur)((int)occur));
        }

        public void Equals(string fieldName, string word, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f)
        {
            TermQuery tq = new TermQuery(new Term(fieldName, word));
            tq.Boost = boost;
            query.Add(tq, (Occur)((int)occur));
        }

        public void Wildcard(string fieldName, string word, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f)
        {
            WildcardQuery wq = new WildcardQuery(new Term(fieldName, word));
            wq.Boost = boost;
            query.Add(wq, (Occur)((int)occur));
        }

        public void Range(string fieldName, object lowerValue, object upperValue, bool includeLower = true, bool includeUpper = true, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f)
        {
            Query q = null;
            var f = DocumentBuilder.GetField(TypeName, fieldName);
            switch (f.FieldType)
            {
                case FieldType.Int:
                    q = NumericRangeQuery.NewIntRange(fieldName, (int)lowerValue, (int)upperValue, includeLower, includeUpper);
                    break;
                case FieldType.Long:
                    q = NumericRangeQuery.NewLongRange(fieldName, (long)lowerValue, (long)upperValue, includeLower, includeUpper);
                    break;
                case FieldType.Float:
                    q = NumericRangeQuery.NewFloatRange(fieldName, (float)lowerValue, (float)upperValue, includeLower, includeUpper);
                    break;
                case FieldType.Double:
                    q = NumericRangeQuery.NewDoubleRange(fieldName, (double)lowerValue, (double)upperValue, includeLower, includeUpper);
                    break;
                case FieldType.DateTime:
                    q = new TermRangeQuery(fieldName, DateTools.DateToString((DateTime)lowerValue, DateTools.Resolution.MILLISECOND), DateTools.DateToString((DateTime)upperValue, DateTools.Resolution.MILLISECOND), includeLower, includeUpper);
                    break;
                case FieldType.String:
                default:
                    q = new TermRangeQuery(fieldName, lowerValue.ToString(), upperValue.ToString(), includeLower, includeUpper);
                    break;
            }
            q.Boost = boost;
            query.Add(q, (Occur)((int)occur));
        }

        public void Prefix(string fieldName, string prefix, ClauseOccur occur = ClauseOccur.SHOULD, float boost = 1.0f)
        {
            var q = new PrefixQuery(new Term(fieldName, prefix));
            q.Boost = boost;
            query.Add(q, (Occur)((int)occur));
        }
        /*
                public void Filter(string fieldName, object lowerValue, object upperValue, bool includeLower = true, bool includeUpper = true)
                {
                    Filter q = null;
                    var f = DocumentBuilder.GetField(TypeName, fieldName);
                    switch (f.FieldType)
                    {
                        case FieldType.Int:
                            q = NumericRangeFilter.NewIntRange(fieldName, (int)lowerValue, (int)upperValue, includeLower, includeUpper);
                            break;
                        case FieldType.Long:
                            q = NumericRangeFilter.NewLongRange(fieldName, (long)lowerValue, (long)upperValue, includeLower, includeUpper);
                            break;
                        case FieldType.Float:
                            q = NumericRangeFilter.NewFloatRange(fieldName, (float)lowerValue, (float)upperValue, includeLower, includeUpper);
                            break;
                        case FieldType.Double:
                            q = NumericRangeFilter.NewDoubleRange(fieldName, (double)lowerValue, (double)upperValue, includeLower, includeUpper);
                            break;
                        case FieldType.DateTime:
                            q = new TermRangeFilter(fieldName, DateTools.DateToString((DateTime)lowerValue, DateTools.Resolution.MILLISECOND), DateTools.DateToString((DateTime)upperValue, DateTools.Resolution.MILLISECOND), includeLower, includeUpper);
                            break;
                        case FieldType.String:
                        default:
                            q = new TermRangeFilter(fieldName, lowerValue.ToString(), upperValue.ToString(), includeLower, includeUpper);
                            break;
                    }
                    filters.Add(q);
                }

                public void Filter(string fieldName, string prefix)
                {
                    PrefixFilter pf = new PrefixFilter(new Term(fieldName, prefix));
                    filters.Add(pf);
                }
        */
        public void SortBy(string fieldName, bool descending = false)
        {
            int comparer = 0;
            var f = DocumentBuilder.GetField(TypeName, fieldName);
            switch (f.FieldType)
            {
                case FieldType.Int:
                    comparer = SortField.INT;
                    break;
                case FieldType.Long:
                    comparer = SortField.LONG;
                    break;
                case FieldType.Float:
                    comparer = SortField.FLOAT;
                    break;
                case FieldType.Double:
                    comparer = SortField.DOUBLE;
                    break;
                case FieldType.DateTime:
                case FieldType.String:
                    comparer = SortField.STRING;
                    break;
                default:
                    comparer = SortField.SCORE;
                    break;
            }
            SortField sf = new SortField(fieldName, comparer, descending);
            sortFields.Add(sf);
        }

        public void Facet(params string[] fieldNames)
        {
            if (fieldNames != null && fieldNames.Length > 0)
            {
                facetFields.AddRange(fieldNames);
            }
        }

        public IEnumerable<IHit> Query(int pageIndex, int pageSize, out int totalCount, out IEnumerable<FacetGroup> facetedResults)
        {
            totalCount = 0;
            facetedResults = null;

            if (searchPaths == null || searchPaths.Count <= 0)
                searchPaths.AddRange(indexPaths.Values.Select(o => o.Path));

            List<LuceneHit> results = new List<LuceneHit>();

            List<IndexSearcher> subSearchs = new List<IndexSearcher>();

            searchPaths.ForEach(o => subSearchs.Add(new IndexSearcher(FSDirectory.Open(o))));

            if (facetFields != null && facetFields.Count > 0)
            {
                var facetGroups = new List<FacetGroup>();
                var mainQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(query));
                MultiReader readers = new MultiReader(subSearchs.Select(o => o.IndexReader).ToArray());

                foreach (var facetField in facetFields)
                {
                    FacetGroup fg = new FacetGroup();
                    fg.FieldName = facetFieldNameProvider.GetMapName(TypeName, facetField);
                    var items = new List<FacetItem>();

                    var allDistinctField = FieldCache_Fields.DEFAULT.GetStrings(readers, facetField).Distinct().ToArray();
                    int totalHits = 0;

                    Parallel.ForEach(allDistinctField, fieldValue =>
                        {
                        //foreach (var fieldValue in allDistinctField)
                        //{
                            var facetQuery = new TermQuery(new Term(facetField, fieldValue));
                            var facetQueryFilter = new CachingWrapperFilter(new QueryWrapperFilter(facetQuery));

                            var bs = new OpenBitSetDISI(facetQueryFilter.GetDocIdSet(readers).Iterator(), readers.MaxDoc);
                            bs.InPlaceAnd(mainQueryFilter.GetDocIdSet(readers).Iterator());
                            int count = (Int32)bs.Cardinality();

                            FacetItem item = new FacetItem();
                            item.GroupValue = fieldValue;
                            item.Count = count;

                            items.Add(item);
                            totalHits += count;
                        }
                    );

                    fg.FacetItems = items.OrderByDescending(o => o.Count);
                    fg.TotalHits = totalHits;

                    facetGroups.Add(fg);
                }

                facetedResults = facetGroups.OrderBy(o => o.FieldName);
            }
            ParallelMultiSearcher searcher = new ParallelMultiSearcher(subSearchs.ToArray());
            Sort sort = null;
            if (sortFields != null && sortFields.Count > 0)
            {
                sort = new Sort(sortFields.ToArray());
            }

            int maxDoc = searcher.MaxDoc;
            int startIndex = 0;
            if (pageIndex >= 0 && pageSize > 0)
            {
                startIndex = pageIndex * pageSize;
                maxDoc = pageSize * (pageIndex + 1);
            }
            var docs = sort == null ?  searcher.Search(query, null, maxDoc) : searcher.Search(query, null, maxDoc, sort);
            totalCount = docs.TotalHits;
            int endIndex = docs.TotalHits - startIndex;
            for (int i = startIndex; i < endIndex; i++)
            {
                LuceneHit h = new LuceneHit(TypeName, DocumentBuilder, searcher.Doc(docs.ScoreDocs[i].Doc));
                results.Add(h);
            }
            return results;
        }

        public void Close()
        {
            indexPaths.Values.ForEach(o =>
            {
                try
                {
                    o.Searcher.Dispose();
                }
                catch
                {
                }
            });
        }
    }
}
