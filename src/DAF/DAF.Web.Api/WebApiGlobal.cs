using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using Autofac;
using Autofac.Integration.WebApi;
using DAF.Core;
using DAF.Core.FileSystem;
using DAF.Web.Api.Filters;

namespace DAF.Web.Api
{
    public class WebApiGlobal : WebGlobal
    {
        protected override ContainerBuilder BuildContainer()
        {
            ContainerBuilder builder = base.BuildContainer();

            Config.Current.AssembiesToScan.ForEach(asm =>
                {
                    builder.RegisterApiControllers(asm);
                });

            return builder;
        }

        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            GlobalConfiguration.Configuration.Services.RemoveAll(typeof(ModelValidatorProvider), v => v is InvalidModelValidatorProvider);
            GlobalConfiguration.Configuration.Filters.Add(new ModelValidationFilterAttribute());
            //GlobalConfiguration.Configuration.Filters.Add(new DAF.Web.Api.TransactionFilter());
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }
    }
}
