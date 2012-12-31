using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Data
{
    public class ObjectNotFoundException : ApplicationException
    {
        public ObjectNotFoundException(Type objType, string keyNames, string keyValues)
            : base(string.Format("{0} with key {1} values {2} not found!", objType.FullName, keyNames, keyValues))
        {
            ObjectType = objType;
            KeyNames = keyNames;
            KeyValues = keyValues;
        }

        public Type ObjectType { get; set; }
        public string KeyNames { get; set; }
        public string KeyValues { get; set; }
    }
}
