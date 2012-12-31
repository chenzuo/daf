using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DAF.Core;
using DAF.Core.Serialization;

namespace DAF.Web
{
    public class WebJsonFileObjectProvider<T> : JsonFileObjectProvider<T>
    {
        public WebJsonFileObjectProvider(string jsonFile, IJsonSerializer jsonSerializer)
            : base(jsonFile.GetPhysicalPath(), jsonSerializer)
        {
        }
    }
}
