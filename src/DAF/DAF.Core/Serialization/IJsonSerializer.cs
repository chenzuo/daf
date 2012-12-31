using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }
}
