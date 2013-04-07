using System;
using System.Collections.Generic;
using System.Linq;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Web;
using DAF.CMS.Models;
using DAF.SSO;

namespace DAF.CMS
{
    public class ContentProvider : IContentProvider
    {
        private IIdGenerator idGenerator;
        private ITransactionManager trans;
        private IRepository<Category> repoCategory;
        private IRepository<CategoryContent> repoContentCategory;
        private IRepository<ContentRelation> repoContentRelation;
        private IRepository<Content> repoContent;

        public ContentProvider(IIdGenerator idGenerator, ITransactionManager trans,
            IRepository<Category> repoCategory,
            IRepository<Content> repoContent,
            IRepository<CategoryContent> repoContentCategory,
            IRepository<ContentRelation> repoContentRelation)
        {
            this.idGenerator = idGenerator;
            this.trans = trans;
            this.repoCategory = repoCategory;
            this.repoContent = repoContent;
            this.repoContentCategory = repoContentCategory;
            this.repoContentRelation = repoContentRelation;
        }

        public IEnumerable<Content> GetContents(string siteId, string category, out int total
            , bool? published = null, ContentType[] contentTypes = null, DateTime? startDate = null, DateTime? endDate = null
            , int pi = 0, int ps = 0)
        {
            DateTime today = DateTime.Today;
            IQueryable<Content> query = null;
            if (string.IsNullOrEmpty(category))
            {
                query = from o in repoContent.Query(null)
                        join cc in repoContentCategory.Query(null)
                            on o.ContentId equals cc.ContentId
                        join c in repoCategory.Query(null)
                            on cc.CategoryId equals c.CategoryId
                        where o.SiteId == siteId && o.CreateAsRelated == false && c.Code == null
                        select o;
            }
            else
            {
                query = from o in repoContent.Query(null)
                        join cc in repoContentCategory.Query(null)
                            on o.ContentId equals cc.ContentId
                        join c in repoCategory.Query(null)
                            on cc.CategoryId equals c.CategoryId
                        where o.SiteId == siteId && o.CreateAsRelated == false && c.Code == category
                        select o;
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

            total = query.Count();
            if (pi >= 0 && ps > 0)
            {
                query = query.Skip(pi * ps).Take(ps);
            }

            return query;
        }

        public Content Get(string siteId, string contentIdOrShortUrl, bool withRelatedContents = true, bool withCategories = true)
        {
            contentIdOrShortUrl = contentIdOrShortUrl.ToLower();
            Content obj = repoContent.Query(o => o.SiteId == siteId && (o.ContentId == contentIdOrShortUrl || o.ShortUrl == contentIdOrShortUrl)).FirstOrDefault();
            if (obj != null)
            {
                if (withCategories)
                {
                    obj.Categories = repoContentCategory.Query(o => o.SiteId == siteId && o.ContentId == obj.ContentId).ToList();
                }
                if (withRelatedContents)
                {
                    obj.RelatedContents = repoContentRelation.Query(o => o.SiteId == siteId && o.ContentId == obj.ContentId).ToList();

                    if (obj.RelatedContents != null && obj.RelatedContents.Count > 0)
                    {
                        foreach (var rc in obj.RelatedContents)
                        {
                            rc.RelatedContent = repoContent.Query(o => o.SiteId == rc.SiteId && o.ContentId == rc.RelatedContentId).FirstOrDefault();
                        }
                    }
                }
            }
            return obj;
        }

        public Content Get(string siteId, ContentType contentType, string title, string content, bool withRelatedContents = true, bool withCategories = true)
        {
            Content obj = null;
            var query = repoContent.Query(o => o.SiteId == siteId && o.ContentType == contentType);
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
                    obj.Categories = repoContentCategory.Query(o => o.SiteId == siteId && o.ContentId == obj.ContentId).ToList();
                }
                if (withRelatedContents)
                {
                    obj.RelatedContents = repoContentRelation.Query(o => o.SiteId == siteId && o.ContentId == obj.ContentId).ToList();

                    if (obj.RelatedContents != null && obj.RelatedContents.Count > 0)
                    {
                        foreach (var rc in obj.RelatedContents)
                        {
                            rc.RelatedContent = repoContent.Query(o => o.SiteId == rc.SiteId && o.ContentId == rc.RelatedContentId).FirstOrDefault();
                        }
                    }
                }
            }
            return obj;
        }

