using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.CMS
{
    public interface ITemplateTypeProvider
    {
        IEnumerable<TemplateType> LoadTemplateTypes();
        TemplateType GetTemplateType(string nameOrPath);
    }
}
