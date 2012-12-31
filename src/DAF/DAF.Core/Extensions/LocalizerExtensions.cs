using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DAF.Core.Localization;

namespace DAF.Core
{
    public static class ILocalizerExtensions
    {
        public static string Get<T>(this ILocalizer localizer, string name)
        {
            Type objType = typeof(T);
            return LocaleHelper.Localizer.Get(name, objType.AssemblyName());
        }

        public static string Format<T>(this ILocalizer localizer, string resourceName, params object[] paras)
        {
            return Format(localizer, resourceName, typeof(T).AssemblyName(), null, paras);
        }

        public static string Format(this ILocalizer localizer, string resourceName, params object[] paras)
        {
            return Format(localizer, resourceName, "DAF.Core", null, paras);
        }

        public static string Format(this ILocalizer localizer, string resourceName, string nameSpace, params object[] paras)
        {
            return Format(localizer, resourceName, nameSpace, null, paras);
        }

        public static string Format(this ILocalizer localizer, string resourceName, string nameSpace, string cultureInfo, params object[] paras)
        {
            var msg = localizer.Get(resourceName, nameSpace, cultureInfo);
            return string.Format(msg, paras);
        }
    }
}
