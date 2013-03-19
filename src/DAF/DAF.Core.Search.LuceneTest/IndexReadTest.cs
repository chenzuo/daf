using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Moq;
using DAF.Core;
using DAF.Core.Search;
using DAF.Core.Search.Lucene;
using DAF.Core.FileSystem;
using Lucene.Net;
using Version = Lucene.Net.Util.Version;
using T = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAF.Core.Search.LuceneTest
{
    [TestClass]
    public class IndexReadTest : TestBase
    {
        private LuceneSearch db;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            db = new LuceneSearch(typeName, directory, mockDocBuilder.Object, mockPathBuilder.Object, mockFacetNameProvider.Object, Version.LUCENE_30);
        }
        
        [TestMethod]
        public void SearchEn()
        {
            db.NewQuery();
            Dictionary<string, object> scope = new Dictionary<string,object>();
            scope.Add("Locales", "en");
            scope.Add("BeginTime", new DateTime(2012, 1, 1));
            scope.Add("EndTime", new DateTime(2013, 1, 1));
            db.Scope(scope);
            db.Where("Title", "news");
            int totalCount = 0;
            IEnumerable<FacetGroup> fgs = null;
            var rs = db.Query(0, 0, out totalCount, out fgs);

            T.Assert.AreEqual(2, totalCount);
        }

        [TestMethod]
        public void SearchEnCn()
        {
            db.NewQuery();
            Dictionary<string, object> scope = new Dictionary<string, object>();
            scope.Add("Locales", "en,cn");
            scope.Add("BeginTime", new DateTime(2012, 1, 1));
            scope.Add("EndTime", new DateTime(2013, 1, 1));
            db.Scope(scope);
            db.Where("Title", "news");
            db.Where("Title", "饭店");
            int totalCount = 0;
            IEnumerable<FacetGroup> fgs = null;
            var rs = db.Query(0, 0, out totalCount, out fgs);

            T.Assert.AreEqual(4, totalCount);
        }

        [TestMethod]
        public void SearchFacet()
        {
            db.NewQuery();
            Dictionary<string, object> scope = new Dictionary<string, object>();
            scope.Add("Locales", "en,cn");
            scope.Add("BeginTime", new DateTime(2012, 1, 1));
            scope.Add("EndTime", new DateTime(2013, 1, 1));
            db.Scope(scope);
            db.Where("Title", "news");
            db.Where("Title", "饭店");
            db.Facet("Source", "Locale");
            int totalCount = 0;
            IEnumerable<FacetGroup> fgs = null;
            var rs = db.Query(0, 0, out totalCount, out fgs);

            T.Assert.AreEqual(4, totalCount);

            T.Assert.AreEqual(2, fgs.Count());

            var fg1 = fgs.Last();
            T.Assert.AreEqual("Blog.Source", fg1.FieldName);
            T.Assert.AreEqual(3, fg1.FacetItems.Count());
            T.Assert.AreEqual(4, fg1.TotalHits);
            T.Assert.AreEqual("net", fg1.FacetItems.First().GroupValue);
            T.Assert.AreEqual(2, fg1.FacetItems.First().Count);
        }
    }
}
