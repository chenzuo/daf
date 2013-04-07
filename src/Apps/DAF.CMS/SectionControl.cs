using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public class SectionControl
    {
        public string Path { get; set; }
        public string Paras { get; set; }
        public string Container { get; set; }
        public string Styles { get; set; }
        public string CacheKey { get; set; }
        public int CachedMunites { get; set; }
        public int Order { get; set; }
    }

    public static class SectionControlExtensions
    {
        public static SectionControl ToSectionControl(this WebPageControl con)
        {
            return new SectionControl()
            {
                Path = con.ControlPath,
                Paras = con.ControlParas,
                Container = con.Container,
                Styles = con.CssStyle,
                CacheKey = con.CacheKey,
                CachedMunites = con.Cached ? (con.CacheMunites.HasValue ? con.CacheMunites.Value : 60) : 0,
                Order = con.ShowOrder
            };
        }

        public static SectionControl ToSectionControl(this PageTemplateControl con)
        {
            return new SectionControl()
            {
                Path = con.ControlPath,
                Paras = con.ControlParas,
                Container = con.Container,
                Styles = con.CssStyle,
                CacheKey = con.CacheKey,
                CachedMunites = con.Cached ? (con.CacheMunites.HasValue ? con.CacheMunites.Value : 60) : 0,
                Order = con.ShowOrder
            };
        }
    }
}
