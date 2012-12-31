using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Localization
{
    public class LocalizationInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string Currency { get; set; }
        public double TimeZone { get; set; }
    }
}
