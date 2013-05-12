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
            builder.RegisterType<Core.Localization.ILocalizer, JsonLocalizer>(LifeTimeScope.Singleton, autoWire: true,
                getConstructorParameters: (ctx) =>
                    {
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("paths", "Localization, Modules/*/Localization");
                        return paras;
                    });

            builder.RegisterType<IObjectProvider<IEnumerable<DAF.Web.Menu.MenuGroup>>, ModuleMenuGroupProvider>(LifeTimeScope.Singleton, autoWire: true,
                getConstructorParameters: (ctx) =>
                    {
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("paths", "App_Data, Modules/*/App_Data");
                        paras.Add("fileName", "menu.js");
                        return paras;
                    });

            builder.RegisterType<IAppSettingProvider, AppSettingProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IBasicDataProvider, BasicDataProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<ICategoryProvider, CategoryProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IContentProvider, ContentProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IWebSiteProvider, WebSiteProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IPageTemplateProvider, PageTemplateProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IMenuProvider, MenuProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IUserGroupProvider, UserGroupProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<ITemplateTypeProvider, TemplateTypeProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IControlTypeProvider, ControlTypeProvider>(LifeTimeScope.Singleton);
            builder.RegisterType<IPageProvider, PageProvider>(LifeTimeScope.Singleton);

            builder.RegisterType<IAppEventHandler, CmsAppEventHandler>();
            builder.RegisterType<DAF.SSO.Client.IDefaultSessionProvider, CmsDefaultSessionProvider>(LifeTimeScope.Singleton);
        }
    }
}
