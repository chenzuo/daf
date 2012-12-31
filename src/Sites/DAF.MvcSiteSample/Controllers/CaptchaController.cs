using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DAF.Core;
using DAF.Core.Security;
using DAF.Core.Configurations;
using DAF.Core.Generators;
using DAF.Web.Security;

namespace DAF.MvcSiteSample.Controllers
{
    public class CaptchaController : Controller
    {
        private string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ3456789";
        private int Length = 5;
        private IRandomTextGenerator radomTextGenerator;
        private ICaptchaGenerator captchaGenerator;

        public CaptchaController(IRandomTextGenerator radomTextGenerator, ICaptchaGenerator captchaGenerator)
        {
            this.radomTextGenerator = radomTextGenerator;
            this.captchaGenerator = captchaGenerator;
            string allowChars = ConfigurationManager.AppSettings["CaptchaChars"];
            if (!string.IsNullOrWhiteSpace(allowChars))
                AllowedChars = allowChars;
        }

        public ActionResult Get()
        {
            int width = 100;
            int height = 30;
            if (!string.IsNullOrEmpty(Request.QueryString["w"]))
                int.TryParse(Request.QueryString["w"], out width);
            if (!string.IsNullOrEmpty(Request.QueryString["h"]))
                int.TryParse(Request.QueryString["h"], out height);
            string randomText = radomTextGenerator.Generate(AllowedChars, Length);
            Bitmap img = captchaGenerator.Generate(this.Session.SessionID, randomText, width, height);
            MemoryStream stream = new MemoryStream();
            img.Save(stream, ImageFormat.Jpeg);
            return new FileContentResult(stream.ToArray(), "image/jpeg");
        }

        public ActionResult Verify(string id)
        {
            bool verified = captchaGenerator.Verify(this.Session.SessionID, id);
            return Json(verified, JsonRequestBehavior.AllowGet);
        }
    }
}
