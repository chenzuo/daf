using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public class SitePage
    {
        public WebPage Page { get; set; }
        public LinkedList<PageTemplate> Templates { get; set; }
    }
}
