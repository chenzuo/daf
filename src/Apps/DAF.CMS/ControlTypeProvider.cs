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
    public class ControlTypeProvider : IControlTypeProvider
    {
        private IFileSystemProvider fileProvider;
        private List<ControlType> controlTypes;

        public ControlTypeProvider(IFileSystemProvider fileProvider)
        {
            this.fileProvider = fileProvider;
            this.fileProvider.SetRootPath("~/".GetPhysicalPath());
        }

        public IEnumerable<ControlType> LoadControlTypes()
        {
            if (controlTypes == null || controlTypes.Count <= 0)
            {
                controlTypes = new List<ControlType>();
                var conFiles = fileProvider.GetFiles("Controls/*", "*.cshtml", false)
                    .Union(fileProvider.GetFiles("Controls", "*.cshtml", false));
                var moduleFiles = fileProvider.GetFiles("Modules/*/Controls/*", "*.cshtml", false)
                    .Union(fileProvider.GetFiles("Modules/*/Controls", "*.cshtml", false));

                conFiles.ForEach(f =>
                    {
                        ControlType tt = new ControlType();
                        tt.Name = f.FileNameWithoutExtension();
                        tt.Path = "~/" + fileProvider.MakeRelative(f.FullName);
                        tt.Category = f.Directory.Name.ToLower() == "controls" ? "" : f.Directory.Name;
                        tt.Parameters = GetParameters(f.FullName);

                        controlTypes.Add(tt);
                    });
                moduleFiles.ForEach(f =>
                {
                    ControlType tt = new ControlType();
                    tt.Name = f.FileNameWithoutExtension();
                    tt.Path = "~/" + fileProvider.MakeRelative(f.FullName);
                    tt.Category = f.Directory.Name.ToLower() == "controls" ? "" : f.Directory.Name;
                    tt.Module = f.Directory.Name.ToLower() == "controls" ? f.Directory.Parent.Name : f.Directory.Parent.Parent.Name;
                    tt.Parameters = GetParameters(f.FullName);

                    controlTypes.Add(tt);
                });

            }

            return controlTypes;
        }

        private IEnumerable<ControlParameter> GetParameters(string file)
        {
            var lines = File.ReadAllLines(file);
            bool started = false;
            StringBuilder sb = new StringBuilder();
            foreach (var line in lines)
            {
                if (started)
                {
                    if (line.Trim() == "/Parameters>-->")
                    {
                        started = false;
                        break;
                    }
                    sb.Append(line);
                }
                else
                {
                    if (line.Trim() == "<!--<Parameters>")
                    {
                        started = true;
                    }
                }
            }
            var json = sb.ToString();
            if (json.Length > 0)
            {
                return JsonHelper.Deserialize<ControlParameter[]>(json);
            }
            return Enumerable.Empty<ControlParameter>();
        }

        public ControlType GetControlType(string nameOrPath)
        {
            nameOrPath = nameOrPath.ToLower();
            var tts = LoadControlTypes();
            return tts.FirstOrDefault(o => o.Name.ToLower() == nameOrPath || o.Path == nameOrPath);
        }
    }
}
