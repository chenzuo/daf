using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Messaging
{
    public interface IMessageSender
    {
        void Send(params object[] msgs);
    }
}
