using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAF.Core;
using DAF.Core.Search;
using DAF.Core.Search.Lucene;
using Lucene.Net;
using Version = Lucene.Net.Util.Version;

namespace DAF.Core.Search.LuceneTest
{
    [TestClass]
    public class IndexWriteTest : TestBase
    {
       
        [TestMethod]
        public void Save()
        {
            var data = PrepareData();
            LuceneIndex db = new LuceneIndex(typeName, directory, mockDocBuilder.Object, mockPathBuilder.Object, Version.LUCENE_30);
            foreach (var d in data)
            {
                db.AddDocument(d);
            }
            db.Save();
            db.Close();
        }

    }
}
