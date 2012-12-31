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

namespace DAF.Web
{
    public class MetadataHelper
    {
        public static ModelMetadata GetModelMetadata<T>()
           where T : class, new()
        {
            var metadataProvider = GlobalConfiguration.Configuration.Services.GetModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForType(() => new T(), typeof(T));
            return metadata;
        }

        public static ModelMetadata GetProppertyMetadata<T>(string propName)
              where T : class, new()
        {
            var metadata = GetModelMetadata<T>();
            return metadata.Properties.FirstOrDefault(o => o.PropertyName == propName);
        }

        public static ModelMetadata GetProppertyMetadata<T, P>(Expression<Func<T, P>> expression)
              where T : class, new()
        {
            MemberExpression me = expression.Body as MemberExpression;
            if (me != null)
            {
                return GetProppertyMetadata<T>(me.Member.Name);
            }
            return null;
        }

        public static string CaptionFor<T, P>(Expression<Func<T, P>> expression)
             where T : class, new()
        {
            Type objType = typeof(T);
            MemberExpression me = expression.Body as MemberExpression;
            if (me != null)
            {
                string res = string.Format("{0}_{1}", objType.Name, me.Member.Name);
                return LocaleHelper.Localizer.Get(res, objType.AssemblyName());
            }
            return string.Format("{0}_{1}", objType.Name, typeof(P).Name);
        }


        public static string Caption<T>(string propName)
            where T : class, new()
        {
            var metadata = GetModelMetadata<T>();
            var prop = metadata.Properties.FirstOrDefault(o => o.PropertyName == propName);
            if (prop != null)
            {
                Type objType = typeof(T);
                string res = string.Format("{0}_{1}", objType.Name, propName);
                return LocaleHelper.Localizer.Get(res, objType.AssemblyName());
            }
            return propName;
        }
    }
}
