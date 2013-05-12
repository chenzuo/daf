using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DAF.Core.IOC
{
    public interface IIocContext
    {
        bool IsRegistered(Type type, string name = null);
        object Resolve(Type type, string name = null);
        object ResolveOptional(Type type, string name = null);
        IEnumerable<object> ResolveAll(Type type);
    }

    public static class IIocContextExtensions
    {
        public static bool IsRegistered<T>(this IIocContext context, string name = null)
        {
            return context.IsRegistered(typeof(T), name);
        }

        public static T Resolve<T>(this IIocContext context, string name = null)
        {
            return (T)context.Resolve(typeof(T), name);
        }

        public static T ResolveOptional<T>(this IIocContext context, string name = null)
        {
            return (T)context.ResolveOptional(typeof(T), name);
        }

        public static IEnumerable<T> ResolveAll<T>(this IIocContext context)
        {
            return context.ResolveAll(typeof(T)).Select(o => (T)o);
        }
    }
}
