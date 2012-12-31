using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DAF.Core.Collections
{
    [Serializable]
    public class SerializableDictionary : Dictionary<string, object>, ISerializable
    {
        public SerializableDictionary()
        {
        }

        protected SerializableDictionary(SerializationInfo info, StreamingContext context)
        {
            foreach (var entry in info)
            {
                this.Add(entry.Name, entry.Value);
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (string key in this.Keys)
            {
                info.AddValue(key, this[key], this[key] == null ? typeof(object) : this[key].GetType());
            }
        }
    }
}
