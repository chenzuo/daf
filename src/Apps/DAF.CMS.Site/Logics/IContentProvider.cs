using System;
using System.Collections.Generic;
using System.Linq;
using DAF.Core;
using DAF.Core.Data;
using DAF.Web;
using DAF.CMS.Site.Models;

namespace DAF.CMS.Site.Logics
{
    public interface IContentProvider
    {
        IEnumerable<Content> GetContents(string site, string language, string category, ContentType? contentType = null
           , bool? published = null, ContentType[] contentTypes = null, DateTime? startDate = null, DateTime? endDate = null
           , int pi = 0, int ps = 0);

        Content Get(string site, string contentId, string language, bool withRelatedContents = true, bool withCategories = true);
        Content Get(string site, ContentType contentType, string language, string title, string content, bool withRelatedContents = true, bool withCategories = true);
        Content New(string site, ContentType contentType, string language);
        bool Save(Content obj);
        bool Delete(string site, string contentId, string language, bool deleteRelatedContent = false);
    }
}