using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Metadata;
using System.Web.WebPages;
using System.ComponentModel.DataAnnotations;
using Validator = System.Web.WebPages.Validator;
using DAF.Core;
using DAF.Web;
using DAF.Web.Api.Metadata.Providers;
using DAF.Web.Api.Metadata;

namespace DAF.Web.Api.Metadata
{
    public partial class PageModel<T>
    {
        public void Validate(params string[] propsToValidate)
        {
            if (propsToValidate != null && propsToValidate.Length > 0)
            {
                Validate(p => propsToValidate.Contains(p));
            }
            else
            {
                Validate();
            }
        }

        public void Validate(Func<string, bool> isPropValidate = null)
        {
            foreach (var prop in Metadata.Properties)
            {
                if (isPropValidate != null && !isPropValidate(prop.PropertyName))
                    continue;

                CachedDataAnnotationsModelMetadata2 prop2 = prop as CachedDataAnnotationsModelMetadata2;
                if (prop2 != null)
                {
                    ValidateForProperty(prop2);
                }
            }
        }

        public void Validate(string propName)
        {
            var prop2 = MetadataHelper.GetProppertyMetadata<T>(propName) as CachedDataAnnotationsModelMetadata2;
            if (prop2 != null)
            {
                ValidateForProperty(prop2);
            }
        }

        public void ValidateFor<P>(Expression<Func<T, P>> expression)
        {
            var prop2 = MetadataHelper.GetProppertyMetadata<T, P>(expression) as CachedDataAnnotationsModelMetadata2;
            if (prop2 != null)
            {
                ValidateForProperty(prop2);
            }
        }

        private void ValidateForProperty(CachedDataAnnotationsModelMetadata2 prop2)
        {
            string propName = prop2.PropertyName;
            string propLocaleName = LocaleHelper.Localizer.Get(string.Format("{0}_{1}", objTypeName, prop2.PropertyName), objAsmName);
            if (prop2.IsRequried())
            {
                string message = DAF.Core.Resources.Locale(o => o.Data_Requried);
                page.Validation.RequireField(propName, string.Format(message, propLocaleName));
            }

            StringLengthAttribute sl = prop2.Attributes.OfType<StringLengthAttribute>().FirstOrDefault();
            if (sl != null)
            {
                if (sl.MaximumLength > 0)
                {
                    string message = DAF.Core.Resources.Locale(o => o.Data_StringLengthRange);
                    page.Validation.Add(propName,
                        Validator.StringLength(sl.MaximumLength, sl.MinimumLength,
                        string.Format(message, propLocaleName, sl.MinimumLength, sl.MaximumLength)));
                }
                else
                {
                    string message = DAF.Core.Resources.Locale(o => o.Data_StringLength);
                    page.Validation.Add(propName,
                        Validator.StringLength(sl.MaximumLength, 0,
                        string.Format(message, propLocaleName, sl.MaximumLength)));
                }
            }

            RegularExpressionAttribute re = prop2.Attributes.OfType<RegularExpressionAttribute>().FirstOrDefault();
            if (re != null)
            {
                string message = DAF.Core.Resources.Locale(o => o.Data_Regex);
                page.Validation.Add(propName, Validator.Regex(re.Pattern, string.Format(message, propLocaleName)));
            }

            DataTypeAttribute dt = prop2.Attributes.OfType<DataTypeAttribute>().FirstOrDefault();
            if (dt != null)
            {
                string message = null;
                string regex = null;
                switch (dt.DataType)
                {
                    case DataType.Date:
                        regex = @"\d{4}-?\d{2}-?\d{2}";
                        message = DAF.Core.Resources.Locale(o => o.Data_Regex);
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(regex))
                    page.Validation.Add(propName, Validator.Regex(regex, string.Format(message, propLocaleName)));
            }

            CompareAttribute ca = prop2.Attributes.OfType<CompareAttribute>().FirstOrDefault();
            if (ca != null)
            {
                string otherPropLocalName = LocaleHelper.Localizer.Get(string.Format("{0}_{1}", objTypeName, ca.OtherProperty), objAsmName);
                string message = DAF.Core.Resources.Locale(o => o.Data_EqualsTo);
                page.Validation.Add(propName,
                    Validator.EqualsTo(ca.OtherProperty,
                    string.Format(message, propLocaleName, otherPropLocalName)));
            }

        }
    }
}
