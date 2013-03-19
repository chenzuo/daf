using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core;
using DAF.Core.Command;

namespace DAF.Web.Commands
{
    public class SessionCommand : ICommand
    {
        public string Name
        {
            get { return "session"; }
        }

        public Dictionary<string, string> Args { get; set; }

        public object Run(object context)
        {
            if (context is HttpContextBase)
            {
                return ((HttpContextBase)context).Session[Args["name"]];
            }
            else if (context is HttpContext)
            {
                return ((HttpContext)context).Session[Args["name"]];
            }
            else
            {
                return HttpContext.Current.Session[Args["name"]];
            }
        }
    }
}
