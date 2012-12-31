using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;

namespace DAF.Core
{
    public static class XElementExtensions
    {
        public static string AttributeValue(this XElement ele, string attributeName, string defaultValue)
        {
            if (ele == null)
                return defaultValue;
            if (ele.Attribute(attributeName) == null)
                return defaultValue;
            return ele.Attribute(attributeName).Value;
        }

        public static T AttributeValue<T>(this XElement ele, string attributeName, T defaultValue)
        {
            if (ele == null)
                return defaultValue;
            if (ele.Attribute(attributeName) == null)
                return defaultValue;
            return ele.Attribute(attributeName).Value.ConvertTo<T>(defaultValue);
        }

        public static string AttributeValue(this XElement ele, string xpath, string attributeName, string defaultValue)
        {
            if (ele == null)
                return defaultValue;
            XElement cele = ele.XPathSelectElement(xpath);
            if (cele == null)
                return defaultValue;
            return cele.AttributeValue(attributeName, defaultValue);
        }

        public static T AttributeValue<T>(this XElement ele, string xpath, string attributeName, T defaultValue)
        {
            if (ele == null)
                return defaultValue;
            XElement cele = ele.XPathSelectElement(xpath);
            if (cele == null)
                return defaultValue;
            return cele.AttributeValue<T>(attributeName, defaultValue);
        }

        public static string ElementValue(this XElement ele, string xpath, string defaultValue)
        {
            if (ele == null)
                return defaultValue;
            XElement cele = ele.XPathSelectElement(xpath);
            if (cele == null)
                return defaultValue;
            return cele.Value;
        }

        public static T ElementValue<T>(this XElement ele, string xpath, T defaultValue)
        {
            if (ele == null)
                return defaultValue;
            XElement cele = ele.XPathSelectElement(xpath);
            if (cele == null)
                return defaultValue;
            return cele.Value.ConvertTo<T>(defaultValue);
        }

        public static T ConvertTo<T>(this XElement ele, IEnumerable<Tuple<string, Action<string, T>>> attrActions, Action<string, T> valAction = null)
            where T : new()
        {
            return ConvertTo<T>(ele, new T(), attrActions, valAction);
        }

        public static T ConvertTo<T>(this XElement ele, T obj, IEnumerable<Tuple<string, Action<string, T>>> attrActions, Action<string, T> valAction = null)
        {
            if (ele == null || obj == null)
                return default(T);
            foreach (var attrAct in attrActions)
            {
                attrAct.Item2(ele.AttributeValue(attrAct.Item1, string.Empty), obj);
            }

            if (valAction != null)
            {
                valAction(ele.Value, obj);
            }

            return obj;
        }
    }
}
