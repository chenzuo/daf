using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace DAF.Core.Search
{
    public class SearchAppEventHandler : IAppEventHandler
    {
        public void OnApplicationStart(IContainer container, object context)
        {
            var searchProvider = container.ResolveOptional<ISearchProvider>();
            if (searchProvider != null)
                searchProvider.Initialize();
        }

        public void OnApplicatoinExit(IContainer container, object context)
        {
            var searchProvider = container.ResolveOptional<ISearchProvider>();
            if (searchProvider != null)
                searchProvider.Close();
        }

        public int ExecuteOrder
        {
            get { return DAF.Core.ExecuteOrder.Later; }
        }
    }
}
