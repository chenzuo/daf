using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;

namespace DAF.Web.Api
{
    public static class ModelStateDictionaryExtension
    {
        public static Dictionary<string, IEnumerable<string>> BuildValidationSummary(this ModelStateDictionary modelState)
        {
            if (modelState == null)
                return null;
            var errors = new Dictionary<string, IEnumerable<string>>();
            foreach (KeyValuePair<string, ModelState> keyValue in modelState)
            {
                errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
            }

            return errors;
        }
    }
}
