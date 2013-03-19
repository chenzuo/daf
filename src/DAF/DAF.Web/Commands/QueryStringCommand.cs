using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core;
using DAF.Core.Command;

namespace DAF.Web.Commands
{
    public class QueryStringCommand : ICommand
    {
        public string Name
        {
            get { return "qs"; }
        }

        public Dictionary<string, string> Args { get; set; }

        public object Run(object context)
        {
            if (context is HttpContextBase)
            {
                return ((HttpContextBase)context).Request.QueryString[Args["name"]];
            }
            else if (context is HttpContext)
            {
                return ((HttpContext)context).Request.QueryString[Args["name"]];
            }
            else
            {
                return HttpContext.Current.Request.QueryString[Args["name"]];
            }
        }
    }
}
