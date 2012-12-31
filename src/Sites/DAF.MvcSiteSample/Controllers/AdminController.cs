using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAF.Web;
using DAF.Web.Mvc;

namespace DAF.MvcSiteSample.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [OAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
