using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.WebPages;
using System.Web.WebPages.Razor;
using RT = RazorTemplates.Core;
using DAF.Core;
using DAF.Core.Template;

namespace DAF.Core.Template.RazorTemplates
{
    public class RazorContentTemplateGenerator : ITemplateGenerator
    {
        public TemplateResult GenerateResult(object data, TemplateContent content)
        {
            var template = RT.Template
                .WithBaseType<RT.TemplateBase>()
                .AddNamespace("DAF.Core")
                .AddNamespace("DAF.Web")
                .Compile(content.Body);

            string body = template.Render(data);
            return new TemplateResult()
            {
                Name = content.Name,
                Body = body,
                OutFilePath = null
            };
        }
    }
}
