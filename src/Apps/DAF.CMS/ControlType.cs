using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.CMS
{
    public class ControlType
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Category { get; set; }
        public IEnumerable<string> Parameters { get; set; }
    }

    public static class ControlTypeExtensions
    {
        public static string ParameterValues(this ControlType ct)
        {
            if (ct.Parameters == null || ct.Parameters.Count() <= 0)
                return string.Empty;
            var paras = ct.Parameters.Select(o => string.Format("{0}=", o));
            return string.Join(";", paras);
        }
    }
}
