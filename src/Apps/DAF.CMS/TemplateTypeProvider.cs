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
    public class TemplateTypeProvider : ITemplateTypeProvider
    {
        private IFileSystemProvider fileProvider;
        private List<TemplateType> templateTypes;

        public TemplateTypeProvider(IFileSystemProvider fileProvider)
        {
            this.fileProvider = fileProvider;
            this.fileProvider.SetRootPath("~/PageTemplates".GetPhysicalPath());
        }

        public IEnumerable<TemplateType> LoadTemplateTypes()
        {
            if (templateTypes == null || templateTypes.Count <= 0)
            {
                templateTypes = new List<TemplateType>();
                fileProvider.GetFiles(null, "*.cshtml", false)
                    .ForEach(f =>
                        {
                            TemplateType tt = new TemplateType();
                            tt.Name = f.FileNameWithoutExtension();
                            tt.Path = f.Name;
                            tt.Sections = GetSections(f.FullName);

                            templateTypes.Add(tt);
                        });
            }

            return templateTypes;
        }

        private ICollection<PageSection> GetSections(string file)
        {
            List<PageSection> sections = new List<PageSection>();
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                string text = line.Trim();
                if (text.StartsWith("@RenderSection("))
                {
                    var infos = text.Replace("@RenderSection(", "").Replace("\"", "").Replace(")", "").Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    if (infos[0] == "head" || infos[0] == "end")
                        continue;
                    PageSection sec = new PageSection();
                    sec.Name = infos[0];
                    if (infos.Length > 1)
                    {
                        sec.Required = Convert.ToBoolean(infos[1]);
                    }
                    else
                    {
                        sec.Required = false;
                    }
                    sections.Add(sec);
                }
            }
            sections.Add(new PageSection()
            {
                Name = "body",
                Required = false
            });
            return sections;
        }

        public TemplateType GetTemplateType(string nameOrPath)
        {
            nameOrPath = nameOrPath.ToLower();
            var tts = LoadTemplateTypes();
            return tts.FirstOrDefault(o => o.Name.ToLower() == nameOrPath || o.Path == nameOrPath);
        }
    }
}
