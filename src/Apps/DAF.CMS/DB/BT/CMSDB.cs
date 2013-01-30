using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAF.Core.Data.BLToolkit;
using DAF.CMS.Models;

namespace DAF.CMS.DB.BT
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
                    typeof(AppSetting),
                    typeof(BasicDataItem),
                    typeof(Category),
                    typeof(Content),
                    typeof(CategoryContent),
                    typeof(ContentRelation),
                    typeof(UserGroup),
                    typeof(UserGroupUser),
                    typeof(CategoryUserGroup),
                    typeof(WebSite),
                    typeof(SubSite),
                    typeof(PageTemplate),
                    typeof(PageTemplateControl),
                    typeof(WebPage),
                    typeof(WebPageControl),
                    typeof(SiteMenuGroup),
                    typeof(SiteMenuItem)
              };
            }
        }
    }
}