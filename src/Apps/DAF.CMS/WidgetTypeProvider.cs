using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Core.FileSystem;
using DAF.Web;

namespace DAF.CMS
{
    public class WidgetTypeProvider : IWidgetTypeProvider
    {
        private IFileSystemProvider fileProvider;
        private List<WidgetType> controlTypes;

        public WidgetTypeProvider(IFileSystemProvider fileProvider)
        {
            this.fileProvider = fileProvider;
            this.fileProvider.SetRootPath("~/".GetPhysicalPath());
        }

        public IEnumerable<WidgetType> LoadWidgetTypes()
        {
            if (controlTypes == null || controlTypes.Count <= 0)
            {
                controlTypes = new List<WidgetType>();
                var conFiles = fileProvider.GetFiles("Widgets/*", "*.cshtml", false)
                    .Union(fileProvider.GetFiles("Widgets", "*.cshtml", false));
                var moduleFiles = fileProvider.GetFiles("Modules/*/Widgets/*", "*.cshtml", false)
                    .Union(fileProvider.GetFiles("Modules/*/Widgets", "*.cshtml", false));

                conFiles.ForEach(f =>
                    {
                        WidgetType tt = new WidgetType();
                        tt.Name = f.FileNameWithoutExtension();
                        tt.Path = "~/" + fileProvider.MakeRelative(f.FullName);
                        tt.Category = f.Directory.Name.ToLower() == "widgets" ? "" : f.Directory.Name;
                        tt.Parameters = GetParameters(f.FullName);

                        controlTypes.Add(tt);
                    });
                moduleFiles.ForEach(f =>
                {
                    WidgetType tt = new WidgetType();
                    tt.Name = f.FileNameWithoutExtension();
                    tt.Path = "~/" + fileProvider.MakeRelative(f.FullName);
                    tt.Category = f.Directory.Name.ToLower() == "widgets" ? "" : f.Directory.Name;
                    tt.Module = f.Directory.Name.ToLower() == "widgets" ? f.Directory.Parent.Name : f.Directory.Parent.Parent.Name;
                    tt.Parameters = GetParameters(f.FullName);

                    controlTypes.Add(tt);
                });

            }

            return controlTypes;
        }

        private IEnumerable<WidgetParameter> GetParameters(string file)
        {
            var lines = File.ReadAllLines(file);
            bool started = false;
            StringBuilder sb = new StringBuilder();
            foreach (var line in lines)
            {
                if (started)
                {
                    if (line.Trim() == "/Parameters>*@")
                    {
                        started = false;
                        break;
                    }
                    sb.Append(line);
                }
                else
                {
                    if (line.Trim() == "@*<Parameters>")
                    {
                        started = true;
                    }
                }
            }
            var json = sb.ToString();
            if (json.Length > 0)
            {
                return JsonHelper.Deserialize<WidgetParameter[]>(json);
            }
            return Enumerable.Empty<WidgetParameter>();
        }

        public WidgetType GetWidgetType(string nameOrPath)
        {
            nameOrPath = nameOrPath.ToLower();
            var tts = LoadWidgetTypes();
            return tts.FirstOrDefault(o => o.Name.ToLower() == nameOrPath || o.Path == nameOrPath);
        }
    }
}
