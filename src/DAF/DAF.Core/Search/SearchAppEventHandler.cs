using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.IOC;

namespace DAF.Core.Search
{
    public class SearchAppEventHandler : IAppEventHandler
    {
        public void OnApplicationStart(IIocContainer container, object context)
        {
            var searchProvider = container.ResolveOptional<ISearchProvider>();
            if (searchProvider != null)
                searchProvider.Initialize();
        }

        public void OnApplicatoinExit(IIocContainer container, object context)
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
