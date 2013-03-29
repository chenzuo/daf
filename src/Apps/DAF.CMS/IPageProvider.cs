using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Core.Data;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public interface IPageProvider
    {
        IEnumerable<WebPage> GetPages(string siteId, string parentId = null);
        WebPage GetPageById(string pageId);
        WebPage GetPageByName(string pageName);
        WebPage GetPageByShortUrl(string shortUrl);
        IEnumerable<WebPageControl> GetControls(string pageId);
        bool Save(ChangedData<WebPageControl> items);

        bool AddPage(WebPage page);
        bool UpdatePage(WebPage page);
        bool DeletePage(string pageId);
        bool SetAsHomePage(string pageId);
    }
}
