using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.WebPages.Html;
using System.ComponentModel;
using DAF.Core;
using DAF.Core.Localization;
using DAF.Core.Data;

namespace DAF.Web
{
    public class UIHelper
    {
        public static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            RouteValueDictionary result = new RouteValueDictionary();

            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
                }
            }

            return result;
        }

        public static IEnumerable<SelectListItem> EnumListItems<T>(int? selectedValue = null)
        {
            var enums = EnumHelper.GetObjectsFromEnum(typeof(T), LocaleHelper.Localizer);
            return enums.Select(o => new SelectListItem() { Text = o.Item1, Value = o.Item2.ToString(), Selected = selectedValue.HasValue && o.Item2 == selectedValue.Value });
        }

        #region controls

        public static IHtmlString SelectEditor(object nameValuePairs, string id = null, string emptyWords = null, object val = null, string attrs = null)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(id))
            {
                sb.AppendFormat("<select {0}>", attrs);
            }
            else
            {
                sb.AppendFormat("<select id=\"{0}\" name=\"{0}\" {1}>", id, attrs);
            }
            sb.AppendFormat("<option value=\"\">{0}</option>", emptyWords);
            RouteValueDictionary rv = new RouteValueDictionary(nameValuePairs);
            foreach (var en in rv)
            {
                sb.AppendFormat("<option value=\"{0}\"{2}>{1}</option>", en.Value, en.Key,
                    val != null && en.Value.ToString() == val.ToString() ? " selected" : "");
            }
            sb.Append("</select>");
            return new HtmlString(sb.ToString());
        }

        public static IHtmlString SelectEnumEditor<T>(string id = null, string emptyWords = null, object val = null, string attrs = null)
        {
            Type enumType = typeof(T);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<select id=\"{0}\" name=\"{0}\" {1}>", id, attrs);
            sb.AppendFormat("<option value=\"\">{0}</option>", emptyWords);
            var enums = EnumHelper.GetObjectsFromEnum(enumType, LocaleHelper.Localizer);
            foreach (var en in enums)
            {
                sb.AppendFormat("<option value=\"{0}\"{2}>{1}</option>", en.Item2, LocaleHelper.Localizer.Get(en.Item1, enumType.AssemblyName()),
                    val != null && en.Item2 == (int)val ? " selected" : "");
            }
            sb.Append("</select>");
            return new HtmlString(sb.ToString());
        }

        public static IHtmlString SelectRepositoryEditor<T>(Func<T, string> funcGetText, Func<T, object> funcGetValue, Expression<Func<T, bool>> predicate = null, string id = null, string emptyWords = null, object val = null, string attrs = null)
            where T : class
        {
            var repo = IOC.Current.GetService<IRepository<T>>(null);
            var entities = repo.Query(predicate);
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(id))
            {
                sb.AppendFormat("<select {0}>", attrs);
            }
            else
            {
                sb.AppendFormat("<select id=\"{0}\" name=\"{0}\" {1}>", id, attrs);
            }
            sb.AppendFormat("<option value=\"\">{0}</option>", emptyWords);
            foreach (var en in entities)
            {
                string text = funcGetText(en);
                object value = funcGetValue(en);
                sb.AppendFormat("<option value=\"{0}\"{2}>{1}</option>", value, text,
                    val != null && value.ToString() == val.ToString() ? " selected" : "");
            }
            sb.Append("</select>");
            return new HtmlString(sb.ToString());
        }

        public static IHtmlString TextEnumEditor<T>(string emptyWords = null, object val = null, string attrs = null)
        {
            Type enumType = typeof(T);
            StringBuilder sb = new StringBuilder();
            string text = emptyWords;
            if (val != null)
            {
                var enums = EnumHelper.GetObjectsFromEnum(enumType, LocaleHelper.Localizer);
                foreach (var en in enums)
                {
                    if (en.Item2 == (int)val)
                    {
                        text = en.Item1;
                        break;
                    }
                }
            }
            sb.AppendFormat("<span>{0}</span>", text);
            return new HtmlString(sb.ToString());
        }

        public static IHtmlString TextRepositoryEditor<T>(Func<T, string> funcGetText, Func<T, object> funcGetValue, Expression<Func<T, bool>> predicate = null, string emptyWords = null, object val = null, string attrs = null)
            where T : class
        {
            var repo = IOC.Current.GetService<IRepository<T>>(null);
            var entities = repo.Query(predicate);
            StringBuilder sb = new StringBuilder();
            string text = emptyWords;
            if (val != null)
            {
                foreach (var en in entities)
                {
                    object value = funcGetValue(en);
                    if (value == val)
                    {
                        text = funcGetText(en);
                        break;
                    }
                }
            }
            sb.AppendFormat("<span>{0}</span>", text);
            return new HtmlString(sb.ToString());
        }

        /*
        public static IHtmlString RepositoryAutoComplete<T>(string id, Func<T, string> funcGetText, Func<T, object> funcGetValue, Expression<Func<T, bool>> predicate = null, string emptyWords = null, object val = null, string attrs = null)
            where T : class
        {
            var repo = IOC.Current.GetService<IRepository<T>>(null);
            var entities = repo.Query(predicate);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"$(function() {{ 
                $('#{0}').autocomplete({{
			source: [", id);
            string text = emptyWords;
            if (val != null)
            {
                foreach (var en in entities)
                {
                    object value = funcGetValue(en);
                    string label = funcGetText(en);
                    if (value == val)
                    {
                        text = label;
                    }
                    sb.AppendFormat("{{ label:{0}, value:{1} }},", label, value);
                }
            }
            sb.AppendFormat("<span>{0}</span>", text);
            return new MvcHtmlString(sb.ToString());
        }
        */
        
        #endregion
    }
}
