using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Messaging
{
    public interface IMessageHandler<T> where T : class
    {
        void Handle(T msg);
    }
}
