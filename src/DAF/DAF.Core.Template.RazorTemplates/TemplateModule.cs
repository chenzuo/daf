using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;

namespace DAF.Core.Template.RazorTemplates
{
    public class TemplateModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<Core.Template.ITemplateEngine, Core.Template.DefaultTemplateEngine>(name: "DefaultTemplateEngine");
            builder.RegisterType<Core.Template.ITemplateGenerator, RazorContentTemplateGenerator>(name: "RazorContentTemplateGenerator");
            builder.RegisterType<Core.Template.ITemplateProvider, Core.Template.FileTemplateProvider>(name: "FileTemplateProvider",
                getConstructorParameters: (ctx) =>
                {
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("root", "~/Templates");
                    return paras;
                });
        }
    }
}
