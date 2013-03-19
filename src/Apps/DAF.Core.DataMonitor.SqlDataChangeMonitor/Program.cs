using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using Autofac;
using DAF.Core;
using DAF.Core.Serialization;
using DAF.Core.DataMonitor;
using DAF.Core.Serialization.JsonNet;
using DAF.Core.DataMonitor.SqlServer;
using DAF.Core.Search.Lucene;

namespace DAF.Core.DataMonitor.SqlDataChangeMonitor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            Config.Current.InitializeDefaultIOC(b =>
                {
                    b.RegisterModule<JsonNetModule>();

                    b.RegisterType<JsonFileObjectProvider<IEnumerable<VersionInfo>>>().As<IObjectProvider<IEnumerable<VersionInfo>>>()
                        .OnPreparing(p =>
                    {
                        var p1 = new NamedParameter("jsonFile", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "versions.js"));
                        var p2 = new NamedParameter("jsonSerializer", p.Context.Resolve<IJsonSerializer>());
                        p.Parameters = new [] { p1, p2 };
                    }).SingleInstance();

                    b.RegisterType<JsonChangeVersionManager>().As<IChangeVersionManager>().SingleInstance();
                });
            IOC.Current.Start();

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
                    IBatchIndexWriter biw = IOC.Current.GetService<IBatchIndexWriter>();
                    biw.Run();
                    Console.WriteLine(":: Initialized ::");
                    Console.ReadLine();
                    Console.WriteLine("Stopping.....");
                }
            }
        }
    }
}
