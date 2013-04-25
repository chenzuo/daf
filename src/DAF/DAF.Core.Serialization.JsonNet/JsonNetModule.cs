using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using DAF.Core.IOC;

namespace DAF.Core.Serialization.JsonNet
{
    public class JsonNetModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterType<IJsonSerializer, JsonNetSerializer>(
                getConstructorParameters: (ctx) =>
                {
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras.Add("preserveReferencesHandling", PreserveReferencesHandling.None);
                    return paras;
                });
        }
    }
}
