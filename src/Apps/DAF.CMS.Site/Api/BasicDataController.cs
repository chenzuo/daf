using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Web;
using DAF.Core.Data;
using DAF.CMS.Site.Models;

namespace DAF.CMS.Site.Controllers
{
    public class BasicDataController : ApiController
    {
        private ITransactionManager trans;
        protected IRepository<BasicDataItem> repoBd;

        public BasicDataController(ITransactionManager trans, IRepository<BasicDataItem> repoBd)
        {
            this.trans = trans;
            this.repoBd = repoBd;
        }

        [HttpGet]
        public IEnumerable<string> Categories(string client)
        {
            var cates = repoBd.Query(a => a.SiteName == AuthHelper.CurrentClient.ClientId && a.ClientId == client).Select(a => a.Category).Distinct().ToArray();
            return cates;
        }

        [HttpGet]
        public IEnumerable<BasicDataItem> Data(string client, string cate = null, string parentName = null)
        {
            var query = repoBd.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId && o.ClientId == client && o.Category == cate);

            if (string.IsNullOrEmpty(parentName))
            {
                query = query.Where(o => o.ParentName == null);
            }
            else
            {
                query = query.Where(o => o.ParentName == parentName);
            }

            query = query.OrderBy(o => o.ShowOrder);

            return query.ToArray();
        }

        [HttpPost]
        public ServerResponse Save([FromBody]ChangedData<BasicDataItem> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoBd.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems))
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
    }
}