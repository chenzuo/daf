using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using DAF.Core.Data;

namespace DAF.Core.Search
{
    public class RepositorySearchModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(SearchRepositoryEventHandler<>)).As(typeof(IRepositoryEventHandler<>));
            builder.RegisterType<SearchAppEventHandler>().As<IAppEventHandler>();
        }
    }
}
