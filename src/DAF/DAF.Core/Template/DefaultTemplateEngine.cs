using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Template
{
    public class DefaultTemplateEngine : ITemplateEngine
    {
        private ITemplateProvider fileProvider;
        private IEnumerable<ITemplateGenerator> generators;

        public DefaultTemplateEngine(ITemplateProvider fileProvider, IEnumerable<ITemplateGenerator> generators)
        {
            this.fileProvider = fileProvider;
            this.generators = generators;
        }

        public TemplateResult LoadTemplate(object data, TemplateProperty templateProperty)
        {
            TemplateContent content = fileProvider.GetTemplate(templateProperty);
            if (content == null)
                return null;

            TemplateResult result = null;
            foreach (var g in generators)
            {
                result = g.GenerateResult(data, content);
                if (result != null)
                    break;
            }

            return result;
        }
    }
}
