using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public interface IPageTemplateProvider
    {
        IEnumerable<PageTemplate> GetTemplates(string siteId);
        IEnumerable<PageTemplate> GetTemplates(string siteId, string parentName);
        PageTemplate GetTemplate(string siteId, string template);
        IEnumerable<PageTemplateWidget> GetControls(string siteId, string template = null);
        bool Save(ChangedData<PageTemplateWidget> items);

        bool AddTemplate(PageTemplate template);
        bool UpdateTemplate(PageTemplate template);
        bool DeleteTemplate(string siteId, string template);
    }
}
