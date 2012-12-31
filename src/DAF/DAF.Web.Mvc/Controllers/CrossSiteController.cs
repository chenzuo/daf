using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Serialization;
using DAF.Web;
using DAF.Web.Security;

namespace DAF.Web.Mvc.Controllers
{
    public class CrossSiteController : System.Web.Mvc.Controller
    {
        private IPasswordEncryptionProvider encrypt;
        private IJsonSerializer jsonSerializer;

        public CrossSiteController(IPasswordEncryptionProvider encrypt, IJsonSerializer jsonSerializer)
        {
            this.encrypt = encrypt;
            this.jsonSerializer = jsonSerializer;
        }

        [HttpGet]
        public ActionResult Get(string url)
        {
            if (url.StartsWith("/") || url.IndexOf("://") <= 0)
                return RedirectPermanent(url);

            url = url.AppendQueryString(string.Format("client={0}&sid={1}", AuthHelper.CurrentClient.ClientId, AuthHelper.CurrentSession.SessionId));
            Uri referer = Request.UrlReferrer ?? Request.Url;
            string html = HttpHelper.Get(url, null, Request.UserAgent, referer, LocaleHelper.Localizer.GetCurrentCultureInfo(), null);
            return Content(html, "text/html");
        }

        [HttpPost]
        public ActionResult Post(string url)
        {
            if (url.StartsWith("/") || url.IndexOf("://") <= 0)
                return RedirectPermanent(url);
            StreamReader sr = new StreamReader(this.Request.InputStream);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            var data = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            url = url.AppendQueryString(string.Format("client={0}&sid={1}", AuthHelper.CurrentClient.ClientId, AuthHelper.CurrentSession.SessionId));
            Uri referer = Request.UrlReferrer ?? Request.Url;
            string html = HttpHelper.Post(url, data, null, Request.UserAgent, referer, LocaleHelper.Localizer.GetCurrentCultureInfo(), null);
            return Content(html, "text/html");
        }
    }
}
