using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.SSO
{
    public class SSOClient
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string BaseUrl { get; set; }
        public string EncryptKey { get; set; }
        public string EncryptScrect { get; set; }
    }
}
