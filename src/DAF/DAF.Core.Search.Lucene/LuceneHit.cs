using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene;
using LN = Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Analysis.Standard;

namespace DAF.Core.Search.Lucene
{
    public class LuceneHit : IHit
    {
        private string typeName;
        private IDocumentBuilder docBuilder;
        private Document doc;

        public LuceneHit(string typeName, IDocumentBuilder docBuilder, Document doc)
        {
            this.typeName = typeName;
            this.docBuilder = docBuilder;
            this.doc = doc;
        }

        public object GetValue(string fieldName)
        {
            Field f = docBuilder.GetField(typeName, fieldName);
            if (f == null)
                return null;
            string val = doc.Get(fieldName);
            switch (f.FieldType)
            {
                case FieldType.Int:
                    return Convert.ToInt32(val);
                case FieldType.Long:
                    return (long)Convert.ToUInt64(val);
                case FieldType.Float:
                    return (float)Convert.ToDouble(val);
                case FieldType.Double:
                    return Convert.ToDouble(val);
                case FieldType.DateTime:
                    return Convert.ToDateTime(val);
            }
            return val;
        }

        public double Score { get; set; }
    }
}
