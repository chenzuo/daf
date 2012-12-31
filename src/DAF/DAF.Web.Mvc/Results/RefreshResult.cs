using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAF.Core;

namespace DAF.Web.Mvc.Results
{
    public class RefreshResult : ContentResult
    {
        private IServerResponse result;

        public RefreshResult(IServerResponse result = null)
        {
            this.result = result;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type='text/javascript'> ");
            sb.Append("if(window.postMessage) { if(window.opener != null) { window.opener.postMessage('{\"event\":\"reload\"");
            if (result != null)
                sb.AppendFormat(", \"result\": {{ \"Status\": \"{0}\", \"Message\": \"{1}\"", (int)result.Status, result.Message);
            sb.Append("}', '*'); }");
            sb.Append("else if(window.parent != null) { window.parent.postMessage('{\"event\":\"reload\"");
            if (result != null)
                sb.AppendFormat(", \"result\": {{ \"Status\": \"{0}\", \"Message\": \"{1}\"", (int)result.Status, result.Message);
            sb.Append("}', '*'); }");
            sb.Append("else { window.top.postMessage('{\"event\":\"reload\"");
            if (result != null)
                sb.AppendFormat(", \"result\": {{ \"Status\": \"{0}\", \"Message\": \"{1}\"", (int)result.Status, result.Message);
            sb.Append("}', '*'); }");
            sb.Append("} else { if(window.opener != null) { ");
            if (result != null)
                sb.AppendFormat("window.localStorage.setItem(\"alert\", '{{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}');", (int)result.Status, result.Message);
            sb.Append("window.opener.location.reload(); }");
            sb.Append("else if(window.parent != null) { ");
            if (result != null)
                sb.AppendFormat("window.localStorage.setItem(\"alert\", '{{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}');", (int)result.Status, result.Message);
            sb.Append("window.parent.location.reload(); }");
            sb.Append("else { ");
            if (result != null)
                sb.AppendFormat("window.localStorage.setItem(\"alert\", '{{ \"Status\": \"{0}\", \"Message\": \"{1}\" }}');", (int)result.Status, result.Message);
            sb.Append("window.top.location.reload(); }");
            sb.Append("} </script>");

            this.Content = sb.ToString();

            base.ExecuteResult(context);
        }
    }
}
