using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;

namespace DAF.Web.Api
{
    public class ShareActionContext
    {
        public static HttpActionContext Current { get; set; }
    }
}
