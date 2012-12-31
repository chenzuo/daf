using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.Core.Data.BLToolkit;

namespace DAF.Social.Models.DB.BT
{
    public class SocialDB : IEntitySet
    {
        public string ConnectionString
        {
            get { return "SocialDB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(Contact),
                    typeof(Org),
                    typeof(Person),
                    typeof(School),
                    typeof(PersonContact),
                    typeof(PersonLink),
                    typeof(PersonCertificate),
                    typeof(WorkResume),
                    typeof(StudyResume),
                };
            }
        }
    }
}