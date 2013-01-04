using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DAF.Core.Serialization.JsonNet
{
    public class JsonNetSerializer : IJsonSerializer
    {
        private JsonSerializerSettings settings;

        public JsonNetSerializer(PreserveReferencesHandling preserveReferencesHandling)
        {
            settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.PreserveReferencesHandling = preserveReferencesHandling;
            settings.ContractResolver = new EFContractResolver();
        }

        public string Serialize(object obj)
        {
            if (obj == null)
                return string.Empty;
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }

        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
