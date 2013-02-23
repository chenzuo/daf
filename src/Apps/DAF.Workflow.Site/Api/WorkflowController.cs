using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAF.Core;
using DAF.Web;
using DAF.Workflow;
using DAF.Workflow.Models;

namespace DAF.Workflow.Site.Api
{
    public class WorkflowController : ApiController
    {
        private IStateFlowManager flowManager;

        public WorkflowController(IStateFlowManager flowManager)
        {
            this.flowManager = flowManager;
        }

        [HttpGet]
        public IEnumerable<BizFlow> Flows(string client)
        {
            var query = flowManager.GetFlows(o => o.ClientId == client).OrderBy(o => o.Code);
            return query.ToArray();
        }

        [HttpPost]
        public ServerResponse SaveFlows([FromBody]ChangedData<BizFlow> objs)
        {
            return objs.Save(items => flowManager.SaveFlows(items.NewItems, items.ModifiedItems, items.DeletedItems));
        }

        [HttpGet]
        public BizFlow GetFlow(string id, bool loadAllInfo = true)
        {
            var item = flowManager.GetFlow(o => o.FlowId == id, loadAllInfo);
            return item;
        }

        [HttpPost]
        public ServerResponse SaveFlow([FromBody]BizFlow obj)
        {
            return obj.Save(flow => flowManager.SaveFlow(flow));
        }

        [HttpGet]
        public IEnumerable<TargetFlow> TargetFlows(string client, string flowCodeOrTargetType, DateTime? beginTime = null, DateTime? endTime = null, bool? started = null, bool? completed = null, FlowResult? result = null, bool loadAllInfo = true)
        {
            var query = flowManager.LoadFlows();
            query = query.Where(o => o.Flow.ClientId == client && (o.Flow.Code == flowCodeOrTargetType || o.Flow.TargetType == flowCodeOrTargetType));

            if(beginTime.HasValue)
                query = query.Where(o => o.CreateTime >= beginTime.Value);
            if(endTime.HasValue)
                query = query.Where(o => o.CreateTime <= endTime.Value);

            if(started.HasValue)
                query = query.Where(o => o.HasStarted == started.Value);
            if(completed.HasValue)
                query = query.Where(o => o.HasCompleted == completed.Value);
            if(result.HasValue)
                query = query.Where(o => o.Result == result.Value);

            var ids = query.Select(o => o.TargetFlowId).ToArray();
            List<TargetFlow> tflows = new List<TargetFlow>();
            foreach(var id in ids)
            {
                var tflow = flowManager.LoadFlow(id, loadAllInfo);
                tflows.Add(tflow);
            }

            return tflows;
        }

        [HttpGet]
        public TargetFlow TargetFlow(string client, string flowCodeOrTargetType, string targetId, bool loadAllInfo = true)
        {
            var query = flowManager.LoadFlows();
            var id = query.Where(o => o.Flow.ClientId == client && (o.Flow.Code == flowCodeOrTargetType || o.Flow.TargetType == flowCodeOrTargetType) && o.TargetId == targetId)
                .Select(o => o.TargetFlowId).FirstOrDefault();

            if (!string.IsNullOrEmpty(id))
            {
                var flow = flowManager.LoadFlow(id, loadAllInfo);
                return flow;
            }
            return null;
        }

        [HttpGet]
        public TargetFlow TargetFlow(string id, bool loadAllInfo = true)
        {
            var flow = flowManager.LoadFlow(id, loadAllInfo);
            return flow;
        }
    }
}