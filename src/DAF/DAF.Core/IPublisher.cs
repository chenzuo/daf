using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public interface IPublisher
    {
        void Publish<T>(T msg) where T : class;
    }
}
