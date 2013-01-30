using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.CMS
{
    public interface IWebControlProvider
    {
        IEnumerable<ControlType> LoadWebControls();
        ControlType GetWebControl(string nameOrPath);
    }
}
