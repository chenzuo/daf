using System;
using System.Collections.Generic;
using System.Linq;
using DAF.Core;
using DAF.Core.Data;
using DAF.Web;
using DAF.CMS.Models;

namespace DAF.CMS
{
    public interface IContentProvider
    {
        IEnumerable<Content> GetContents(string siteId, string category, out int total
           , bool includeSubCategories = false
           , bool? published = null, ContentType[] contentTypes = null, DateTime? startDate = null, DateTime? endDate = null
           , int pi = 0, int ps = 0);

        Content Get(string siteId, string contentIdOrShortUrl, bool withRelatedContents = true, bool withCategories = true);
        Content Get(string siteId, ContentType contentType, string title, string content, bool withRelatedContents = true, bool withCategories = true);
        Content New(string siteId, ContentType contentType);
        bool Save(Content obj);
        bool Delete(string siteId, string contentId, bool deleteRelatedContent = false);
    }
}