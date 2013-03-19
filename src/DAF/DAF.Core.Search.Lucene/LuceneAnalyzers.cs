using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene;
using LN = Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace DAF.Core.Search.Lucene
{
    public class LuceneAnalyzers
    {
        public static Analyzer CreateAnalyzer(string analyzerName, LN.Util.Version version)
        {
            Analyzer analyzer = null;
            if (string.IsNullOrEmpty(analyzerName))
                analyzerName = "standard";
            switch (analyzerName.ToLower())
            {
                case "standard":
                default:
                    analyzer = new StandardAnalyzer(version);
                    break;
            }

            return analyzer;
        }
    }
}
