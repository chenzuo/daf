using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Logging;
using DAF.Core.Localization;

namespace DAF.Core
{
    public abstract class ProviderBaseWithLoggerNLocalizer : ProviderBaseWithLogger
    {
        public ProviderBaseWithLoggerNLocalizer()
            : base()
        {
            Localizer = NullLocalizer.Instance;
        }

        public ILocalizer Localizer { get; set; }
    }
}
