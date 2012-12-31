using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Template
{
    public interface ITemplateProvider
    {
        TemplateContent GetTemplate(TemplateProperty templateProperty);
    }
}
