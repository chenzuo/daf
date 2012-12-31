using System;
using System.Collections.Generic;
using System.Linq;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Web;
using DAF.CMS.Site.Models;
using DAF.SSO;

namespace DAF.CMS.Site.Logics
{
    public class ContentProvider : IContentProvider
    {
        private IIdGenerator idGenerator;
        private ITransactionManager trans;
        private IRepository<CategoryContent> repoContentCategory;
        private IRepository<ContentRelation> repoContentRelation;
        private IRepository<Content> repoContent;

        public ContentProvider(IIdGenerator idGenerator, ITransactionManager trans,
            IRepository<Content> repoContent,
            IRepository<CategoryContent> repoContentCategory,
            IRepository<ContentRelation> repoContentRelation)
        {
            this.idGenerator = idGenerator;
            this.trans = trans;
            this.repoContent = repoContent;
            this.repoContentCategory = repoContentCategory;
            this.repoContentRelation = repoContentRelation;
        }

        public IEnumerable<Content> GetContents(string site, string language, string category, ContentType? contentType = null
            , bool? published = null, ContentType[] contentTypes = null, DateTime? startDate = null, DateTime? endDate = null
            , int pi = 0, int ps = 0)
        {
            DateTime today = DateTime.Today;
            IQueryable<Content> query = null;
            if (string.IsNullOrEmpty(category))
            {
                var categoyContents = repoContentCategory.Query(o => o.SiteName == site && o.Language == language).Select(o => o.ContentId);
                query = repoContent.Query(o => o.SiteName == site && o.Language == language && !categoyContents.Contains(o.ContentId));
            }
            else
            {
                query = from o in repoContent.Query(null)
                        join c in repoContentCategory.Query(null) 
                            on new { o.SiteName, o.ContentId, o.Language } equals new { c.SiteName, c.ContentId, c.Language }
                        where o.SiteName == site && o.Language == language && c.CategoryCode == category
                        && (c.OnTime == null || c.OnTime >= today) && (c.OffTime == null || c.OffTime < today)
                        select o;
            }

            if (contentType.HasValue)
            {
                query = query.Where(o => o.ContentType == contentType.Value);
            }

            if (published.HasValue)
            {
                query = query.Where(o => o.Published == published.Value);
                if (published.Value)
                {
                    if (startDate.HasValue)
                        query = query.Where(o => o.PublishTime > startDate.Value);
                    if (endDate.HasValue)
                        query = query.Where(o => o.PublishTime < endDate.Value);
                }
                else
                {
                    if (startDate.HasValue)
                        query = query.Where(o => o.CreateTime > startDate.Value);
                    if (endDate.HasValue)
                        query = query.Where(o => o.CreateTime < endDate.Value);
                }
            }
            else
            {
                if (startDate.HasValue)
                    query = query.Where(o => o.CreateTime > startDate.Value);
                if (endDate.HasValue)
                    query = query.Where(o => o.CreateTime < endDate.Value);
            }

            if (contentTypes != null && contentTypes.Length > 0)
                query = query.Where(o => contentTypes.Contains(o.ContentType));

            query = query.OrderBy(o => o.ShowOrder).ThenByDescending(o => o.PublishTime).ThenByDescending(o => o.CreateTime);

            if (pi >= 0 && ps > 0)
            {
                query = query.Skip(pi * ps).Take(ps);
            }

            return query;
        }

        public Content Get(string site, string contentId, string language, bool withRelatedContents = true, bool withCategories = true)
        {
            Content obj = repoContent.Query(o => o.SiteName == site && o.ContentId == contentId && o.Language == language).FirstOrDefault();
            if (obj != null)
            {
                if (withCategories)
                {
                    obj.Categories = repoContentCategory.Query(o => o.SiteName == site && o.ContentId == contentId && o.Language == language).ToList();
                }
                if (withRelatedContents)
                {
                    obj.RelatedContents = repoContentRelation.Query(o => o.SiteName == site && o.ContentId == contentId && o.Language == language).ToList();

                    if (obj.RelatedContents != null && obj.RelatedContents.Count > 0)
                    {
                        foreach (var rc in obj.RelatedContents)
                        {
                            rc.RelatedContent = repoContent.Query(o => o.SiteName == site && o.ContentId == rc.RelatedContentId && o.Language == language).FirstOrDefault();
                        }
                    }
                }
            }
            return obj;
        }

        public Content Get(string site, ContentType contentType, string language, string title, string content, bool withRelatedContents = true, bool withCategories = true)
        {
            Content obj = null;
            var query = repoContent.Query(o => o.SiteName == site && o.Language == language && o.ContentType == contentType);
            if (!string.IsNullOrEmpty(title))
                query = query.Where(o => o.Title == title);
            if (!string.IsNullOrEmpty(content))
            {
                switch (contentType)
                {
                    case ContentType.Html:
                    case ContentType.Text:
                        query = query.Where(o => o.PlainBody.Contains(content));
                        break;
                    case ContentType.Audio:
                    case ContentType.File:
                    case ContentType.Image:
                    case ContentType.Link:
                    case ContentType.Video:
                        content = content.ToLower();
                        query = query.Where(o => o.ContentUrl.ToLower() == content);
                        break;
                    case ContentType.Org:
                    case ContentType.Person:
                    case ContentType.Contact:
                    default:
                        query = query.Where(o => o.PlainBody == content);
                        break;
                }
            }
            obj = query.FirstOrDefault();
            if (obj != null)
            {
                if (withCategories)
                {
                    obj.Categories = repoContentCategory.Query(o => o.SiteName == site && o.ContentId == obj.ContentId && o.Language == language).ToList();
                }
                if (withRelatedContents)
                {
                    obj.RelatedContents = repoContentRelation.Query(o => o.SiteName == site && o.ContentId == obj.ContentId && o.Language == language).ToList();

                    if (obj.RelatedContents != null && obj.RelatedContents.Count > 0)
                    {
                        foreach (var rc in obj.RelatedContents)
                        {
                            rc.RelatedContent = repoContent.Query(o => o.SiteName == site && o.ContentId == rc.RelatedContentId && o.Language == language).FirstOrDefault();
                        }
                    }
                }
            }
            return obj;
        }

