using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Core.Serialization;

namespace DAF.Core
{
    public class JsonObjectProvider<T> : IObjectProvider<T>
    {
        protected string json;
        protected IJsonSerializer jsonConvert;

        public JsonObjectProvider(string json, IJsonSerializer jsonConvert)
        {
            this.json = json;
            this.jsonConvert = jsonConvert;
        }

        protected virtual void InitializeObject(T obj)
        {
        }

        public virtual T GetObject()
        {
            if(!string.IsNullOrEmpty(json))
            {
                T obj = jsonConvert.Deserialize<T>(json);
                InitializeObject(obj);
                return obj;
            }
            return default(T);
        }
    }
}
