using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Core.Serialization;

namespace DAF.Core
{
    public class JsonFileObjectProvider<T> : JsonObjectProvider<T>
    {
        public JsonFileObjectProvider(string jsonFile, IJsonSerializer jsonSerializer)
            : base(File.ReadAllText(jsonFile), jsonSerializer)
        {
        }
    }
}
