using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Configuration;
using DAF.Core;

namespace DAF.Web
{
    public class WebGlobal : System.Web.HttpApplication
    {
        protected static IContainer container;

        protected virtual void ConfigureTypesToScan()
        {
            Config.Current.IgnoreAssemblies("system", "autofac", "bltoolkit", "entityframework", "microsoft", "newtonsoft", "nservicebus", "log4net", "emitmapper")
                .With();
        }

        protected virtual ContainerBuilder BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            List<IAutoRegisterContainerWithType> autoRegisters = new List<IAutoRegisterContainerWithType>();
            Config.Current.TypesToScan.Where(t => typeof(IAutoRegisterContainerWithType).IsAssignableFrom(t))
                .ForEach(o =>
                {
                    if (o.GetConstructor(Type.EmptyTypes) != null)
                        autoRegisters.Add(Activator.CreateInstance(o) as IAutoRegisterContainerWithType);
                });

            Config.Current.TypesToScan.ForEach(t =>
            {
                autoRegisters.ForEach(o => o.Register(builder, t));
            });

            builder.RegisterModule(new ConfigurationSettingsReader());
            return builder;
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            ConfigureTypesToScan();
            var builder = BuildContainer();
            container = builder.Build();

            IOC.SetContainer(container);

            var appEventHandlers = container.ResolveOptional<IEnumerable<IAppEventHandler>>();
            if (appEventHandlers != null)
            {
                foreach (var aeh in appEventHandlers.OrderBy(o => o.ExecuteOrder))
                    aeh.OnApplicationStart(container, this.Context);
            }

            var startups = container.ResolveOptional<IEnumerable<IStartup>>();
            if (startups != null)
            {
                foreach (var aeh in startups.OrderBy(o => o.ExecuteOrder))
                    aeh.OnStarted();
            }
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            var appEventHandlers = container.ResolveOptional<IEnumerable<IAppEventHandler>>();
            if (appEventHandlers != null)
            {
                foreach (var aeh in appEventHandlers.OrderBy(o => o.ExecuteOrder))
                    aeh.OnApplicatoinExit(container, this.Context);
            }
        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            ILifetimeScope workUnitScope = container.BeginWorkUnitScope();
            this.Context.Items["workUnitScope"] = workUnitScope;
        }

        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
            ILifetimeScope workUnitScope = this.Context.Items["workUnitScope"] as ILifetimeScope;
            if (workUnitScope != null)
                workUnitScope.Dispose();
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        protected virtual void Session_End(object sender, EventArgs e)
        {
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom == "session")
            {
                return context.Request["ASP.NET_SessionId"];
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
}
