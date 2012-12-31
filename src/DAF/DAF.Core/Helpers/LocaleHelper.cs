using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Reflection;
using Autofac;
using DAF.Core.Localization;

namespace DAF.Core
{
    public class LocaleHelper
    {
        public static IEnumerable<LocalizationInfo> GetSupportLanguages()
        {
            IObjectProvider<IEnumerable<LocalizationInfo>> provider = IOC.Current.ResolveOptional<IObjectProvider<IEnumerable<LocalizationInfo>>>();
            if (provider == null)
            {
                var culture = CultureInfo.CurrentCulture;
                return new LocalizationInfo[] {
                    new LocalizationInfo(){
                        Code = culture.Name,
                        Name = culture.EnglishName,
                        DisplayName = culture.DisplayName,
                        DateFormat = culture.DateTimeFormat.LongDatePattern,
                        TimeFormat = culture.DateTimeFormat.LongTimePattern,
                        Currency = culture.NumberFormat.CurrencySymbol,
                        TimeZone = 8.0
                    }
                };
            }
            return provider.GetObject();
        }

        public static ILocalizer Localizer
        {
            get
            {
                if (IOC.Current == null)
                    return NullLocalizer.Instance;
                var localizer = IOC.Current.ResolveOptional<ILocalizer>();
                return localizer ?? NullLocalizer.Instance;
            }
        }
    }
}
