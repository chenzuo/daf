using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Command;

namespace DAF.Web.Commands
{
    public class UrlCommand : ICommand
    {
        public string Name
        {
            get { return "url"; }
        }

        public Dictionary<string, string> Args { get; set; }

        public object Run(object context)
        {
            string with = Args["with"].ToLower();
            switch (with)
            {
                case "sso_server":
                    return AuthHelper.CurrentServer.BaseUrl;
                case "sso_client":
                    if (Args.ContainsKey("name"))
                    {
                        var client = AuthHelper.Clients.FirstOrDefault(o => o.ClientName == Args["name"]);
                        if (client == null)
                            return null;
                        return client.BaseUrl;
                    }
                    return AuthHelper.CurrentClient.BaseUrl;
            }
            return null;
        }
    }
}
