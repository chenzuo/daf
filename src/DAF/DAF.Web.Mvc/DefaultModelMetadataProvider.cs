using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DAF.Core;
using DAF.Core.Localization;

namespace DAF.Web.Mvc
{
    public class DefaultModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var md = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            if (containerType != null)
            {
                md.DisplayName = LocaleHelper.Localizer.Get(string.Format("{0}_{1}", containerType.Name, propertyName), containerType.AssemblyName());
                var valAttrs = attributes.OfType<ValidationAttribute>();
                if (valAttrs.Any())
                {
                    valAttrs.ForEach(o =>
                    {
                        o.ErrorMessage = LocaleHelper.Localizer.Get(o.GetType().Name.Replace("Attribute", ""), "DAF.Core");
                    });
                }
            }

            return md;
        }
    }
}
