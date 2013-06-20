using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Core.Generators;
using DAF.Web;
using DAF.Web.Menu;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS.Site.Controllers
{
    public class PageController : ApiController
    {
        private ITemplateTypeProvider templateProvider;
        private IWidgetTypeProvider widgetProvider;
        private IPageTemplateProvider pageTemplateProvider;
        private IPageProvider pageProvider;

        public PageController(ITemplateTypeProvider templateProvider, IWidgetTypeProvider widgetProvider, IPageTemplateProvider pageTemplateProvider, IPageProvider pageProvider)
        {
            this.templateProvider = templateProvider;
            this.widgetProvider = widgetProvider;
            this.pageTemplateProvider = pageTemplateProvider;
            this.pageProvider = pageProvider;
        }

        [HttpGet]
        public IEnumerable<TemplateType> TemplateTypes()
        {
            return templateProvider.LoadTemplateTypes();
        }

        [HttpGet]
        public IEnumerable<WidgetType> WidgetTypes()
        {
            return widgetProvider.LoadWidgetTypes();
        }

        [HttpGet]
        public IEnumerable<TemplateType> Templates(string siteId)
        {
            var templateTypes = templateProvider.LoadTemplateTypes();
            var query = pageTemplateProvider.GetTemplates(siteId);

            var ts = HierarchyHelper.Build<PageTemplate, TemplateType>(query.Where(o => o.ParentTemplateName == null),
                o => new TemplateType
            {
                Name = o.TemplateName,
                Path = o.TemplatePath,
                ParentName = o.ParentTemplateName,
                Sections = templateTypes.First(t => t.Path.ToLower() == o.TemplatePath.ToLower()).Sections
            }, o => query.Where(q => q.ParentTemplateName == o.TemplateName), (p, c) => { p.Children.Add(c); });

            return ts;
        }

        [HttpGet]
        public IEnumerable<WebPage> Pages(string siteId)
        {
            return pageProvider.GetPages(siteId);
        }

        [HttpGet]
        public IEnumerable<WebPageWidget> Data(string pageId)
        {
            return pageProvider.GetControls(pageId);
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<WebPageWidget> items)
        {
            return items.Save(o => pageProvider.Save(items));
        }

        [HttpPost]
        public ServerResponse AddPage(WebPage page)
        {
            return page.Save(o => pageProvider.AddPage(o));
        }

        [HttpPost]
        public ServerResponse EditPage(WebPage page)
        {
            return page.Save(o => pageProvider.UpdatePage(o));
        }

        [HttpGet]
        public ServerResponse DeletePage(string pageId)
        {
            return pageId.Save(o => pageProvider.DeletePage(pageId),
                LocaleHelper.Localizer.Get("DeleteSuccessfully"), LocaleHelper.Localizer.Get("DeleteFailure"));
        }

        [HttpGet]
        public ServerResponse SetAsHomePage(string pageId)
        {
            var response = pageId.Save(o => pageProvider.SetAsHomePage(pageId));
            if (response.Status == ResponseStatus.Success)
            {
                CmsHelper.CurrentSite.HomePageId = pageId;
            }
            return response;
        }
    }
}