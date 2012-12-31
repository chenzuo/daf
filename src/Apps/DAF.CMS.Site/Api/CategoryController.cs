using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Core.Collections;
using DAF.Web;
using DAF.Core.Data;
using DAF.CMS.Site.Models;

namespace DAF.CMS.Site.Controllers
{
    public class CategoryController : ApiController
    {
        private ITransactionManager trans;
        protected IRepository<Category> repoC;

        public CategoryController(ITransactionManager trans, IRepository<Category> repoC)
        {
            this.trans = trans;
            this.repoC = repoC;
        }

        [HttpGet]
        public IEnumerable<Category> Data(string language, string parentCode = null, string code = null, bool createIfNotExists = true)
        {
            if (!string.IsNullOrEmpty(code) && createIfNotExists)
            {
                if (repoC.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId && o.Language == language && o.Code == code).Any() == false)
                {
                    var count = repoC.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId && o.Language == language).Count();
                    Category cate = new Category()
                    {
                        SiteName = AuthHelper.CurrentClient.ClientId,
                        Language = language,
                        Code = code,
                        Name = code,
                        ShowOrder = count,
                        Status = DataStatus.Normal,
                        ParentCode = null
                    };
                    repoC.Insert(cate);
                }
            }

            var query = repoC.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId && o.Language == language);

            if (string.IsNullOrEmpty(parentCode))
            {
                query = query.Where(o => o.ParentCode == null);

                if (!string.IsNullOrEmpty(code))
                {
                    query = query.Where(o => o.Code == code);
                }
            }
            else
            {
                query = query.Where(o => o.ParentCode == parentCode);
            }

            query = query.OrderBy(o => o.ShowOrder);

            return query.ToArray();
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<Category> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoC.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems))
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = LocaleHelper.Localizer.Get("SaveSuccessfully");
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = LocaleHelper.Localizer.Get("SaveFailure");
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet]
        //[OutputCache(CacheProfile = "common")]
        public IEnumerable<TreeNode> Tree(string language, string code)
        {
            var query = repoC.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId && o.Language == language && o.ParentCode == code).ToArray();

            var tree = HierarchyHelper.Build(query, o => o.Code, o => o.Name,
                o => repoC.Query(c => c.SiteName == AuthHelper.CurrentClient.ClientId && c.Language == language && c.ParentCode == o.Code).ToArray());

            return tree;
        }
    }
}