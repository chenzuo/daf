using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAF.Core
{
    public static class ObjectExtension
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            if (obj == null)
                return null;

            int idx = propertyName.IndexOf('.');
            if (idx > 0)
            {
                string subObjPropName = propertyName.Substring(0, idx);
                object subObj = GetPropertyValue(obj, subObjPropName);
                string propName = propertyName.Substring(idx + 1);
                return GetPropertyValue(subObj, propName);
            }

            PropertyDescriptor descriptor = obj.GetType().GetPropertyDescriptor(propertyName, true);
            return descriptor.GetValue(obj);
        }

        public static object[] GetPropertyValues(this object obj, string[] propertyNames)
        {
            if (obj == null)
                return null;

            if (propertyNames == null || propertyNames.Count() <= 0)
                return null;
            List<object> values = new List<object>();
            foreach (var propName in propertyNames)
            {
                var val = GetPropertyValue(obj, propName);
                values.Add(val);
            }
            return values.ToArray();
        }

        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            if (obj == null)
                return;

            PropertyDescriptor descriptor = obj.GetType().GetPropertyDescriptor(propertyName, true);

            object val = value.ConvertTo(descriptor.PropertyType, null);
            descriptor.SetValue(obj, val);
        }

        public static void SetPropertyValues(this object obj, NameValueCollection values)
        {
            foreach (string key in values.Keys)
            {
                SetPropertyValue(obj, key, values[key]);
            }
        }

        public static void SetPropertyValues(this object obj, Dictionary<string, object> values)
        {
            foreach (KeyValuePair<string, object> kvp in values)
            {
                SetPropertyValue(obj, kvp.Key, kvp.Value);
            }
        }

        public static T FromNameValueCollection<T>(this T obj, NameValueCollection values) where T : class, new()
        {
            SetPropertyValues(obj, values);
            return obj;
        }

        public static T FromDictionary<T>(this T obj, Dictionary<string, object> values) where T : class, new()
        {
            SetPropertyValues(obj, values);
            return obj;
        }

        public static T ConvertTo<T>(this object obj, T defaultValue)
        {
            var convertedObj = ConvertTo(obj, typeof(T), defaultValue);
            if (convertedObj == null)
                return defaultValue;

            return (T)convertedObj;
        }

        public static object ConvertTo(this object obj, Type type, object defaultValue)
        {
            if (obj == null || obj == DBNull.Value || string.IsNullOrWhiteSpace(obj.ToString()))
                return defaultValue;
            if (type.IsGenericType)
            {
                Type[] realTypes = type.GetGenericArguments();
                if (realTypes.Length == 1)
                {
                    return ConvertTo(obj, realTypes[0], defaultValue);
                }
            }

            //Type objType = obj.GetType();
            //string converterName = string.Format("DataConverter:{0}|{1}", objType, type);
            //IDataConverter converter = Unity.Current.ResolveOne<IDataConverter>(converterName);
            //if (converter != null)
            //{
            //    return converter.Convert(obj);
            //}

            if (type.IsEnum)
            {
                //int val = -1;
                //if (int.TryParse(obj.ToString(), out val))
                //{
                //    return ((object)val);
                //}
                //else
                //{
                return Enum.Parse(type, obj.ToString());
                //}
            }
            else if (typeof(Guid).IsAssignableFrom(type))
            {
                Guid guid = new Guid(obj.ToString());
                return guid as object;
            }
            else if (typeof(bool).IsAssignableFrom(type))
            {
                bool val = false;
                switch (obj.ToString().ToLower())
                {
                    case "on":
                    case "yes":
                    case "1":
                    case "true":
                        val = true;
                        break;
                    default:
                        val = false;
                        break;
                }
                return val as object;
            }
            else if (typeof(System.Xml.Linq.XElement).IsAssignableFrom(type))
            {
                return System.Xml.Linq.XElement.Parse(obj.ToString());
            }
            else
            {
                try
                {
                    return Convert.ChangeType(obj, type);
                }
                catch { }
            }
            return defaultValue;
        }

        public static T DefaultIfNull<T>(this T obj, T obj2)
        {
            if (typeof(T).Equals(typeof(String)))
            {
                return DefaultIf<T>(obj, obj2, (o) => o != null && !string.IsNullOrWhiteSpace(o.ToString()));
            }
            return DefaultIf<T>(obj, obj2, (o) => o != null);
        }

        public static T DefaultIf<T>(this T obj, T obj2, Func<T, bool> predicate)
        {
            if (predicate(obj))
                return obj;
            else
                return obj2;
        }

        public static P PropertyValue<T, P>(this T obj, Expression<Func<T, P>> propExpression, P defaultValue)
        {
            if (obj == null)
                return defaultValue;
            return propExpression.Compile().Invoke(obj);
        }

        public static object DefaultIfNull(this object obj, object obj2)
        {
            return DefaultIf(obj, obj2, (o) => o != null);
        }

        public static object DefaultIf(this object obj, object obj2, Func<object, bool> predicate)
        {
            if (predicate(obj))
                return obj;
            else
                return obj2;
        }

        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null)
                return null;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var val = prop.GetValue(obj);
                dic.Add(prop.Name, val);
            }

            return dic;
        }

        public static NameValueCollection ToNameValueCollection(this object obj)
        {
            if (obj == null)
                return null;

            NameValueCollection dic = new NameValueCollection();
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var val = prop.GetValue(obj);
                dic.Add(prop.Name, val == null ? string.Empty : val.ToString());
            }

            return dic;
        }

        public static IDictionary<string, object> GetObjectKey(this object obj)
        {
            if (obj == null)
                return null;

            Dictionary<string, object> keys = new Dictionary<string, object>();
            var keyProps = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(o => o.GetCustomAttribute<KeyAttribute>(true) != null)
                .Select(o => new 
                {
                    Name = o.Name,
                    PropertyInfo = o,
                    Order = o.GetCustomAttribute<ColumnAttribute>(true).PropertyValue(p => p.Order, 0)
                })
                .OrderBy(o => o.Order);
            if (keyProps != null && keyProps.Count() > 0)
            {
                keyProps.ForEach(p => keys.Add(p.Name, p.PropertyInfo.GetValue(obj)));
            }
            return keys;
        }
    }
}
