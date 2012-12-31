using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Metadata;
using System.Web.WebPages;
using System.Web.WebPages.Html;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using DAF.Core;
using DAF.Web;
using DAF.Web.Api.Metadata.Providers;
using DAF.Web.Api.Metadata;

namespace DAF.Web.Api.Metadata
{
    public partial class PageModel<T>
    {
        public IHtmlString TextBoxFor<P>(Expression<Func<T, P>> expression, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? null : expression.Compile().Invoke(Instance).ConvertTo<string>(null);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = page.Html.TextBox(propName, val, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString PasswordFor<P>(Expression<Func<T, P>> expression, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? null : expression.Compile().Invoke(Instance).ConvertTo<string>(null);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = page.Html.Password(propName, val, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString HiddenFor<P>(Expression<Func<T, P>> expression, object htmlAttributes = null)
        {
            var me = (MemberExpression)expression.Body;
            string val = null;
            if (Instance != null)
            {
                var obj = expression.Compile().Invoke(Instance);
                if (obj != null)
                    val = obj.ToString();
            }

            string propName = me.Member.Name;
            var html = page.Html.Hidden(propName, val, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return html;
        }

        public IHtmlString CheckBoxFor<P>(Expression<Func<T, P>> expression, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? null : expression.Compile().Invoke(Instance).ConvertTo<bool?>(null);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = val.HasValue ? page.Html.CheckBox(propName, val.Value, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)) : page.Html.CheckBox(propName, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString RadioButtonFor<P>(Expression<Func<T, P>> expression, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? null : expression.Compile().Invoke(Instance).ConvertTo<bool?>(null);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = val.HasValue ? page.Html.RadioButton(propName, val.Value, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)) : page.Html.RadioButton(propName, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString TextAreaFor<P>(Expression<Func<T, P>> expression, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? null : expression.Compile().Invoke(Instance).ConvertTo<string>(null);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = page.Html.TextArea(propName, val, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString TextAreaFor<P>(Expression<Func<T, P>> expression, int rows, int columns, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? null : expression.Compile().Invoke(Instance).ConvertTo<string>(null);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = page.Html.TextArea(propName, val, rows, columns, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString DropDownListFor<P>(Expression<Func<T, P>> expression, string defaultOption, IEnumerable<SelectListItem> selectList, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? default(P) : expression.Compile().Invoke(Instance);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = page.Html.DropDownList(propName, defaultOption, selectList, val, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString ListBoxFor<P>(Expression<Func<T, P>> expression, string defaultOption, IEnumerable<SelectListItem> selectList, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? default(P) : expression.Compile().Invoke(Instance);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = page.Html.ListBox(propName, defaultOption, selectList, val, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString ListBoxFor<P>(Expression<Func<T, P>> expression, string defaultOption, IEnumerable<SelectListItem> selectList, int size, bool allowMultiple = false, object htmlAttributes = null, bool includeValidation = true)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? default(P) : expression.Compile().Invoke(Instance);

            string propName = me.Member.Name;
            if (includeValidation)
                Validate(propName);

            var html = page.Html.ListBox(propName, defaultOption, selectList, val, size, allowMultiple, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            if (includeValidation)
            {
                return html.Concact(page.Html.ValidationMessage(propName));
            }
            return html;
        }

        public IHtmlString LabelFor<P>(Expression<Func<T, P>> expression, object htmlAttributes = null)
        {
            var me = (MemberExpression)expression.Body;
            var val = Instance == null ? null : expression.Compile().Invoke(Instance).ConvertTo<string>(null);

            string propName = me.Member.Name;

            var html = page.Html.Label(val, propName, UIHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return html;
        }


    }
}
