using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DAF.Core.Configurations
{
    public interface IConfigurationProvider
    {
        void LoadConfigurations();
        NameValueCollection GetAppSettings();
        ConnectionStringSettingsCollection GetConnectionStrings();
    }
}
