using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Web;

namespace DAF.SSO.Site.Api
{
    public class SiteController : ApiController
    {
        public SiteController()
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<dynamic> Clients()
        {
            var clients = AuthHelper.Clients;
            var objs = clients.Select(o => new
            {
                ClientId = o.ClientId,
                ClientName = o.ClientName,
                BaseUrl = o.BaseUrl
            });
            return objs;
        }

        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<dynamic> Languages()
        {
            var langs = LocaleHelper.GetSupportLanguages();
            var objs = langs.Select(o => new
            {
                Code = o.Code,
                Name = o.DisplayName
            });
            return objs;
        }
    }
}