﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using DAF.Core;

namespace DAF.Web
{
    public class WebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<Configurations.AreaConfigurationProvider>().As<Core.Configurations.IConfigurationProvider>();
            builder.RegisterType<Core.FileSystem.LocalFileProvider>().As<Core.FileSystem.IFileSystemProvider>().Named<Core.FileSystem.IFileSystemProvider>("LocalFileSystem");
            builder.RegisterType<Caching.WebCacheManager>().As<Core.Caching.ICacheManager>().SingleInstance();
            builder.RegisterType<Core.Security.HashEncryptionProvider>().As<Core.Security.IPasswordEncryptionProvider>().SingleInstance();
            builder.RegisterType<Core.Generators.RNGRandomTextGenerator>().As<Core.Generators.IRandomTextGenerator>().SingleInstance();
            builder.RegisterType<Core.Generators.TicksIdGenerator>().As<Core.Generators.IIdGenerator>().SingleInstance();
#if DEBUG
            builder.RegisterType<Core.Data.NullTransactionManager>().As<Core.Data.ITransactionManager>();
#else
            builder.RegisterType<Core.Data.DefaultTransactionManager>().As<Core.Data.ITransactionManager>();
#endif
            builder.RegisterType<Security.DefaultCaptchaGenerator>().As<Security.ICaptchaGenerator>().SingleInstance();
            builder.RegisterType<Serialization.JavascriptConvert>().As<Core.Serialization.IJsonSerializer>().SingleInstance();

            builder.RegisterType<WebJsonFileObjectProvider<IEnumerable<DAF.Core.Localization.LocalizationInfo>>>().OnPreparing(pe =>
            {
                NamedParameter np = new NamedParameter("jsonFile", "~/App_Data/languages.js");
                pe.Parameters = new Parameter[] { np };
            }).As<IObjectProvider<IEnumerable<DAF.Core.Localization.LocalizationInfo>>>().SingleInstance();

            builder.RegisterType<WebJsonFileObjectProvider<IEnumerable<Menu.MenuGroup>>>().OnPreparing(pe =>
            {
                NamedParameter np = new NamedParameter("jsonFile", "~/App_Data/menu.js");
                pe.Parameters = new Parameter[] { np };
            }).As<IObjectProvider<IEnumerable<Menu.MenuGroup>>>().SingleInstance();

            builder.RegisterModule(new AutoWireModule<Core.Localization.ILocalizer>(
                o => o.RegisterType<Localization.JsonLocalizer>().As<Core.Localization.ILocalizer>().SingleInstance()));
            //builder.RegisterModule(new AutoWireModule<Core.Logging.ILogger>(
            //    o => o.RegisterType<Core.Logging.ILogger>().As<Core.Logging.ILogger>()));

            //builder.RegisterType<WebAppEventHandler>().As<IAppEventHandler>();
        }
    }
}
