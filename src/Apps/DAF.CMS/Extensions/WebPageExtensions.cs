using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public static class WebPageExtensions
    {
        public static IEnumerable<SectionControl> OrderedControls(this WebPage pageData, string section)
        {
            var templateControls = pageData.Template.Controls.Where(o => o.Section == section)
                .Select(o => new SectionControl() { Path = o.ControlPath, Paras = o.ControlParas, Container = o.Container, Order = o.ShowOrder });
            var pageControls = pageData.Controls.Where(o => o.Section == section)
                .Select(o => new SectionControl() { Path = o.ControlPath, Paras = o.ControlParas, Container = o.Container, Order = o.ShowOrder });
            var controls = templateControls.Union(pageControls).OrderBy(o => o.Order);
            return controls;
        }
    }
}
