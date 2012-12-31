using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace DAF.Web.Mvc.Results
{
    public class XmlResult : ActionResult
    {

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

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "text/xml";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    //要求缩进
                    settings.Indent = true;
                    //注意如果不设置encoding默认将输出utf-16
                    //注意这儿不能直接用Encoding.UTF8如果用Encoding.UTF8将在输出文本的最前面添加4个字节的非xml内容
                    settings.Encoding = new UTF8Encoding(false);

                    //设置换行符
                    settings.NewLineChars = Environment.NewLine;
                    Type objType = Data.GetType();

                    using (XmlWriter xmlWriter = XmlWriter.Create(ms, settings))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(objType);

                        serializer.WriteObject(xmlWriter, Data);
                    }

                    string xml = Encoding.UTF8.GetString(ms.ToArray());
                    response.Write(xml);
                }
            }
        }
    }
}
