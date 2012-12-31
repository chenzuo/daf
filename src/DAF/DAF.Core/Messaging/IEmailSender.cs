using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Messaging
{
    public interface IEmailSender
    {
        void Send(string from, string[] to, string[] cc, string subject, string body, string[] attachments = null);
    }
}
