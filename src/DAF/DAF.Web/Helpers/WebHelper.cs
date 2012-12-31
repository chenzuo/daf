using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.WebPages.Html;
using System.ComponentModel;
using DAF.Core;
using DAF.Core.Localization;
using DAF.Core.Data;

namespace DAF.Web
{
    public class WebHelper
    {
        public static void Refresh(IServerResponse msg = null, HttpResponseBase response = null)
        {
            if (response == null)
                response = new HttpResponseWrapper(HttpContext.Current.Response);

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'> ");
            sb.Append("if(window.postMessage) { if(window.opener != null) { window.opener.postMessage('{\"event\":\"reload\"");
            if (msg != null)
                sb.AppendFormat(", \"msg\": {{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}", (int)msg.Status, msg.Message);
            sb.Append("}', '*'); }");
            sb.Append("else if(window.parent != null) { window.parent.postMessage('{\"event\":\"reload\"");
            if (msg != null)
                sb.AppendFormat(", \"msg\": {{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}", (int)msg.Status, msg.Message);
            sb.Append("}', '*'); }");
            sb.Append("else { window.top.postMessage('{\"event\":\"reload\"");
            if (msg != null)
                sb.AppendFormat(", \"msg\": {{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}", (int)msg.Status, msg.Message);
            sb.Append("}', '*'); }");
            sb.Append("} else { if(window.opener != null) { ");
            if (msg != null)
                sb.AppendFormat("window.localStorage.setItem(\"alert\", '{{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}');", (int)msg.Status, msg.Message);
            sb.Append("window.opener.location.reload(); }");
            sb.Append("else if(window.parent != null) { ");
            if (msg != null)
                sb.AppendFormat("window.localStorage.setItem(\"alert\", '{{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}');", (int)msg.Status, msg.Message);
            sb.Append("window.parent.location.reload(); }");
            sb.Append("else { ");
            if (msg != null)
                sb.AppendFormat("window.localStorage.setItem(\"alert\", '{{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}');", (int)msg.Status, msg.Message);
            sb.Append("window.top.location.reload(); }");
            sb.Append("} </script>");

            response.Write(sb.ToString());
            response.End();
        }

        public static void AllowCros(HttpContext context = null)
        {
            if (context == null)
                context = HttpContext.Current;

            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Cache-Control", "private, no-cache");
            context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type,Content-Range,Content-Disposition,Content-Description,X-File-Name,X-File-Type,X-File-Size");
            context.Response.AddHeader("Access-Control-Allow-Origin", context.Request.UrlReferrer == null ? "*" : context.Request.UrlReferrer.BaseUrl());
        }

        public static void AllowCros(HttpContextBase context = null)
        {
            if (context == null)
                context = new HttpContextWrapper(HttpContext.Current);

            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Cache-Control", "private, no-cache");
            context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type,Content-Range,Content-Disposition,Content-Description,X-File-Name,X-File-Type,X-File-Size");
            context.Response.AddHeader("Access-Control-Allow-Origin", context.Request.UrlReferrer == null ? "*" : context.Request.UrlReferrer.BaseUrl());
        }
    }
}
