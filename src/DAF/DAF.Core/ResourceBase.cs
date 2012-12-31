using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DAF.Core;

namespace DAF.Core
{
    public abstract class ResourceBase<T> where T : ResourceBase<T>
    {
        public static string Locale(Expression<Func<T, ConstString>> expression, params object[] paras)
        {
            var type = typeof(T);
            MemberExpression me = expression.Body as MemberExpression;
            string locale = LocaleHelper.Localizer.Get(me.Member.Name, type.AssemblyName());
            if (paras != null && paras.Length > 0)
                return string.Format(locale, paras);
            return locale;
        }

        public static IEnumerable<string> ResourceNames()
        {
            Type type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public).Where(f => typeof(ConstString).IsAssignableFrom(f.ReflectedType));
            return fields.Select(f => f.Name);
        }
    }
}
