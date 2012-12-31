using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.Core.Data.BLToolkit;
using DAF.Core.Scheduling.Provider;

namespace DAF.Scheduling.Site.DB.BT
{
    public class ScheduleDB : IEntitySet
    {
        public string ConnectionString
        {
            get { return "ScheduleDB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(ScheduleTask),
                    typeof(ScheduleTaskTrigger),
                    typeof(SheduleTaskExecuteLog),
                };
            }
        }
    }
}