        public Content New(string site, ContentType contentType, string language)
        {
            Content obj = new Content()
            {
                SiteName = site,
                ContentId = idGenerator.NewId(),
                Language = language,
                ContentType = contentType,
                CreateTime = DateTime.Now,
                PublishTime = DateTime.Now,
                Published = false
            };

            if (AuthHelper.IsAuthenticated)
            {
                obj.CreatorId = AuthHelper.CurrentSession.User.UserId;
                obj.CreatorName = AuthHelper.CurrentSession.User.Name();
            }

            return obj;
        }

        public bool Save(Content obj)
        {
            if (AuthHelper.IsAuthenticated)
            {
                obj.ModifierId = AuthHelper.CurrentSession.User.UserId;
                obj.ModifierName = AuthHelper.CurrentSession.User.Name();

                if (obj.Published)
                {
                    obj.PublisherId = AuthHelper.CurrentSession.User.UserId;
                    obj.PublisherName = AuthHelper.CurrentSession.User.Name();
                    if (obj.PublishTime == null)
                        obj.PublishTime = DateTime.Now;
                }
            }
            if (obj.ModifiedTime == null)
                obj.ModifiedTime = DateTime.Now;

            try
            {
                trans.BeginTransaction();
                if (repoContent.Save(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language, obj))
                {
                    if (obj.Published)
                    {
                        var dbObjs = repoContentCategory.Query(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language);
                        repoContentCategory.SaveAll(trans, dbObjs, obj.Categories,
                            (o, n) => o.SiteName == n.SiteName && o.ContentId == n.ContentId && o.Language == n.Language && o.CategoryCode == n.CategoryCode,
                            (r, o) =>
                            {
                                if (o.HotIndex > 0)
                                {
                                    o.HotIndex = repoContentCategory.Query(cate => cate.SiteName == o.SiteName && cate.CategoryCode == o.CategoryCode && cate.Language == o.Language).Select(cate => cate.HotIndex).Max();
                                    if (o.HotIndex == null || o.HotIndex <= 0)
                                        o.HotIndex = 1;
                                }
                                if (o.TopIndex > 0)
                                {
                                    o.TopIndex = repoContentCategory.Query(cate => cate.SiteName == o.SiteName && cate.CategoryCode == o.CategoryCode && cate.Language == o.Language).Select(cate => cate.TopIndex).Max();
                                    if (o.TopIndex == null || o.TopIndex <= 0)
                                        o.TopIndex = 1;
                                }
                                r.Insert(o);
                            },
                            (r, o, c) => { r.Update(c); },
                            (r, o) => { r.Delete(o); });
                    }
                    else
                    {
                        repoContentCategory.DeleteBatch(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language);
                    }
                    if (obj.RelatedContents != null && obj.RelatedContents.Count > 0)
                    {
                        obj.RelatedContents.ForEach(r =>
                        {
                            if (r.RelatedContent != null)
                            {
                                var rc = r.RelatedContent;
                                if (string.IsNullOrEmpty(rc.CreatorId))
                                {
                                    rc.CreatorId = obj.CreatorId;
                                }
                                if (string.IsNullOrEmpty(rc.CreatorName))
                                {
                                    rc.CreatorName = obj.CreatorName;
                                }
                                if (rc.CreateTime == null)
                                {
                                    rc.CreateTime = obj.CreateTime;
                                }
                                if (rc.ShowOrder <= 0)
                                {
                                    rc.ShowOrder = repoContent.Query(o => o.SiteName == obj.SiteName && o.ContentType == obj.ContentType && o.Language == obj.Language).Select(o => o.ShowOrder).Max();
                                }
                                repoContent.Save(o => o.SiteName == rc.SiteName && o.ContentId == rc.ContentId && o.Language == rc.Language, rc);
                            }
                        });

                        var dbObjs = repoContentRelation.Query(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language);

                        repoContentRelation.SaveAll(trans, dbObjs, obj.RelatedContents,
                            (o, n) => o.SiteName == n.SiteName && o.ContentId == n.ContentId && o.Language == n.Language && o.RelatedContentId == n.RelatedContentId,
                            true, true);
                    }
                    else
                    {
                        repoContentRelation.DeleteBatch(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language);
                    }
                }
                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }

        public bool Delete(string site, string contentId, string language, bool deleteRelatedContent = false)
        {
            var obj = repoContent.Query(o => o.SiteName == site && o.ContentId == contentId && o.Language == language).FirstOrDefault();
            if (obj != null)
            {
                try
                {
                    trans.BeginTransaction();
                    repoContentCategory.DeleteBatch(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language);
                    IEnumerable<string> relatedContentIds = null;
                    if (deleteRelatedContent)
                    {
                        relatedContentIds = repoContentRelation.Query(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language).Select(o => o.RelatedContentId).ToArray();
                    }
                    repoContentRelation.DeleteBatch(o => o.SiteName == obj.SiteName && o.ContentId == obj.ContentId && o.Language == obj.Language);
                    if (relatedContentIds != null && relatedContentIds.Count() > 0)
                    {
                        repoContent.DeleteBatch(o => o.SiteName == obj.SiteName && o.Language == obj.Language && relatedContentIds.Contains(o.ContentId));
                    }

                    repoContent.Delete(obj);

                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    return false;
                }
            }
            return false;
        }
    }
}