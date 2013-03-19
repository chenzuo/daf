using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core.Serialization;

namespace DAF.Core
{
    public class JsonHelper
    {
        public static string Serialize(object obj)
        {
            var js = IOC.Current.ResolveOptional<IJsonSerializer>();
            if (js != null)
            {
                return js.Serialize(obj);
            }
            return null;
        }

        public static T Deserialize<T>(string json)
        {
            var js = IOC.Current.ResolveOptional<IJsonSerializer>();
            if (js != null)
            {
                return js.Deserialize<T>(json);
            }
            return default(T);
        }
    }
}
