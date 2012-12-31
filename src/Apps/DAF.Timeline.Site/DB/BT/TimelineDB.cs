using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.SSO;
using DAF.SSO.Client;
using DAF.SSO.Server;
using DAF.Core.Data.BLToolkit;
using DAF.Timeline.Site.Models;

namespace DAF.Timeline.Site.DB.BT
{
    public class TimelineDB : IEntitySet
    {
        public string ConnectionString
        {
            get { return "TimelineDB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(TimelineItem),
                };
            }
        }
    }
}