using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.Core.Data.BLToolkit;
using DAF.Web.Menu;
using DAF.CMS.Site.Models;

namespace DAF.CMS.Site.DB.BT
{
    public class CMSDB : IEntitySet
    {
        public string ConnectionString
        {
            get { return "CMSDB"; }
        }

        public Type[] EntityTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(Content),
                    typeof(CategoryContent),
                    typeof(ContentRelation),
                    typeof(BasicDataItem),
                    typeof(AppSetting),
                    typeof(Category),
                    typeof(UserGroup),
                    typeof(UserGroupUser),
                    typeof(CategoryUserGroup),
                    typeof(MenuGroup),
                    typeof(MenuItem)
               };
            }
        }
    }
}