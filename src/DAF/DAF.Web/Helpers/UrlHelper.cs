using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using System.Globalization;
using DAF.Core.IOC;
using DAF.Core;
using DAF.Core.Localization;
using DAF.Core.Logging;
using DAF.Core.Security;
using DAF.Core.FileSystem;
using DAF.Core.Serialization;
using DAF.Web.Security;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.Web.Menu;

namespace DAF.Web
{
    public class UrlHelper
    {
        public static string SSOUrl(string url)
        {
            if (AuthHelper.CurrentServer != null)
            {
                return AuthHelper.CurrentServer.BaseUrl.UriCombine(url);
            }
            return "/".UriCombine(url);
        }

        public static string ClientUrl(string url, string clientId = null)
        {
            IObjectProvider<SSOClient> clientProvider = null;
            if (string.IsNullOrEmpty(clientId))
            {
                if (AuthHelper.CurrentSession != null && !string.IsNullOrEmpty(AuthHelper.CurrentSession.ClientBaseUrl))
                {
                    return AuthHelper.CurrentSession.ClientBaseUrl.UriCombine(url);
                }
                clientProvider = IocInstance.Container.ResolveOptional<IObjectProvider<SSOClient>>();
                if (clientProvider != null)
                {
                    return clientProvider.GetObject().BaseUrl.UriCombine(url);
                }
            }
            else
            {
                var clientsProvider = IocInstance.Container.ResolveOptional<IObjectProvider<SSOClient[]>>();
                if (clientsProvider != null)
                {
                    var clients = clientsProvider.GetObject();
                    if (clients != null && clients.Length > 0)
                    {
                        var c = clients.FirstOrDefault(o => o.ClientId == clientId);
                        if (c != null)
                            return c.BaseUrl.UriCombine(url);
                    }
                }
            }
            return "/".UriCombine(url);
        }
    }
}
