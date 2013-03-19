using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DAF.Core;
using DAF.Core.Search;
using DAF.Core.Search.Lucene;
using Lucene.Net;

namespace DAF.Core.Search.LuceneTest
{
    [TestClass]
    public class TestBase
    {
        protected string typeName = "Blog";
        protected string directory = "e:\\lucene";
        protected Mock<IDocumentBuilder> mockDocBuilder;
        protected Mock<IIndexPathBuilder> mockPathBuilder;
        protected Mock<IFacetFieldNameProvider> mockFacetNameProvider;

        [TestInitialize]
        public virtual void Init()
        {
            mockDocBuilder = new Mock<IDocumentBuilder>();
            mockDocBuilder.Setup(o => o.GetFields(It.IsAny<string>()))
                .Returns(new Field[]
                {
                    new Field() { FieldName = "BlogId", FieldType = FieldType.String, IsKey = true, IndexMode = FieldIndexMode.NO }
                    , new Field() { FieldName = "Source", FieldType = FieldType.String }
                    , new Field() { FieldName = "Title", FieldType = FieldType.String, Boost = 1.5f }
                    , new Field() { FieldName = "Content", FieldType = FieldType.String }
                    , new Field() { FieldName = "Locale", FieldType = FieldType.String }
                    , new Field() { FieldName = "CreateTime", FieldType = FieldType.DateTime, IndexMode = FieldIndexMode.NO }
                });

            mockDocBuilder.Setup(o => o.GetAnalyzer(It.IsAny<string>()))
                .Returns(string.Empty);

            mockDocBuilder.Setup(o => o.GetKey(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns<string, IDictionary<string, object>>((f, dic) => new KeyValuePair<string, string>("BlogId", dic["BlogId"].ToString()));


            mockPathBuilder = new Mock<IIndexPathBuilder>();
            mockPathBuilder.Setup(o => o.GetAllPath(It.IsAny<string>()))
                .Returns<string>(directory =>
                {
                    DirectoryInfo dir = new DirectoryInfo(directory);
                    var paths = dir.GetDirectories("*", SearchOption.AllDirectories);
                    return paths.Where(o =>
                    {
                        var dirs = o.GetDirectories();
                        return dirs == null || dirs.Length <= 0;
                    }).Select(o => o.FullName).ToArray();
                });

            mockPathBuilder.Setup(o => o.BuildIndexPath(It.IsAny<IDictionary<string, object>>()))
                .Returns<IDictionary<string, object>>(dic =>
                    {
                        string locale = dic["Locale"].ToString();
                        DateTime createTime = (DateTime)dic["CreateTime"];
                        DateTime beginDate = new DateTime(createTime.Year, createTime.Month, 1);
                        DateTime endDate = beginDate.AddMonths(1).AddDays(-1.0d);
                        return string.Format("{1}{0}{2:yyyyMMdd}_{3:yyyyMMdd}", Path.DirectorySeparatorChar, locale, beginDate, endDate);
                    });

            mockPathBuilder.Setup(o => o.BuildSearchPaths(It.IsAny<IDictionary<string, object>>()))
                .Returns<IDictionary<string, object>>(dic =>
                    {
                        string dicLocales = dic["Locales"].ToString();
                        DateTime beginTime = (DateTime)dic["BeginTime"];
                        DateTime endTime = (DateTime)dic["EndTime"];
                        List<string> paths = new List<string>();
                        string[] locales = dicLocales.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var locale in locales)
                        {
                            DateTime beginDate = new DateTime(beginTime.Year, beginTime.Month, 1);
                            while (beginDate < endTime)
                            {
                                DateTime endDate = beginDate.AddMonths(1).AddDays(-1.0d);
                                string p = string.Format("{1}{0}{2:yyyyMMdd}_{3:yyyyMMdd}", Path.DirectorySeparatorChar, locale, beginDate, endDate);
                                paths.Add(p);

                                beginDate = beginDate.AddMonths(1);
                            }
                        }
                        return paths.ToArray();
                    });

            mockFacetNameProvider = new Mock<IFacetFieldNameProvider>();
            mockFacetNameProvider.Setup(o => o.GetMapName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((t, f) =>
                    {
                        return string.Format("{0}.{1}", t, f);
                    });
        }

        [TestCleanup]
        public virtual void Finish()
        {
        }

        protected IEnumerable<IDictionary<string, object>> PrepareData()
        {
            List<IDictionary<string, object>> data = new List<IDictionary<string, object>>();

            var d1 = new Dictionary<string, object>();
            d1.Add("BlogId", "blog1");
            d1.Add("Source", "net");
            d1.Add("Title", "news is coming");
            d1.Add("Content", "In 1929, News shown on TV were very late.");
            d1.Add("Locale", "en");
            d1.Add("CreateTime", new DateTime(2012, 1, 1));
            data.Add(d1);

            var d2 = new Dictionary<string, object>();
            d2.Add("BlogId", "blog2");
            d2.Add("Source", "tv");
            d2.Add("Title", "news gone");
            d2.Add("Content", "News were gone when MV comes.");
            d2.Add("Locale", "en");
            d2.Add("CreateTime", new DateTime(2012, 2, 2));
            data.Add(d2);

            var d3 = new Dictionary<string, object>();
            d3.Add("BlogId", "blog3");
            d3.Add("Source", "net");
            d3.Add("Title", "有家饭店着火了");
            d3.Add("Content", "昨天晚上12点，上海某饭店，因不明原因起火，事故原因，目前还在调查中。");
            d3.Add("Locale", "cn");
            d3.Add("CreateTime", new DateTime(2012, 1, 2));
            data.Add(d3);

            var d4 = new Dictionary<string, object>();
            d4.Add("BlogId", "blog4");
            d4.Add("Source", "mv");
            d4.Add("Title", "饭店升级");
            d4.Add("Content", "国家饭店总局要求各省、地市认真做好饭店升级工作，避免起火。");
            d4.Add("Locale", "cn");
            d4.Add("CreateTime", new DateTime(2012, 3, 1));
            data.Add(d4);

            return data;
        }
    }
}
