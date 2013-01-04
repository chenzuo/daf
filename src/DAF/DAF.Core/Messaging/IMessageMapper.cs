using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Messaging
{
    public abstract class MessageMapper<TF, TT> : IMessageMapper<TF>
    {
        public Type MessageType
        {
            get { return typeof(TT); }
        }

        protected abstract TT MapTo(TF obj);

        public object Map(TF obj)
        {
            return MapTo(obj);
        }
    }

    public interface IMessageMapper<TF>
    {
        Type MessageType { get; }
        object Map(TF obj);
    }
}
