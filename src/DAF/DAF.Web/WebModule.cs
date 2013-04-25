using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.IOC;

namespace DAF.Web
{
    public class WebModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<Core.FileSystem.IFileSystemProvider, Core.FileSystem.LocalFileProvider>(name: "LocalFileSystem");
            builder.RegisterType<Core.Caching.ICacheManager, Caching.WebCacheManager>();
            builder.RegisterType<Core.Security.IPasswordEncryptionProvider, Core.Security.HashEncryptionProvider>(LiftTimeScope.Singleton);
            builder.RegisterType<Core.Generators.IRandomTextGenerator, Core.Generators.RNGRandomTextGenerator>(LiftTimeScope.Singleton);
            builder.RegisterType<Core.Generators.IIdGenerator, Core.Generators.TicksIdGenerator>(LiftTimeScope.Singleton);
#if DEBUG
            builder.RegisterType<Core.Data.ITransactionManager, Core.Data.NullTransactionManager>();
#else
            builder.RegisterType<Core.Data.ITransactionManager, Core.Data.DefaultTransactionManager>();
#endif
            builder.RegisterType<Security.ICaptchaGenerator, Security.DefaultCaptchaGenerator>(LiftTimeScope.Singleton);
            builder.RegisterType<Core.Serialization.IJsonSerializer, Serialization.JavascriptConvert>(LiftTimeScope.Singleton);

            builder.RegisterType<IObjectProvider<IEnumerable<DAF.Core.Localization.LocalizationInfo>>, WebJsonFileObjectProvider<IEnumerable<DAF.Core.Localization.LocalizationInfo>>>(LiftTimeScope.Singleton,
                getConstructorParameters: (ctx) =>
                    {
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("jsonFile", "~/App_Data/languages.js");
                        return paras;
                    });

            builder.RegisterType<IObjectProvider<IEnumerable<Menu.MenuGroup>>, WebJsonFileObjectProvider<IEnumerable<Menu.MenuGroup>>>(LiftTimeScope.Singleton,
                getConstructorParameters: (ctx) =>
                    {
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("jsonFile", "~/App_Data/menu.js");
                        return paras;
                    });

            builder.RegisterType<Core.Localization.ILocalizer, Localization.JsonLocalizer>(LiftTimeScope.Singleton, autoWire: true,
                getConstructorParameters: (ctx) =>
                    {
                        Dictionary<string, object> paras = new Dictionary<string, object>();
                        paras.Add("paths", "Localization");
                        return paras;
                    });

            //builder.RegisterType<Core.Logging.ILogger, Core.Logging.NullLogger>(LiftTimeScope.Singleton, autoWire: true);
        }
    }
}
