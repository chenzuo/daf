using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using DAF.Core;

namespace DAF.Report.Site
{
    public class ReportHelper
    {
        public const string ReportTemplatePath = "Templates";
        
        public static string GetPath(string client, string group = null, string report = null)
        {
            if (string.IsNullOrEmpty(client))
                client = "System";
            string path = string.Format("/{0}/{1}", ReportTemplatePath, client).GetPhysicalPath();
            if (!string.IsNullOrEmpty(group))
                path = Path.Combine(path, group);
            if (!string.IsNullOrEmpty(report))
                path = Path.Combine(path, report);

            return path;
        }
    }
}