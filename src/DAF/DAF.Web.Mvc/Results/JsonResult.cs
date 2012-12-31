using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using DAF.Core.Security;
using DAF.Core.Serialization;

namespace DAF.Web.Mvc.Results
{
    public class JsonResult : ActionResult
    {
        private IJsonSerializer jsonSerializer;
        private IEncryptionProvider encryptor;

        public JsonResult(IJsonSerializer jsonSerializer, IEncryptionProvider encryptor)
        {
            this.jsonSerializer = jsonSerializer;
            this.encryptor = encryptor;
        }

        public Encoding ContentEncoding
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public object Data
        {
            get;
            set;
        }

        public string JsonCallback { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            this.JsonCallback = context.HttpContext.Request["callback"];

            if (string.IsNullOrEmpty(this.JsonCallback))
                this.JsonCallback = context.HttpContext.Request["jsoncallback"];

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                string json = jsonSerializer.Serialize(Data);
                if (!string.IsNullOrEmpty(JsonCallback))
                {
                    json = string.Format("{0}({1});", this.JsonCallback, json);
                }
                if (encryptor != null)
                {
                    json = encryptor.Encrypt(json);
                }
                response.Write(json);
            }
        }
    }
}
