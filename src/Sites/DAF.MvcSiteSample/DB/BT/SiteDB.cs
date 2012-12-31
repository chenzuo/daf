using System;
using System.Collections.Generic;
using System.Linq;
using DAF.Core.Data.BLToolkit;

namespace DAF.MvcSiteSample.DB.BT
{
    public class SiteDB : IEntitySet
    {
        public string ConnectionString
        {
            get { return "SiteDB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    //typeof(Object),
                };
            }
        }
    }
}