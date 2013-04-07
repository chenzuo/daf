using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using DAF.Core;
using DAF.Web.Localization;

namespace DAF.CMS
{
    public class CmsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Configurations.ModuleConfigurationProvider>().As<Core.Configurations.IConfigurationProvider>();
            builder.RegisterModule(new AutoWireModule<Core.Localization.ILocalizer>(
             o => o.RegisterType<JsonLocalizer>().OnPreparing(pe =>
             {
                 NamedParameter np = new NamedParameter("paths", "Localization, Modules/*/Localization");
                 pe.Parameters = new Parameter[] { np };
             }).As<Core.Localization.ILocalizer>().SingleInstance()));

            builder.RegisterType<ModuleMenuGroupProvider>().OnPreparing(pe =>
            {
                NamedParameter p1 = new NamedParameter("paths", "App_Data, Modules/*/App_Data");
                NamedParameter p2 = new NamedParameter("fileName", "menu.js");
                pe.Parameters = new Parameter[] { p1, p2 };
            }).As<IObjectProvider<IEnumerable<DAF.Web.Menu.MenuGroup>>>().SingleInstance();
        
            builder.RegisterType<AppSettingProvider>().As<IAppSettingProvider>().SingleInstance();
            builder.RegisterType<BasicDataProvider>().As<IBasicDataProvider>().SingleInstance();
            builder.RegisterType<CategoryProvider>().As<ICategoryProvider>().SingleInstance();
            builder.RegisterType<ContentProvider>().As<IContentProvider>().SingleInstance();
            builder.RegisterType<WebSiteProvider>().As<IWebSiteProvider>().SingleInstance();
            builder.RegisterType<PageTemplateProvider>().As<IPageTemplateProvider>().SingleInstance();
            builder.RegisterType<MenuProvider>().As<IMenuProvider>().SingleInstance();
            builder.RegisterType<UserGroupProvider>().As<IUserGroupProvider>().SingleInstance();
            builder.RegisterType<TemplateTypeProvider>().As<ITemplateTypeProvider>().SingleInstance();
            builder.RegisterType<ControlTypeProvider>().As<IControlTypeProvider>().SingleInstance();
            builder.RegisterType<PageProvider>().As<IPageProvider>().SingleInstance();

            builder.RegisterType<CmsAppEventHandler>().As<DAF.Core.IAppEventHandler>();
            builder.RegisterType<CmsDefaultSessionProvider>().As<DAF.SSO.Client.IDefaultSessionProvider>().SingleInstance();
        }
    }
}
