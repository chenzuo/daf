using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using DAF.Core.Serialization;

namespace DAF.Web.Serialization
{
    public class JavascriptConvert : IJsonSerializer
    {
        public T Deserialize<T>(string json)
        {
            JavaScriptSerializer jsSer = new JavaScriptSerializer();
            return jsSer.Deserialize<T>(json);
        }

        public string Serialize(object obj)
        {
            JavaScriptSerializer jsSer = new JavaScriptSerializer();
            return jsSer.Serialize(obj);
        }
    }
}
