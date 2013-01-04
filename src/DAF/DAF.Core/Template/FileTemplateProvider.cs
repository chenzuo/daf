using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAF.Core.FileSystem;

namespace DAF.Core.Template
{
    public class FileTemplateProvider : ITemplateProvider
    {
        private IFileSystemProvider fileProvider;

        public FileTemplateProvider(string root, IFileSystemProvider fileProvider)
        {
            this.fileProvider = fileProvider;
            this.fileProvider.SetRootPath(root.GetPhysicalPath());
        }

        public TemplateContent GetTemplate(TemplateProperty templateProperty)
        {
            string file = string.Format("/{0}/{1}/{2}/{3}/{4}",
                templateProperty.ClientId, templateProperty.Language, templateProperty.Year,
                templateProperty.BizGroup, templateProperty.TemplateName);

            FileInfo fi  = fileProvider.GetFile(file);
            if (!fi.Exists)
            {
                file = string.Format("/{0}/{1}/{2}/{3}",
                templateProperty.Language, templateProperty.Year,
                templateProperty.BizGroup, templateProperty.TemplateName);
                fi = fileProvider.GetFile(file);
            }
            if (!fi.Exists)
            {
                file = string.Format("/{0}/{1}/{2}",
                templateProperty.Language,
                templateProperty.BizGroup, templateProperty.TemplateName);
                fi = fileProvider.GetFile(file);
            }
            if (!fi.Exists)
            {
                file = string.Format("/{0}/{1}/{2}",
                templateProperty.Year,
                templateProperty.BizGroup, templateProperty.TemplateName);
                fi = fileProvider.GetFile(file);
            }
            if (!fi.Exists)
            {
                file = string.Format("/{0}/{1}",
                templateProperty.BizGroup, templateProperty.TemplateName);
                fi = fileProvider.GetFile(file);
            }
            if (!fi.Exists)
            {
                file = string.Format("/{0}", templateProperty.TemplateName);
                fi = fileProvider.GetFile(file);
            }
            if (!fi.Exists)
                throw new FileNotFoundException(DAF.Core.Resources.Locale(o => o.FileNotFound), file);

            string body = File.ReadAllText(fi.FullName);

            TemplateContent content = new TemplateContent()
            {
                FilePath = file,
                Name = templateProperty.TemplateName,
                Body = body
            };
            return content;
        }
    }
}
