using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAF.CMS.Site.Models
{
    public enum ContentType
    {
        Html = 0,
        Image = 1,
        File = 2,
        Link = 3,
        Text = 4,
        Audio = 5,
        Video = 6,

        Org = 10,
        Person = 11,
        Contact = 12,
    }
}