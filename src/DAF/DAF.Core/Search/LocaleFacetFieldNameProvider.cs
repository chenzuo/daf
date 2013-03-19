using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Localization;

namespace DAF.Core.Search
{
    public class LocaleFacetFieldNameProvider : IFacetFieldNameProvider
    {
        public string GetMapName(string typeName, string fieldName)
        {
            return LocaleHelper.Localizer.Get(string.Format("{0}_{1}", typeName, fieldName));
        }
    }
}
