using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Localization
{
    public interface ILocalizer
    {
        string GetCurrentCultureInfo();
        void SetCurrentCultureInfo(string culture);
        string Get(string resourceName, string nameSpace = "DAF.Core", string cultureInfo = null);
        void Set(string resourceName, string value, string nameSpace = "DAF.Core", string cultureInfo = null);
        void Flush();
    }
}
