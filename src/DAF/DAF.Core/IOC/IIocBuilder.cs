using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace DAF.Core.IOC
{
    public interface IIocBuilder
    {
        void RegisterType(Type interfaceType, Type serviceType, LifeTimeScope scope = LifeTimeScope.Transient, string name = null, bool autoWire = false,
            Func<IIocContext, IDictionary<string, object>> getConstructorParameters = null, Func<IIocContext, object, object> onActivating = null, Action<IIocContext, object> onActivated = null);
        void RegisterInstance(Type interfaceType, object instance, LifeTimeScope scope = LifeTimeScope.Transient, string name = null, bool autoWire = false);
        void RegisterFactory<IT, TT>(LifeTimeScope scope = LifeTimeScope.Transient)
            where TT : class, IServiceFactory<IT>;
        void RegisterGeneric(Type interfaceType, Type serviceType, LifeTimeScope scope = LifeTimeScope.Transient);
        IIocContainer Build();
    }

    public static class IIocBuilderExtensions
    {
        public static void RegisterType<IT, TT>(this IIocBuilder builder, LifeTimeScope scope = LifeTimeScope.Transient, string name = null, bool autoWire = false,
            Func<IIocContext, IDictionary<string, object>> getConstructorParameters = null, Func<IIocContext, TT, TT> onActivating = null, Action<IIocContext, TT> onActivated = null)
            where TT : class, IT
        {
            Func<IIocContext, object, object> onObjActivaing = null;
            Action<IIocContext, object> onObjActivated = null;

            if (onActivating != null)
            {
                onObjActivaing = (ctx, obj) =>
                    {
                        var nobj = onActivating(ctx, (TT)obj);
                        return nobj;
                    };
            }


            if (onActivated != null)
            {
                onObjActivated = (ctx, obj) =>
                {
                    onActivated(ctx, (TT)obj);
                };
            }

            builder.RegisterType(typeof(IT), typeof(TT), scope, name, autoWire, getConstructorParameters, onObjActivaing, onObjActivated);
        }

        public static void RegisterInstance<IT, TT>(this IIocBuilder builder, TT instance, LifeTimeScope scope = LifeTimeScope.Transient, string name = null, bool autoWire = false)
        {
            builder.RegisterInstance(typeof(IT), instance, scope, name, autoWire);
        }

        public static void RegisterModule(this IIocBuilder builder, IIocModule module)
        {
            module.Load(builder);
        }

        public static void RegisterConfig(this IIocBuilder builder, string file)
        {
            if (File.Exists(file))
            {
                var doc = XElement.Load(file);
                RegisterConfig(builder, doc);
            }
        }

        public static void RegisterConfig(this IIocBuilder builder, XElement root)
        {
            var compEles = root.Element("components").Elements("component");
            foreach (var ele in compEles)
            {
                var serviceType = Type.GetType(ele.Attribute("type").Value);
                var interfaceType = Type.GetType(ele.Attribute("service").Value);
                LifeTimeScope scope = LifeTimeScope.Transient;
                if (ele.Attribute("scope") != null)
                {
                    switch (ele.Attribute("scope").Value.ToLower())
                    {
                        case "singleton":
                            scope = LifeTimeScope.Singleton;
                            break;
                        case "workunit":
                            scope = LifeTimeScope.WorkUnit;
                            break;
                        case "transiant":
                        default:
                            scope = LifeTimeScope.Transient;
                            break;
                    }
                }
                bool autoWire = ele.AttributeValue<bool>("autowire", false);
                string name = ele.AttributeValue("name", null);
                Func<IIocContext, IDictionary<string, object>> getConstructorParameters = null; 
                if (ele.HasElements)
                {
                    var paraEles = ele.Element("parameters").Elements("parameter");
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    foreach (var pele in paraEles)
                    {
                        paras.Add(pele.Attribute("name").Value, pele.Attribute("value").Value);
                    }
                    getConstructorParameters = (ctx) => { return paras; };
                }

                builder.RegisterType(interfaceType, serviceType, scope, name, autoWire, getConstructorParameters);
            }

            var moduleEles = root.Element("modules").Elements("module");
            foreach (var ele in moduleEles)
            {
                var moduleType = Type.GetType(ele.Attribute("type").Value);
                IIocModule module = Activator.CreateInstance(moduleType) as IIocModule;
                builder.RegisterModule(module);
            }
        }
    }
}
