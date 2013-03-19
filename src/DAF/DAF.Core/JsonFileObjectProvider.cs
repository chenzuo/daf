using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Core.Serialization;

namespace DAF.Core
{
    public class JsonFileObjectProvider<T> : IObjectProvider<T>
    {
        protected string jsonFile;
        protected IJsonSerializer jsonSerializer;
        
        public JsonFileObjectProvider(string jsonFile, IJsonSerializer jsonSerializer)
        {
            this.jsonFile = jsonFile;
            this.jsonSerializer = jsonSerializer;
        }

        protected virtual void InitializeObject(T obj)
        {
        }

        public virtual T GetObject()
        {
            string json = File.ReadAllText(jsonFile);
            if (!string.IsNullOrEmpty(json))
            {
                T obj = jsonSerializer.Deserialize<T>(json);
                InitializeObject(obj);
                return obj;
            }
            return default(T);
        }

        public virtual void SaveObject(T obj)
        {
            var json = jsonSerializer.Serialize(obj);
            File.WriteAllText(jsonFile, json);
        }
    }
}
