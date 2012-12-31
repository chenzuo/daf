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
using DAF.CMS.Site.Models;
using DAF.CMS.Site.Logics;

namespace DAF.CMS.Site.Controllers
{
    public class ContentController : ApiController
    {
        protected IIdGenerator idGenerator;
        protected IContentProvider provider;

        public ContentController(IIdGenerator idGenerator, IContentProvider provider)
        {
            this.idGenerator = idGenerator;
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<Content> Data(string language = null, string category = null, ContentType? contentType = null)
        {
            if (string.IsNullOrEmpty(language))
                language = LocaleHelper.Localizer.GetCurrentCultureInfo();
            var query = provider.GetContents(AuthHelper.CurrentClient.ClientId, language, category, contentType);

            return query.ToArray();
        }

        [HttpGet]
        public Content RelatedContent(string site, string language, ContentType? contentType, string title, string content, string contentId = null)
        {
            Content obj = null;
            if (string.IsNullOrEmpty(contentId))
            {
                obj = provider.Get(site, contentType.Value, language, title, content, false, false);
            }
            else
            {
                obj = provider.Get(site, contentId, language);
            }

            return obj;
        }

        [HttpGet]
        public Content Detail(string id = null, string language = null, ContentType contentType = ContentType.Html)
        {
            if (string.IsNullOrEmpty(language))
                language = LocaleHelper.Localizer.GetCurrentCultureInfo();
            Content obj = null;
            if (string.IsNullOrEmpty(id))
            {
                obj = provider.New(AuthHelper.CurrentClient.ClientId, contentType, language);
            }
            else
            {
                obj = provider.Get(AuthHelper.CurrentClient.ClientId, id, language);
            }

            return obj;
        }

        [HttpPost]
        public ServerResponse Save([FromBody]Content obj)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (provider.Save(obj))
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = LocaleHelper.Localizer.Get("SaveSuccessfully");
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = LocaleHelper.Localizer.Get("SaveFailure");
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public ServerResponse Delete(string language, bool deleteRelated, [FromBody]string[] deleteIds)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                foreach (var k in deleteIds)
                {
                        provider.Delete(AuthHelper.CurrentClient.ClientId, k, language, deleteRelated);
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