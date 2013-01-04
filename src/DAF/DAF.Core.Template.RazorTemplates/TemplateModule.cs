using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using DAF.Core;

namespace DAF.Core.Template.RazorTemplates
{
    public class TemplateModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Core.Template.DefaultTemplateEngine>().As<Core.Template.ITemplateEngine>().Named<Core.Template.ITemplateEngine>("DefaultTemplateEngine");
            builder.RegisterType<RazorContentTemplateGenerator>().As<Core.Template.ITemplateGenerator>().Named<Core.Template.ITemplateGenerator>("RazorContentTemplateGenerator");
            builder.RegisterType<Core.Template.FileTemplateProvider>().OnPreparing(pe =>
            {
                NamedParameter np = new NamedParameter("root", "~/Templates");
                pe.Parameters = new Parameter[] { np };
            }).As<Core.Template.ITemplateProvider>().Named<Core.Template.ITemplateProvider>("FileTemplateProvider");
        }
    }
}
