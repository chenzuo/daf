using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Specialized;
using DAF.Core.Localization;

namespace DAF.Core
{
    /// <summary>
    /// 枚举辅助器
    /// </summary>
    public class EnumHelper
    {
        private static ConcurrentDictionary<Type, List<Tuple<string, object>>> enumDic = new ConcurrentDictionary<Type, List<Tuple<string, object>>>();

        /// <summary>
        /// 获取枚举类型对象集合
        /// </summary>
        /// <param name="typeName">枚举类型名称</param>
        /// <returns>对象集合</returns>
        public static List<Tuple<string, object>> GetObjectsFromEnum(string typeName, ILocalizer localizer = null)
        {
            Type enumType = Type.GetType(typeName);
            return GetObjectsFromEnum(enumType, localizer);
        }

        /// <summary>
        /// 获取枚举类型对象集合
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>对象集合</returns>
        public static List<Tuple<string, object>> GetObjectsFromEnum(Type enumType, ILocalizer localizer = null)
        {
            if (enumType == null || !enumType.IsEnum)
                return null;
            if (enumDic.ContainsKey(enumType))
                return enumDic[enumType];

            List<Tuple<string, object>> objs = new List<Tuple<string, object>>();
            Type typeDescription = typeof(DescriptionAttribute);
            Type typeBrowsable = typeof(BrowsableAttribute);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    string name;
                    object value;
                    object[] arr = field.GetCustomAttributes(typeBrowsable, true);
                    if (arr.Length > 0)
                    {
                        BrowsableAttribute bb = (BrowsableAttribute)arr[0];
                        if (bb.Browsable == false)
                            continue;
                    }
                    if (localizer == null)
                    {
                        arr = field.GetCustomAttributes(typeDescription, true);
                        if (arr.Length > 0)
                        {
                            DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                            name = aa.Description;
                        }
                        else
                        {
                            name = field.Name;
                        }
                    }
                    else
                    {
                        name = localizer.Get(string.Format("{0}_{1}", enumType.Name, field.Name), enumType.AssemblyName());
                    }
                    //string enumName = enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null).ToString();
                    value = Enum.Parse(enumType, field.Name);

                    objs.Add(new Tuple<string, object>(name, value));
                }
            }

            if (objs.Count > 0)
            {
                enumDic.AddOrUpdate(enumType, objs, (o, n) => n);
            }

            return objs;
        }

        /// <summary>
        /// 获取枚举类型值对应的描述名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="value">枚举类型值</param>
        /// <returns>描述名称</returns>
        public static string GetNameFromEnum(Type enumType, object value, ILocalizer localizer = null)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            if (enumType.IsEnum)
            {
                List<Tuple<string, object>> enumList = null;
                if (enumDic.ContainsKey(enumType))
                {
                    enumList = GetObjectsFromEnum(enumType, localizer);
                }
                else
                {
                    enumList = enumDic.GetOrAdd(enumType, new List<Tuple<string, object>>());
                }

                if (enumList != null)
                {
                    foreach (var el in enumList)
                    {
                        if (el.Item2 == value)
                            return el.Item1;
                    }
                }
            }
            return value.ToString();
        }
    }
}
