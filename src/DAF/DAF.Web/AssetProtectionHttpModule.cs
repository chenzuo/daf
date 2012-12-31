using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Autofac;
using DAF.Core;
using DAF.Web.Security;

namespace DAF.Web
{
    public class AssetProtectionHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            bool needCheck = false;
            var checkers = IOC.Current.ResolveOptional<IEnumerable<IProtectedAssetChecker>>();
            if (checkers != null && checkers.Count() > 0)
            {
                foreach (var c in checkers)
                {
                    if (c.IsProtectedAsset(context.Request.Url))
                    {
                        needCheck = true;
                        break;
                    }
                }
            }
            if (needCheck)
            {
                var filters = IOC.Current.ResolveOptional<IEnumerable<IAssetProtectionFilter>>();
                if (filters != null && filters.Count() > 0)
                {
                    foreach (var f in filters)
                    {
                        if (f.AllowAccess(context.Context) == false)
                        {
                            var ext = VirtualPathUtility.GetExtension(context.Request.FilePath);
                            string path = string.Format("~/Content/errors/403" + ext);
                            context.Server.TransferRequest(path);
                        }
                    }
                }
            }
        }
    }
}
