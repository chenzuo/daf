using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using Newtonsoft.Json;

namespace DAF.Core.Serialization.JsonNet
{
    public class JsonNetModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonNetSerializer>().As<IJsonSerializer>().OnPreparing(pe =>
                {
                    NamedParameter np = new NamedParameter("preserveReferencesHandling", PreserveReferencesHandling.None);
                    pe.Parameters = new Parameter[] { np };
                });
        }
    }
}
