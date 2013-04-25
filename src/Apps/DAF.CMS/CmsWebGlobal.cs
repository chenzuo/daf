using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Web.Api;

namespace DAF.CMS
{
    public class CmsWebGlobal : WebApiGlobal
    {
        protected override void BuildeIOC(IIocBuilder builder)
        {
            base.BuildeIOC(builder);
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Modules"));
            var files = dir.GetFiles("ioc.config", true);
            if (files != null && files.Count() > 0)
            {
                foreach (var f in files)
                {
                    builder.RegisterConfig(f.FullName);
                }
            }
        }

        protected override IIocBuilder CreateIocBuilder()
        {
            return new DAF.Core.IOC.AutofacForApi.AutofacBuilderForApi();
        }
    }
}
