using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Web.Localization;

namespace DAF.CMS
{
    public class CmsModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<Core.Configurations.IConfigurationProvider, Configurations.ModuleConfigurationProvider>();
            builder.RegisterType<Core.Localization.ILocalizer, JsonLocalizer>(LiftTimeScope.Singleton, autoWire: true,
                getConstructorParameters: (ctx) =>
                    {
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("paths", "Localization, Modules/*/Localization");
                        return paras;
                    });

            builder.RegisterType<IObjectProvider<IEnumerable<DAF.Web.Menu.MenuGroup>>, ModuleMenuGroupProvider>(LiftTimeScope.Singleton, autoWire: true,
                getConstructorParameters: (ctx) =>
                    {
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("paths", "App_Data, Modules/*/App_Data");
                        paras.Add("fileName", "menu.js");
                        return paras;
                    });

            builder.RegisterType<IAppSettingProvider, AppSettingProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IBasicDataProvider, BasicDataProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<ICategoryProvider, CategoryProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IContentProvider, ContentProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IWebSiteProvider, WebSiteProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IPageTemplateProvider, PageTemplateProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IMenuProvider, MenuProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IUserGroupProvider, UserGroupProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<ITemplateTypeProvider, TemplateTypeProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IControlTypeProvider, ControlTypeProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<IPageProvider, PageProvider>(LiftTimeScope.Singleton);

            builder.RegisterType<IAppEventHandler, CmsAppEventHandler>();
            builder.RegisterType<DAF.SSO.Client.IDefaultSessionProvider, CmsDefaultSessionProvider>(LiftTimeScope.Singleton);
        }
    }
}
