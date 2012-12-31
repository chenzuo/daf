using System;
using System.Collections.Generic;
using System.Linq;
using DAF.Core;

namespace DAF.Web
{
    public class Resources : ResourceBase<Resources>
    {
        public ConstString SiteLogo = "SiteLogo";
        public ConstString SiteName = "SiteName";
        public ConstString DefaultTitle = "DefaultTitle";
        public ConstString DefaultMetaKeywords = "DefaultMetaKeywords";
        public ConstString DefaultMetaDescription = "DefaultMetaDescription";
        public ConstString DefaultTheme = "DefaultTheme";
        public ConstString DefaultSkin = "DefaultSkin";
        public ConstString DefaultLocale = "DefaultLocale";
        public ConstString DefaultTimeZone = "DefaultTimeZone";

        public ConstString Error_Title = "Error_Title";
    }
}
