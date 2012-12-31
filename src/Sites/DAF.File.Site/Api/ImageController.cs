using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Drawing;
using DAF.Core;
using DAF.Web;
using DAF.Core.FileSystem;
using DAF.File.Site.Models;

namespace DAF.File.Site.Api
{
    public class ImageController : ApiController
    {
        private IFileSystemProvider fileProvider;
        private string rootPath;

        public ImageController(IFileSystemProvider fileProvider)
        {
            this.fileProvider = fileProvider;
            rootPath = ConfigurationManager.AppSettings["UploadPath"] ?? "/Uploads/";
            if (!rootPath.StartsWith("/"))
                rootPath = "/" + rootPath;
            if (!rootPath.EndsWith("/"))
                rootPath += "/";
            this.fileProvider.SetRootPath(("~" + rootPath).GetPhysicalPath());
        }

        [HttpGet]
        public string Save(string file, float x, float y, float width, float height, int? toWidth = null, int? toHeight = null)
        {
            if (file.IndexOf('?') >= 0)
            {
                file = file.Substring(0, file.IndexOf('?'));
            }
            string path = file.GetPhysicalPath();

            if (x > 0 && y > 0 && width > 0 && height > 0)
            {
                Image img = Image.FromFile(path);
                RectangleF rect = new RectangleF(x, y, width, height);
                var cropImg = ImageHelper.Crop(img, rect);

                if (toWidth.HasValue && toHeight.HasValue)
                {
                    cropImg = ImageHelper.Resize(cropImg, new Size(toWidth.Value, toHeight.Value));
                }

                cropImg.Save(path);
            }

            return file;
        }
    }
}