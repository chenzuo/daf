using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.CMS
{
    public class CmsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppSettingProvider>().As<IAppSettingProvider>().SingleInstance();
            builder.RegisterType<BasicDataProvider>().As<IBasicDataProvider>().SingleInstance();
            builder.RegisterType<CategoryProvider>().As<ICategoryProvider>().SingleInstance();
            builder.RegisterType<ContentProvider>().As<IContentProvider>().SingleInstance();
            builder.RegisterType<WebSiteProvider>().As<IWebSiteProvider>().SingleInstance();
            builder.RegisterType<PageTemplateProvider>().As<IPageTemplateProvider>().SingleInstance();
            builder.RegisterType<WebControlProvider>().As<IWebControlProvider>().SingleInstance();
            builder.RegisterType<MenuProvider>().As<IMenuProvider>().SingleInstance();
            builder.RegisterType<UserGroupProvider>().As<IUserGroupProvider>().SingleInstance();
            builder.RegisterType<TemplateTypeProvider>().As<ITemplateTypeProvider>().SingleInstance();
            builder.RegisterType<ControlTypeProvider>().As<IControlTypeProvider>().SingleInstance();
            builder.RegisterType<PageProvider>().As<IPageProvider>().SingleInstance();
        }
    }
}
