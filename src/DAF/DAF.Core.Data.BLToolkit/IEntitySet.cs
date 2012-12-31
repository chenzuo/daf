using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data.Linq;

namespace DAF.Core.Data.BLToolkit
{
    public interface IEntitySet
    {
        string ConnectionString { get; }
        Type[] EntityTypes { get; }
    }
}
