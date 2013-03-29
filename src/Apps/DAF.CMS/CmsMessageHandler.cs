using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Web.Handlers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebPages;
using System.IO;
using System.Web.Http.Hosting;

namespace DAF.CMS
{
    public class CmsMessageHandler : WebRequestHandler
    {
        const int BufferSize = 32 * 1024;
        private string subdir = null;

        public CmsMessageHandler(string subdir = null)
        {
            this.subdir = subdir;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder("/");
            if (!string.IsNullOrEmpty(subdir))
            {
                sb.AppendFormat("{0}/", subdir);
            }
            sb.Append("Default.cshtml");

            var routeData = request.GetRouteData();
            if (routeData != null && routeData.Values.Count > 0)
            {
                sb.Append("?");
                foreach (var r in routeData.Values)
                {
                    sb.AppendFormat("{0}={1}&", r.Key, r.Value);
                }
                sb.Remove(sb.Length - 1, 1);
            }
            var url = request.RequestUri.AbsoluteUri;
            url = url.Replace(request.RequestUri.AbsolutePath, sb.ToString());

            request.RequestUri = new Uri(url);
            if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Options)
                request.Content = null;

            return base.SendAsync(request, cancellationToken);
        }
    }
}
