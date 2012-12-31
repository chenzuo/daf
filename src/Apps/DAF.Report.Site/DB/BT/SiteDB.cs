using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;
using DAF.Core.Data.BLToolkit;

namespace DAF.Report.Site.DB.BT
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