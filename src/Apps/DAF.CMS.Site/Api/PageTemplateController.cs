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
    public class PageTemplateController : ApiController
    {
        private ITemplateTypeProvider templateProvider;
        private IControlTypeProvider controlProvider;
        private IPageTemplateProvider pageProvider;

        public PageTemplateController(ITemplateTypeProvider templateProvider, IControlTypeProvider controlProvider, IPageTemplateProvider pageProvider)
        {
            this.templateProvider = templateProvider;
            this.controlProvider = controlProvider;
            this.pageProvider = pageProvider;
        }

        [HttpGet]
        public IEnumerable<TemplateType> TemplateTypes()
        {
            return templateProvider.LoadTemplateTypes();
        }

        [HttpGet]
        public IEnumerable<ControlType> ControlTypes()
        {
            return controlProvider.LoadControlTypes();
        }

        [HttpGet]
        public IEnumerable<PageTemplate> Templates(string siteId, string parentName = null)
        {
            return pageProvider.GetTemplates(siteId, parentName);
        }

        [HttpGet]
        public IEnumerable<PageTemplateControl> Data(string siteId, string template = null)
        {
            return pageProvider.GetControls(siteId, template);
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<PageTemplateControl> items)
        {
            return items.Save(o => pageProvider.Save(items));
        }

        [HttpPost]
        public ServerResponse AddTemplate(PageTemplate template)
        {
            return template.Save(o => pageProvider.AddTemplate(o));
        }

        [HttpPost]
        public ServerResponse EditTemplate(PageTemplate template)
        {
            return template.Save(o => pageProvider.UpdateTemplate(o));
        }

        [HttpGet]
        public ServerResponse DeleteTemplate(string siteId, string template)
        {
            return siteId.Save(o => pageProvider.DeleteTemplate(siteId, template),
                LocaleHelper.Localizer.Get("DeleteSuccessfully"), LocaleHelper.Localizer.Get("DeleteFailure"));
        }
    }
}