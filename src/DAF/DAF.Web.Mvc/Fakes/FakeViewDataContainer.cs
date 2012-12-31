using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DAF.Web.Mvc.Fakes
{
    public class FakeViewDataContainer : IViewDataContainer
    {
        public FakeViewDataContainer(ViewDataDictionary viewData)
        {
            this.ViewData = viewData;
        }

        #region IViewDataContainer Members

        public ViewDataDictionary ViewData { get; set; }

        #endregion
    }
}
