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
using DAF.Core;
using DAF.Web;
using DAF.Web.Api.Metadata.Providers;
using DAF.Web.Api.Metadata;

namespace DAF.Web.Api.Metadata
{
    public partial class PageModel<T> where T : class, new()
    {
        private WebPage page;
        private T instance;
        private Type objType;
        private string objTypeName;
        private string objAsmName;

        public PageModel(WebPage page, T instance = null)
        {
            this.page = page;
            this.objType = typeof(T);
            this.objTypeName = this.objType.Name;
            this.objAsmName = this.objType.AssemblyName();
            this.instance = instance;
        }

        public string CaptionFor<P>(Expression<Func<T, P>> expression)
        {
            MemberExpression me = expression.Body as MemberExpression;
            if (me != null)
            {
                string res = string.Format("{0}_{1}", objTypeName, me.Member.Name);
                return LocaleHelper.Localizer.Get(res, objAsmName);
            }
            return string.Format("{0}_{1}", objTypeName, typeof(P).Name);
        }

        public string Caption(string propName)
        {
            var prop = Metadata.Properties.FirstOrDefault(o => o.PropertyName == propName);
            if (prop != null)
            {
                string res = string.Format("{0}_{1}", objTypeName, propName);
                return LocaleHelper.Localizer.Get(res, objAsmName);
            }
            return propName;
        }

        private ModelMetadata metadata;
        public ModelMetadata Metadata
        {
            get
            {
                if (metadata == null)
                    metadata = MetadataHelper.GetModelMetadata<T>();
                return metadata;
            }
        }

        public T Instance
        {
            get
            {
                return this.instance;
            }
        }
    }
}
