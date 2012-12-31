using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Web;
using DAF.Report.Site.ViewModels;

namespace DAF.Report.Site.Api
{
    public class ReportController : ApiController
    {
        public ReportController()
        {
        }

        [HttpGet]
        public dynamic Reports(string client)
        {
            string path = ReportHelper.GetPath(client);
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
            var rpts = dir.GetDirectories().OrderBy(d => d.CreationTime).Select(gd => new
            {
                Group = gd.Name,
                Reports = gd.GetDirectories().OrderBy(d => d.CreationTime).Select(rd => new
                {
                    Report = rd.Name,
                    Pages = rd.GetFiles().OrderBy(f => f.Name).Select(f => new
                    {
                        Page = f.FileNameWithoutExtension(),
                        File = f.FullName
                    }).ToArray()
                }).ToArray()
            }).ToArray();

            return rpts;
        }

        [HttpGet]
        public ServerResponse Create(string client, string group, string report)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var path = ReportHelper.GetPath(client, group, report);
                var dir = new DirectoryInfo(path);
                if (!dir.Exists)
                    dir.Create();

                result.Status = ResponseStatus.Success;
                result.Message = "成功创建报表";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ServerResponse RenameGroup(string client, string group, string newName)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var path = ReportHelper.GetPath(client, group);
                var dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    dir.MoveTo(ReportHelper.GetPath(client, newName));
                }
                result.Status = ResponseStatus.Success;
                result.Message = "成功重命名报表分组";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ServerResponse DeleteGroup(string client, string group)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var path = ReportHelper.GetPath(client, group);
                var dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    dir.Delete(true);
                }
                result.Status = ResponseStatus.Success;
                result.Message = "成功删除报表分组";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ServerResponse RenameReport(string client, string group, string report, string newName)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var path = ReportHelper.GetPath(client, group, report);
                var dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    dir.MoveTo(ReportHelper.GetPath(client, group, newName));
                }
                result.Status = ResponseStatus.Success;
                result.Message = "成功重命名报表";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ServerResponse DeleteReport(string client, string group, string report)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var path = ReportHelper.GetPath(client, group, report);
                var dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    dir.Delete(true);
                }
                result.Status = ResponseStatus.Success;
                result.Message = "成功删除报表";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ServerResponse RenamePage(string path, string newName)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.MoveTo(Path.Combine(fi.DirectoryName, string.Format("{0}.cshtml", newName)));
                }
                result.Status = ResponseStatus.Success;
                result.Message = "成功重命名报表页面";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ServerResponse DeletePage(string path)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
                result.Status = ResponseStatus.Success;
                result.Message = "成功删除页面";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ServerResponse AddPage(string client, string group, string report, string page)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                var path = Path.Combine(ReportHelper.GetPath(client, group, report), string.Format("{0}.cshtml", page));
                FileInfo fi = new FileInfo(path);
                if (!fi.Exists)
                {
                    var fs = fi.Create();
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                result.Status = ResponseStatus.Success;
                result.Message = "成功添加报表页面";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public ReportPageContent GetPage(string path)
        {
            FileInfo f = new FileInfo(path);
            if (!f.Exists)
                f.Create();

            var content = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
            ReportPageContent page = new ReportPageContent();
            StringReader sr = new StringReader(content);
            string line = string.Empty;
            int contentType = 0; // 0: none, 1: css, 2: html, 3: js
            StringBuilder sbCss = new StringBuilder();
            StringBuilder sbHtml = new StringBuilder();
            StringBuilder sbJs = new StringBuilder();
            line = sr.ReadLine();
            while (line != null)
            {
                switch (contentType)
                {
                    case 1:
                        if (line == "</style>")
                            contentType = 0;
                        else
                            sbCss.AppendLine(line);
                        break;
                    case 2:
                        if (line == "<!-- end html content -->")
                            contentType = 0;
                        else
                            sbHtml.AppendLine(line);
                        break;
                    case 3:
                        if (line == "</script>")
                            contentType = 0;
                        else
                            sbJs.AppendLine(line);
                        break;
                    case 0:
                        if (line == "<style type=\"text/css\">")
                        {
                            contentType = 1;
                        }
                        else if (line == "<!-- start html content -->")
                        {
                            contentType = 2;
                        }
                        else if (line == "<script type=\"text/javascript\">")
                        {
                            contentType = 3;
                        }
                        break;
                }
                line = sr.ReadLine();
            }
            page.Css = sbCss.ToString();
            page.Html = sbHtml.ToString();
            page.Js = sbJs.ToString();
            return page;
        }

        [HttpPost]
        public ServerResponse SavePage([FromBody]ReportPageContent page)
        {
            ServerResponse result = new ServerResponse();
            try
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(page.Css))
                {
                    sb.AppendLine("<style type=\"text/css\">");
                    sb.AppendLine(page.Css);
                    sb.AppendLine("</style>");
                }
                sb.AppendLine("<!-- start html content -->");
                sb.AppendLine(page.Html);
                sb.AppendLine("<!-- end html content -->");
                if (!string.IsNullOrEmpty(page.Js))
                {
                    sb.AppendLine("<script type=\"text/javascript\">");
                    sb.AppendLine(page.Js);
                    sb.AppendLine("</script>");
                }
                File.WriteAllText(page.File, sb.ToString(), System.Text.Encoding.UTF8);

                result.Status = ResponseStatus.Success;
                result.Message = "成功保存页面";
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatus.Exception;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}