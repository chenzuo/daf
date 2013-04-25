using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Serialization;
using DAF.Core.DataMonitor;
using DAF.Core.Serialization.JsonNet;
using DAF.Core.DataMonitor.SqlServer;
using DAF.Core.Search.Lucene;
using DAF.Core.IOC.Autofac;

namespace DAF.Core.DataMonitor.SqlDataChangeMonitor
{
    static class Program
    {
        private static string[] IgnoreAssemblyFiles = new string[] { "system", "autofac", "bltoolkit", "entityframework", "microsoft", "newtonsoft", "nservicebus", "log4net", "emitmapper" };
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            Config.Current.IgnoreAssemblies(IgnoreAssemblyFiles).With();
            IIocBuilder builder = new AutofacBuilder();
            IocInstance.RegisterBuilder(builder);
            IocInstance.AutoRegister(Config.Current.TypesToScan);

            builder.RegisterModule(new JsonNetModule());
            builder.RegisterType<IChangeVersionManager, JsonChangeVersionManager>(LiftTimeScope.Singleton);
            builder.RegisterType<IObjectProvider<IEnumerable<VersionInfo>>, JsonFileObjectProvider<IEnumerable<VersionInfo>>>(LiftTimeScope.Singleton,
                getConstructorParameters: (ctx) =>
                {
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("jsonFile", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "versions.js"));
                    paras.Add("jsonSerializer", ctx.Resolve<IJsonSerializer>());
                    return paras;
                });

            builder.RegisterConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ioc.config"));

            IocInstance.Build();

            IocInstance.Start();

            if (args.Length > 0)
            {
                if (args.Any(o => o.ToLower() == "-monitor"))
                {
                    if (args.Any(o => o.ToLower() == "-service"))
                    {
                        ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[] 
                        { 
                            new MonitorService() 
                        };

                        ServiceBase.Run(ServicesToRun);
                    }
                    else
                    {
                        Monitors.Start();
                        Console.WriteLine(":: Monitoring ::");
                        Console.ReadLine();
                        Monitors.Stop();
                        Console.WriteLine("Stopping.....");
                    }
                }
                else if (args.Any(o => o.ToLower() == "-init"))
                {
                    Console.WriteLine(":: Initializing ::");
                    IBatchIndexWriter biw = IocInstance.Container.Resolve<IBatchIndexWriter>();
                    biw.Run();
                    Console.WriteLine(":: Initialized ::");
                    Console.ReadLine();
                    Console.WriteLine("Stopping.....");
                }
            }
        }
    }
}
