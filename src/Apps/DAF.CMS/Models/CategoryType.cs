using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAF.CMS.Models
{
    [Flags]
    public enum CategoryType
    {
        Virtual = 1,
        Column = 2,
        Menu = 4,
    }
}
