using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.CMS
{
    public interface IControlTypeProvider
    {
        IEnumerable<ControlType> LoadControlTypes();
        ControlType GetControlType(string nameOrPath);
    }
}
