using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Template
{
    public interface ITemplateGenerator
    {
        bool SupportTemplate(TemplateProperty templateProperty);
        TemplateResult GenerateResult(object data, TemplateContent content);
    }
}
