using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using Autofac;
using Autofac.Configuration;
using DAF.Core;
using DAF.Web.Api;

namespace DAF.CMS
{
    public class CmsWebGlobal : WebApiGlobal
    {
        protected override void BuildContainer(ContainerBuilder builder)
        {
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Modules"));
            var files = dir.GetFiles("autofac.config", true);
            if (files != null && files.Count() > 0)
            {
                foreach (var f in files)
                {
                    builder.RegisterModule(new ConfigurationSettingsReader("autofac", f.FullName));
                }
            }
        }
    }
}
