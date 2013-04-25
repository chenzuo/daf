using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Core.Security;
using DAF.Web.Mvc;
using DAF.Web.Mvc.Results;

namespace DAF.Web
{
    public static class ControllerExtensions
    {
        public static HttpStatusCodeResult Message(this Controller controller, string message, int statusCode = 200)
        {
            return new HttpStatusCodeResult(statusCode, message);
        }

        public static HttpStatusCodeResult Success(this Controller controller, string message)
        {
            return new HttpStatusCodeResult(200, message);
        }

        public static HttpStatusCodeResult Failure(this Controller controller, string message)
        {
            return new HttpStatusCodeResult(500, message);
        }

        public static ActionResult Data(this Controller controller, object data, string contentType = null, Encoding encoding = null, IEncryptionProvider encryptor = null)
        {
            string type = controller.Request.QueryString["type"];
            if (string.IsNullOrEmpty(type))
            {
                type = "json";
            }
            switch (type.ToLower())
            {
                case "json":
                default:
                    //return new JsonResult() { Data = data, ContentType = contentType, ContentEncoding = encoding, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    var jsonSerializer = IocInstance.Container.Resolve<Core.Serialization.IJsonSerializer>();
                    return new DAF.Web.Mvc.Results.JsonResult(jsonSerializer, encryptor) { Data = data, ContentType = contentType, ContentEncoding = encoding };
                case "xml":
                    return new XmlResult() { Data = data, ContentType = contentType, ContentEncoding = encoding };
            }
        }

        public static RefreshResult Refresh(this Controller controller)
        {
            return new RefreshResult();
        }

        public static GoBackResult GoBack(this Controller controller)
        {
            return new GoBackResult();
        }
    }
}
