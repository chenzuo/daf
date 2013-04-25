using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.IOC;
using DAF.Core.Data;

namespace DAF.Core.Search
{
    public class RepositorySearchModule : IIocModule
    {
        public void Load(IIocBuilder builder)
        {
            builder.RegisterGeneric(typeof(IRepositoryEventHandler<>), typeof(SearchRepositoryEventHandler<>));
            builder.RegisterType<IAppEventHandler, SearchAppEventHandler>();
        }
    }
}
