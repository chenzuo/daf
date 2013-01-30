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
            this.fileProvider.SetRootPath("~/Controls".GetPhysicalPath());
        }

        public IEnumerable<ControlType> LoadControlTypes()
        {
            if (controlTypes == null || controlTypes.Count <= 0)
            {
                controlTypes = new List<ControlType>();
                fileProvider.GetFiles(null, "*.cshtml", true)
                    .ForEach(f =>
                    {
                        string category = fileProvider.MakeRelative(f.DirectoryName);
                        ControlType tt = new ControlType();
                        tt.Name = f.FileNameWithoutExtension();
                        tt.Path = string.IsNullOrEmpty(category) ? f.Name : string.Join("/", category, f.Name).ToLower();
                        tt.Category = category;
                        tt.Parameters = GetParameters(f.FullName);

                        controlTypes.Add(tt);
                    });
            }

            return controlTypes;
        }

        private IEnumerable<string> GetParameters(string file)
        {
            return Enumerable.Empty<string>();
        }

        public ControlType GetControlType(string nameOrPath)
        {
            nameOrPath = nameOrPath.ToLower();
            var tts = LoadControlTypes();
            return tts.FirstOrDefault(o => o.Name.ToLower() == nameOrPath || o.Path == nameOrPath);
        }
    }
}
