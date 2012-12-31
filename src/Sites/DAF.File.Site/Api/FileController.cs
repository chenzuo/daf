using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using DAF.Core;
using DAF.Web;
using DAF.Core.FileSystem;
using DAF.File.Site.Models;

namespace DAF.File.Site.Api
{
    public class FileController : ApiController
    {
        private IFileSystemProvider fileProvider;
        private string rootPath;

        public FileController(IFileSystemProvider fileProvider)
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
        public dynamic Files(string path, int orderBy = 0, int pi = 0, int ps = 50)
        {
            var files = fileProvider.GetFiles(path, "*.*");

            switch (orderBy)
            {
                case 1:
                    files = files.OrderBy(o => o.Name);
                    break;
                case 0:
                default:
                    files = files.OrderByDescending(o => o.CreationTime);
                    break;
            }

            if (pi >= 0 && ps > 0)
            {
                files = files.Skip(pi * ps).Take(ps);
            }
            var vfs = files.Where(f => !f.Name.StartsWith("_")).Select(f => new
            {
                name = f.Name,
                url = UrlHelper.ClientUrl(rootPath.UriCombine(fileProvider.MakeRelative(f.FullName))),
                size = FileType.GetFriendlyFileSize((int)f.Length),
                creationTime = f.CreationTime.ToString("yyyy-MM-dd")
            }).ToArray();

            return vfs;
        }

        [HttpGet]
        public ServerResponse Do(string file, string op, string args)
        {
            ServerResponse response = new ServerResponse();
            var f = fileProvider.GetFile(file);
            if (f != null)
            {
                try
                {
                    switch (op.ToLower())
                    {
                        case "delete":
                            response.Status = ResponseStatus.Success;
                            response.Message = LocaleHelper.Localizer.Get("DeleteSuccessfully");
                            break;
                        case "rename":
                            break;
                    }
                }
                catch (Exception ex)
                {
                    response.Status = ResponseStatus.Exception;
                    response.Message = ex.Message;
                }
            }
            return response;
        }
    }
}