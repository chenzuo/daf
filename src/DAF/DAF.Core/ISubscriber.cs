using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public interface ISubscriber<T> where T : class
    {
        void OnReceive(T msg);
    }
}
