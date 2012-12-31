using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autofac;
using BL = BLToolkit;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Reflection.MetadataProvider;
using DAF.Core;
using DAF.Core.Security;

namespace DAF.Core.Data.BLToolkit
{
    public class BLToolkitDataEventHandler : IAppEventHandler
    {
        public virtual void OnApplicationStart(IContainer container, object context)
        {
            MetadataProviderBase.CreateProvider = () =>
            {
                MetadataProviderList list = new MetadataProviderList();
                var metadataProvider = new DAF.Core.Data.BLToolkit.DataAnnotationMetadataProvider();
                list.InsertProvider(0, metadataProvider);
                return list;
            };
        }

        public virtual void OnApplicatoinExit(IContainer container, object context)
        {
        }

        public virtual int ExecuteOrder
        {
            get { return 1; }
        }
    }
}
