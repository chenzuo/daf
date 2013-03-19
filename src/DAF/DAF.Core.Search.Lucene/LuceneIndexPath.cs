using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Store;

namespace DAF.Core.Search.Lucene
{
    public class LuceneIndexPath
    {
        private string path;
        private Analyzer analyzer;
        private IndexSearcher searcher;
        private IndexWriter writer;

        public LuceneIndexPath(Analyzer analyzer, string path)
        {
            this.analyzer = analyzer;
            this.path = path;
        }

        public string Path { get { return path; } }
        public Analyzer Analyzer { get { return analyzer; } }
        public bool IsDirty { get; set; }

        public IndexSearcher Searcher 
        {
            get
            {
                if (searcher == null)
                {
                    searcher = new IndexSearcher(FSDirectory.Open(path));
                }
                return searcher;
            }
        }

        public IndexWriter Writer
        {
            get
            {
                if (writer == null)
                {
                    writer = new IndexWriter(FSDirectory.Open(path), analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
                }
                return writer;
            }
        }
    }
}
