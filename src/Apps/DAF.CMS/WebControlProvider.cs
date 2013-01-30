using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.CMS
{
    public class WebControlProvider : IWebControlProvider
    {
        public IEnumerable<ControlType> LoadWebControls()
        {
            throw new NotImplementedException();
        }

        public ControlType GetWebControl(string nameOrPath)
        {
            throw new NotImplementedException();
        }
    }
}
