using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace DAF.Core
{
    public static class TypeExtensions
    {
        public static string AssemblyName(this Type type)
        {
            return type.Assembly.GetName().Name;
        }

        public static IEnumerable<ConstructorInfo> GetConstructorsWithDependency(this Type type, Type dependencyType)
        {
            return type.GetConstructors()
                .Where(x => x.GetParameters()
                .Any(xx => xx.ParameterType == dependencyType));
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithDependency(this Type type, Type dependencyType)
        {
            var props = type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                 .Select(p => new
                 {
                     PropertyInfo = p,
                     p.PropertyType,
                     IndexParameters = p.GetIndexParameters(),
                     Accessors = p.GetAccessors(false)
                 })
                 .Where(x => x.PropertyType == dependencyType
                     && x.IndexParameters.Count() == 0 // must not be an indexer
                     && (x.Accessors.Length != 1 || x.Accessors[0].ReturnType == typeof(void))); //must have get/set, or only set

            return props.Select(p => p.PropertyInfo);
        }

        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName, bool ignoreCase)
        {
            Func<PropertyInfo, bool> predicate = null;
            if (ignoreCase)
                predicate = o => o.Name.ToLower() == propertyName.ToLower();
            else
                predicate = o => o.Name == propertyName;

            return type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(predicate);
        }

        public static PropertyDescriptor GetPropertyDescriptor(this Type type, string propertyName, bool ignoreCase)
        {
            return System.ComponentModel.TypeDescriptor.GetProperties(type).Find(propertyName, ignoreCase);
        }
    }
}
