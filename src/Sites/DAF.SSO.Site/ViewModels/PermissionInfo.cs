using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAF.SSO.Site.ViewModels
{
    public class PermissionInfo
    {
        public string PermissionName { get; set; }
        public PermissionType PermissionType { get; set; }
        public int Position { get; set; }
        public string Uri { get; set; }
        public string GroupName { get; set; }
        public bool HasPermitted { get; set; }
    }
}