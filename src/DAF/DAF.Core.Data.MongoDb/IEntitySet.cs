using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core.Data.MongoDb
{
    public interface IEntitySet
    {
        string ConnectionString { get; }
        Type[] EntityTypes { get; }
    }
}
