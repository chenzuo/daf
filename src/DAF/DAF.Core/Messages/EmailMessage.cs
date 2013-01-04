using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Messages
{
    [Serializable]
    public class EmailMessage
    {
        public string From { get; set; }
        public string[] To { get; set; }
        public string[] CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] Attachments { get; set; }
    }
}
