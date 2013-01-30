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
using DAF.Core.Data;
using DAF.CMS;
using DAF.CMS.Models;

namespace DAF.CMS.Site.Controllers
{
    public class ContentController : ApiController
    {
        private IWebSiteProvider siteProvider;
        private IContentProvider provider;

        public ContentController(IWebSiteProvider siteProvider, IContentProvider provider)
        {
            this.siteProvider = siteProvider;
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<Content> Data(string siteId, string cate = null, ContentType? contentType = null)
        {
            ContentType[] contentTypes = null;
            if (contentType != null)
                contentTypes = new ContentType[] { contentType.Value };
            var query = provider.GetContents(siteId, cate, null, contentTypes);

            return query.ToArray();
        }

        [HttpGet]
        public Content RelatedContent(string siteId, ContentType? contentType, string title, string content, string contentId = null)
        {
            Content obj = null;
            if (string.IsNullOrEmpty(contentId))
            {
                obj = provider.Get(siteId, contentType.Value, title, content, false, false);
            }
            else
            {
                obj = provider.Get(siteId, contentId);
            }

            return obj;
        }

        [HttpGet]
        public Content Detail(string siteId, string id = null, ContentType contentType = ContentType.Html)
        {
            Content obj = null;
            if (string.IsNullOrEmpty(id))
            {
                obj = provider.New(siteId, contentType);
            }
            else
            {
                obj = provider.Get(siteId, id);
            }

            return obj;
        }

        [HttpPost]
        public ServerResponse Save([FromBody]Content obj)
        {
            return obj.Save(o => provider.Save(o));
        }

        [HttpPost]
        public ServerResponse Delete(string siteId, bool deleteRelated, [FromBody]string[] deleteIds)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                foreach (var k in deleteIds)
                {
                    provider.Delete(siteId, k, deleteRelated);
                }
                response.Status = ResponseStatus.Success;
                response.Message = LocaleHelper.Localizer.Get("DeleteSuccessfully");
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}