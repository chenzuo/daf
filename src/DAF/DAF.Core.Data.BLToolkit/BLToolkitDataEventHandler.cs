using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BL = BLToolkit;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Reflection.MetadataProvider;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.IOC;

namespace DAF.Core.Data.BLToolkit
{
    public class BLToolkitDataEventHandler : IAppEventHandler
    {
        public virtual void OnApplicationStart(IIocContainer container, object context)
        {
            MetadataProviderBase.CreateProvider = () =>
            {
                MetadataProviderList list = new MetadataProviderList();
                var metadataProvider = new DAF.Core.Data.BLToolkit.DataAnnotationMetadataProvider();
                list.InsertProvider(0, metadataProvider);
                return list;
            };
        }

        public virtual void OnApplicatoinExit(IIocContainer container, object context)
        {
        }

        public virtual int ExecuteOrder
        {
            get { return 1; }
        }
    }
}
