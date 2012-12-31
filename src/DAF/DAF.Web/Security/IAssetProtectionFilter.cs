using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DAF.Web.Security
{
    public interface IAssetProtectionFilter
    {
        bool AllowAccess(HttpContext context);
    }
}
