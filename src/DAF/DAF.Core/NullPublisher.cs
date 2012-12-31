using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public class NullPublisher : IPublisher
    {
        public void Publish<T>(T msg) where T : class
        {
        }
    }
}
