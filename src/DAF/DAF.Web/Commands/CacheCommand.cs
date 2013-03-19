using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAF.Core;
using DAF.Core.Command;
using DAF.Core.Caching;

namespace DAF.Web.Commands
{
    public class CacheCommand : ICommand
    {
        public string Name
        {
            get { return "cache"; }
        }

        public Dictionary<string, string> Args { get; set; }

        public object Run(object context)
        {
            var cacheMgr = IOC.Current.GetService<ICacheManager>();
            var cache = cacheMgr.CreateCacheProvider(CacheScope.Global);
            string cmd = Args["cmd"].ToLower();
            switch (cmd)
            {
                case "clear":
                    cache.Clear();
                    break;
                case "count":
                    return cache.Count;
            }
            return null;
        }
    }
}
