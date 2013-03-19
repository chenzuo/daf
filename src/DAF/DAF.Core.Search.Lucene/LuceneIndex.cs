using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lucene;
using LN = Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Analysis.Standard;
using DAF.Core;
using DAF.Core.Search;

namespace DAF.Core.Search.Lucene
{
    public class LuceneIndex : IIndex
    {
        private Analyzer analyzer;
        private Dictionary<string, LuceneIndexPath> indexPaths;
        private Document document;
        private LN.Util.Version version;

        public LuceneIndex(string typeName, string indexDir, IDocumentBuilder docBuilder, IIndexPathBuilder pathBuilder, LN.Util.Version version)
        {
            this.TypeName = typeName;
            this.Directory = indexDir;
            this.version = version;
            this.DocumentBuilder = docBuilder;
            this.IndexPathBuilder = pathBuilder;
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

        private void EnsureDocument()
        {
            if (document == null)
            {
                document = new Document();
                var fields = DocumentBuilder.GetFields(TypeName);
                foreach (var f in fields)
                {
                    AbstractField af = null;
                    switch (f.FieldType)
                    {
                        case FieldType.Int:
                        case FieldType.Float:
                        case FieldType.Long:
                        case FieldType.Double:
                            af = new LN.Documents.NumericField(f.FieldName, (LN.Documents.Field.Store)((int)f.StoreMode), (int)f.IndexMode > 0);
                            break;
                        case FieldType.String:
                        case FieldType.DateTime:
                        default:
                            af = new LN.Documents.Field(f.FieldName, string.Empty, (LN.Documents.Field.Store)((int)f.StoreMode), (LN.Documents.Field.Index)((int)f.IndexMode));
                            break;
                    }
                    af.Boost = f.Boost;
                    document.Add(af);
                }
            }
        }

        private void SetDocumentValue(IDictionary<string, object> values)
        {
            EnsureDocument();
            foreach (var kvp in values)
            {
                var f = document.GetFieldable(kvp.Key);
                if (f != null)
                {
                    if (kvp.Value == null)
                    {
                        ((LN.Documents.Field)f).SetValue(string.Empty);
                    }
                    else
                    {
                        Field p = DocumentBuilder.GetField(TypeName, kvp.Key);
                        switch (p.FieldType)
                        {
                            case FieldType.Int:
                                ((NumericField)f).SetIntValue((int)kvp.Value);
                                break;
                            case FieldType.Long:
                                ((NumericField)f).SetLongValue((long)kvp.Value);
                                break;
                            case FieldType.Float:
                                ((NumericField)f).SetFloatValue((float)kvp.Value);
                                break;
                            case FieldType.Double:
                                ((NumericField)f).SetDoubleValue((double)kvp.Value);
                                break;
                            case FieldType.DateTime:
                                ((LN.Documents.Field)f).SetValue(DateTools.DateToString((DateTime)kvp.Value, DateTools.Resolution.MILLISECOND));
                                break;
                            case FieldType.String:
                            default:
                                ((LN.Documents.Field)f).SetValue(kvp.Value.ToString());
                                break;
                        }
                    }
                }
            }
        }

        private LuceneIndexPath GetIndexPath(IDictionary<string, object> values)
        {
            string path = Path.Combine(Directory, TypeName, IndexPathBuilder.BuildIndexPath(values)).ToLower();
            if (indexPaths.ContainsKey(path))
            {
                return indexPaths[path];
            }
            EnsureAnalyzer();
            LuceneIndexPath p = new LuceneIndexPath(analyzer, path);
            lock (indexPaths)
            {
                indexPaths.Add(path, p);
            }
            return p;
        }

        public void Initialize()
        {
        }

        public void AddDocument(IDictionary<string, object> values, float boost = 1.0f)
        {
            if (values == null)
                return;
            var ip = GetIndexPath(values);
            SetDocumentValue(values);
            document.Boost = boost;
            ip.IsDirty = true;
            ip.Writer.AddDocument(document);
        }

        public void UpdateDocument(IDictionary<string, object> values)
        {
            if (values == null)
                return;
            var ip = GetIndexPath(values);
            if (ip != null)
            {
                Document doc = null;
                var key = DocumentBuilder.GetKey(TypeName, values);
                var tq = new TermQuery(new Term(key.Key, key.Value));
                var docs = ip.Searcher.Search(tq, 1);
                if (docs != null && docs.TotalHits == 1)
                {
                    doc = ip.Searcher.Doc(docs.ScoreDocs[0].Doc);
                }
                if (doc != null)
                {
                    ip.IsDirty = true;
                    ip.Writer.UpdateDocument(new Term(key.Key, key.Value), doc);
                }
            }
        }

        public void RemoveDocument(IDictionary<string, object> values)
        {
            if (values == null)
                return;
            var ip = GetIndexPath(values);
            if (ip != null)
            {
                Document doc = null;
                var key = DocumentBuilder.GetKey(TypeName, values);
                var tq = new TermQuery(new Term(key.Key, key.Value));
                var docs = ip.Searcher.Search(tq, 1);
                if (docs != null && docs.TotalHits == 1)
                {
                    doc = ip.Searcher.Doc(docs.ScoreDocs[0].Doc);
                }
                if (doc != null)
                {
                    ip.IsDirty = true;
                    ip.Writer.DeleteDocuments(new Term(key.Key, key.Value));
                }
            }
        }

        public void Save()
        {
            var dirtyPaths = indexPaths.Values.Where(o => o.IsDirty);
            dirtyPaths.ForEach(o => { o.Writer.Commit(); o.Writer.Flush(true, true, true); });
        }

        public void Optimize()
        {
            indexPaths.Values.ForEach(o =>
                {
                    try
                    {
                        o.Writer.Optimize();
                    }
                    catch
                    {
                        o.Writer.Dispose();
                    }
                });
        }

        public void Flush()
        {
            var dirtyPaths = indexPaths.Values.Where(o => o.IsDirty);
            foreach (var p in dirtyPaths)
            {
                p.Writer.Flush(true, true, true);
            }
        }

        public void Close()
        {
            indexPaths.Values.ForEach(o =>
            {
                try
                {
                    o.Writer.Dispose();
                }
                catch
                {
                }
            });
        }
    }
}
