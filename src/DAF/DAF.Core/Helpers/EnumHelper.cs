using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 获取枚举类型对象集合
        /// </summary>
        /// <param name="typeName">枚举类型名称</param>
        /// <returns>对象集合</returns>
        public static List<Tuple<string, int>> GetObjectsFromEnum(string typeName, ILocalizer localizer = null)
        {
            Type enumType = Type.GetType(typeName);
            return GetObjectsFromEnum(enumType, localizer);
        }

        /// <summary>
        /// 获取枚举类型对象集合
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>对象集合</returns>
        public static List<Tuple<string, int>> GetObjectsFromEnum(Type enumType, ILocalizer localizer = null)
        {
            if (enumType == null || !enumType.IsEnum)
                return null;
            List<Tuple<string, int>> objs = new List<Tuple<string, int>>();
            Type typeDescription = typeof(DescriptionAttribute);
            Type typeBrowsable = typeof(BrowsableAttribute);
            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    string name;
                    int value;
                    object[] arr = field.GetCustomAttributes(typeBrowsable, true);
                    if (arr.Length > 0)
                    {
                        BrowsableAttribute bb = (BrowsableAttribute)arr[0];
                        if (bb.Browsable == false)
                            continue;
                    }
                    arr = field.GetCustomAttributes(typeDescription, true);
                    if (localizer == null)
                    {
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
                    string enumName = enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null).ToString();
                    value = Convert.ToInt32(Enum.Parse(enumType, enumName));

                    objs.Add(new Tuple<string, int>(name, value));
                }
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
                Type typeDescription = typeof(DescriptionAttribute);
                FieldInfo[] fields = enumType.GetFields();
                foreach (FieldInfo field in fields)
                {
                    if (field.FieldType.IsEnum == true && field.Name == value.ToString())
                    {
                        object[] arr = field.GetCustomAttributes(typeDescription, true);
                        if (localizer == null)
                        {
                            if (arr.Length > 0)
                            {
                                DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                                return aa.Description;
                            }
                        }
                        else
                        {
                            return localizer.Get(string.Format("{0}_{1}", enumType.Name, field.Name), enumType.AssemblyName());
                        }
                    }
                }
            }
            return value.ToString();
        }

    }
}
