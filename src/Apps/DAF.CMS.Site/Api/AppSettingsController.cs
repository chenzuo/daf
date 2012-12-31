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
    public class AppSettingsController : ApiController
    {
        private ITransactionManager trans;
        private IRepository<AppSetting> repoAs;

        public AppSettingsController(ITransactionManager trans, IRepository<AppSetting> repoAs)
        {
            this.trans = trans;
            this.repoAs = repoAs;
        }

        [HttpGet]
        public IEnumerable<string> Categories(string client)
        {
            var cates = repoAs.Query(a => a.SiteName == AuthHelper.CurrentClient.ClientId && a.ClientId == client).Select(a => a.Category).Distinct().ToArray();
            return cates;
        }

        [HttpGet]
        public IEnumerable<AppSetting> Data(string client = null, string cate = null)
        {
            var query = repoAs.Query(o => o.SiteName == AuthHelper.CurrentClient.ClientId);
            if (!string.IsNullOrEmpty(client))
                query = query.Where(o => o.ClientId == client);
            if (!string.IsNullOrEmpty(cate))
                query = query.Where(o => o.Category == cate);
            query = query.OrderBy(o => o.ClientId).ThenBy(o => o.Category).ThenBy(o => o.ShowOrder);
            var objs = query.ToArray();
            return objs;
        }

        [HttpPost]
        public ServerResponse Save(ChangedData<AppSetting> items)
        {
            ServerResponse response = new ServerResponse();
            try
            {
                if (repoAs.SaveAll(trans, items.NewItems, items.ModifiedItems, items.DeletedItems))
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