        public Content New(string siteId, ContentType contentType)
        {
            Content obj = new Content()
            {
                ContentId = idGenerator.NewId(),
                SiteId = siteId,
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
                if (repoContent.Save(o => o.SiteId == obj.SiteId && o.ContentId == obj.ContentId, obj))
                {
                    if (obj.Published)
                    {
                        var dbObjs = repoContentCategory.Query(o => o.SiteId == obj.SiteId && o.ContentId == obj.ContentId).ToArray();
                        repoContentCategory.SaveAll(trans, dbObjs, obj.Categories,
                            (o, n) => o.SiteId == n.SiteId && o.ContentId == n.ContentId && o.CategoryId == n.CategoryId,
                            (r, o) =>
                            {
                                if (o.HotIndex > 0)
                                {
                                    o.HotIndex = repoContentCategory.Query(cate => cate.SiteId == o.SiteId && cate.CategoryId == o.CategoryId).Select(cate => cate.HotIndex).Max();
                                    if (o.HotIndex == null || o.HotIndex <= 0)
                                        o.HotIndex = 1;
                                }
                                if (o.TopIndex > 0)
                                {
                                    o.TopIndex = repoContentCategory.Query(cate => cate.SiteId == o.SiteId && cate.CategoryId == o.CategoryId).Select(cate => cate.TopIndex).Max();
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
                        repoContentCategory.DeleteBatch(o => o.SiteId == obj.SiteId && o.ContentId == obj.ContentId);
                    }
                    if (obj.RelatedContents != null && obj.RelatedContents.Count > 0)
                    {
                        obj.RelatedContents.ForEach(r =>
                        {
                            if (r.RelatedContent != null)
                            {
                                var rc = r.RelatedContent;
                                rc.SiteId = obj.SiteId;
                                rc.CreateAsRelated = true;
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
                                    rc.ShowOrder = repoContent.Query(o => o.SiteId == obj.SiteId && o.ContentType == obj.ContentType).Select(o => o.ShowOrder).Max();
                                }
                                repoContent.Save(o => o.SiteId == rc.SiteId && o.ContentId == rc.ContentId, rc);
                            }
                        });

                        var dbObjs = repoContentRelation.Query(o => o.SiteId == obj.SiteId && o.ContentId == obj.ContentId).ToArray();

                        repoContentRelation.SaveAll(trans, dbObjs, obj.RelatedContents,
                            (o, n) => o.SiteId == n.SiteId && o.ContentId == n.ContentId && o.RelatedContentId == n.RelatedContentId,
                            true, true);
                    }
                    else
                    {
                        repoContentRelation.DeleteBatch(o => o.SiteId == obj.SiteId && o.ContentId == obj.ContentId);
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

        public bool Delete(string siteId, string contentId, bool deleteRelatedContent = false)
        {
            var obj = repoContent.Query(o => o.SiteId == siteId && o.ContentId == contentId).FirstOrDefault();
            if (obj != null)
            {
                try
                {
                    trans.BeginTransaction();
                    repoContentCategory.DeleteBatch(o => o.SiteId == obj.SiteId && o.ContentId == obj.ContentId);
                    IEnumerable<string> relatedContentIds = null;
                    if (deleteRelatedContent)
                    {
                        relatedContentIds = repoContentRelation.Query(o => o.SiteId == siteId && o.ContentId == obj.ContentId).Select(o => o.RelatedContentId).ToArray();
                    }
                    if (relatedContentIds != null && relatedContentIds.Count() > 0)
                    {
                        repoContent.DeleteBatch(o => o.SiteId == siteId && relatedContentIds.Contains(o.ContentId));
                    }
                    repoContentRelation.DeleteBatch(o => o.SiteId == siteId && o.ContentId == obj.ContentId);

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