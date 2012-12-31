using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace DAF.Core.Localization
{
    public class NullLocalizer : ILocalizer
    {
        private static readonly ILocalizer _instance = new NullLocalizer();

        public static ILocalizer Instance
        {
            get { return _instance; }
        }

        public string GetCurrentCultureInfo()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }

        public void SetCurrentCultureInfo(string culture)
        {
            try
            {
                CultureInfo cultureInfo = new CultureInfo(culture);
                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch
            {
            }
        }

        public string Get(string resourceName, string nameSpace = "Common", string cultureInfo = null)
        {
            return resourceName;
        }

        public void Set(string resourceName, string value, string nameSpace = "Common", string cultureInfo = null)
        {
        }

        public void Flush()
        {
        }
    }
}
