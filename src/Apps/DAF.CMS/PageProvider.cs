using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public class PageProvider : IPageProvider
    {
        private ITransactionManager trans;
        private IRepository<WebPage> repoPage;
        private IRepository<WebPageControl> repoControl;
        private IRepository<SubSite> repoSite;

        public PageProvider(ITransactionManager trans,  IRepository<SubSite> repoSite, IRepository<WebPage> repoPage, IRepository<WebPageControl> repoControl)
        {
            this.trans = trans;
            this.repoSite = repoSite;
            this.repoPage = repoPage;
            this.repoControl = repoControl;
        }

        public IEnumerable<WebPage> GetPages(string siteId, string parentId = null)
        {
            var query = repoPage.Query(o => o.SiteId == siteId);
            if (string.IsNullOrEmpty(parentId))
                query = query.Where(o => o.ParentPageId == null);
            else
                query = query.Where(o => o.ParentPageId == parentId);
            return query.ToArray();
        }

        public WebPage GetPage(string pageId)
        {
            var query = repoPage.Query(o => o.PageId == pageId).FirstOrDefault();
            return query;
        }

        public IEnumerable<WebPageControl> GetControls(string pageId)
        {
            var query = repoControl.Query(o => o.PageId == pageId);
            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public bool Save(ChangedData<WebPageControl> items)
        {
            return repoControl.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }

        public bool AddPage(WebPage page)
        {
            return repoPage.Insert(page);
        }

        public bool UpdatePage(WebPage page)
        {
            return repoPage.Update(page);
        }

        public bool DeletePage(string pageId)
        {
            return repoPage.DeleteBatch(o => o.PageId == pageId);
        }

        public bool SetAsHomePage(string pageId)
        {
            var p = repoPage.Query(o => o.PageId == pageId).FirstOrDefault();
            if (p != null)
            {
                var s = repoSite.Query(o => o.SiteId == p.SiteId).FirstOrDefault();
                if (s != null)
                {
                    s.HomePageId = p.PageId;
                    return repoSite.Update(s);
                }
            }
            return false;
        }
    }
}
