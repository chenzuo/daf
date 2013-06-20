using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Collections;
using DAF.Web;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public class PageTemplateProvider : IPageTemplateProvider
    {
        private ITransactionManager trans;
        private IRepository<PageTemplate> repoTemplate;
        private IRepository<PageTemplateWidget> repoControl;

        public PageTemplateProvider(ITransactionManager trans, IRepository<PageTemplate> repoTemplate, IRepository<PageTemplateWidget> repoControl)
        {
            this.trans = trans;
            this.repoTemplate = repoTemplate;
            this.repoControl = repoControl;
        }

        public IEnumerable<PageTemplate> GetTemplates(string siteId)
        {
            var query = repoTemplate.Query(o => o.SiteId == siteId);
            return query.ToArray();
        }

        public IEnumerable<PageTemplate> GetTemplates(string siteId, string parentName)
        {
            var query = repoTemplate.Query(o => o.SiteId == siteId);
            if (string.IsNullOrEmpty(parentName))
                query = query.Where(o => o.ParentTemplateName == null);
            else
                query = query.Where(o => o.ParentTemplateName == parentName);
            return query.ToArray();
        }

        public PageTemplate GetTemplate(string siteId, string template)
        {
            var query = repoTemplate.Query(o => o.SiteId == siteId && o.TemplateName == template).FirstOrDefault();
            return query;

        }

        public IEnumerable<PageTemplateWidget> GetControls(string siteId, string template = null)
        {
            var query = repoControl.Query(o => o.SiteId == siteId);
            if (!string.IsNullOrEmpty(template))
                query = query.Where(o => o.TemplateName == template);
            query = query.OrderBy(o => o.ShowOrder);
            return query.ToArray();
        }

        public bool Save(ChangedData<PageTemplateWidget> items)
        {
            return repoControl.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems);
        }

        public bool AddTemplate(PageTemplate template)
        {
            return repoTemplate.Insert(template);
        }

        public bool UpdateTemplate(PageTemplate template)
        {
            return repoTemplate.Update(template);
        }

        public bool DeleteTemplate(string siteId, string template)
        {
            return repoTemplate.DeleteBatch(o => o.SiteId == siteId && o.TemplateName == template);
        }
    }
}
