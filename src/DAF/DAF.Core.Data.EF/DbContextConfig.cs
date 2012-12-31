using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.Core.Data.EF
{
    public class DbContextConfig
    {
        private static Dictionary<Type, Type> entityTypeToDbContextType = new Dictionary<Type, Type>();
        public static Dictionary<Type, Type> EentityTypeToDbContextType
        {
            get { return entityTypeToDbContextType; }
        }
    }
}
