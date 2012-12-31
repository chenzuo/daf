using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Core.Generators;
using DAF.Web;
using DAF.Web.Menu;
using DAF.Core.Data;
using DAF.CMS.Site.Models;

namespace DAF.CMS.Site.Controllers
{
    public class MenuController : ApiController
    {
        private ITransactionManager trans;
        private IRepository<MenuGroup> repoMg;
        private IRepository<MenuItem> repoMi;

        public MenuController(ITransactionManager trans, IRepository<MenuGroup> repoMg, IRepository<MenuItem> repoMi)
        {
            this.trans = trans;
            this.repoMg = repoMg;
            this.repoMi = repoMi;
        }

        [HttpGet]
        public IEnumerable<MenuGroup> Groups()
        {
            var objs = repoMg.Query(null).OrderBy(o => o.Name).ToArray();
            return objs;
        }

        [HttpGet]
        public IEnumerable<MenuItem> Data(string group = null, string parentName = null)
        {
            var query = repoMi.Query(o => o.MenuGroupName == group);
            if (string.IsNullOrEmpty(parentName))
                query = query.Where(o => o.ParentName == null);
            else
                query = query.Where(o => o.ParentName == parentName);

            var objs = query.OrderBy(o => o.ShowOrder).ToArray();
            return objs;
        }

        [HttpGet]
        public IEnumerable<MenuItem> Data(string id)
        {
            var items = repoMi.Query(o => o.MenuGroupName == id && o.ParentName == null).OrderBy(o => o.ShowOrder).ToArray();
            HierarchyHelper.DoDescendants(items, m => m.Children, m => m.Children == null || m.Children.Count() <= 0,
                                        m => m.Children = repoMi.Query(o => o.ParentName == m.Name).ToArray());

            return items;
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<MenuItem> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoMi.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems))
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

        [HttpPost]
        public ServerResponse AddGroup(MenuGroup group)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoMg.Insert(group))
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

        [HttpPost]
        public ServerResponse EditGroup(MenuGroup group)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoMg.Update(group))
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
        public ServerResponse DeleteGroup(string id)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                var obj = repoMg.Query(o => o.Name == id).FirstOrDefault();
                if (obj != null)
                {
                    if (repoMg.Delete(obj))
                    {
                        response.Status = ResponseStatus.Success;
                        response.Message = LocaleHelper.Localizer.Get("DeleteSuccessfully");
                    }
                    else
                    {
                        response.Status = ResponseStatus.Failed;
                        response.Message = LocaleHelper.Localizer.Get("DeleteFailure");
                    }
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = string.Format("菜单组{0}不存在！", id);
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Exception;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}