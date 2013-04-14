using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DAF.Web
{
    public static class ModelStateDictionaryExtension
    {
        public static string BuildValidationSummary(this ModelStateDictionary dic)
        {
            if (dic == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var k in dic.Keys)
            {
                var ms = dic[k];
                if (ms.Errors != null && ms.Errors.Count() > 0)
                {
                    foreach (var err in ms.Errors)
                    {
                        sb.AppendLine();
                        sb.AppendFormat(err.ErrorMessage, k, ms.Value);
                    }                    
                }
            }

            return sb.ToString();
        }
    }
}
