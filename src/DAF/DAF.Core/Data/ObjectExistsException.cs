using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Data
{
    public class ObjectExistsException : ApplicationException
    {
        public ObjectExistsException(Type objType, string keyNames, string keyValues)
            : base(string.Format("{0} with key {1} values {2} already exists!", objType.FullName, keyNames, keyValues))
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
