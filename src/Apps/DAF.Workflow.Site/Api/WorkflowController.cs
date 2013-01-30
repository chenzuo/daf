using System;
using System.Collections.Generic;
using System.Linq;
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
        private IStateFlowService flowSevr;

        public WorkflowController(IStateFlowManager flowManager, IStateFlowService flowSevr)
        {
            this.flowManager = flowManager;
            this.flowSevr = flowSevr;
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
        public IEnumerable<TargetFlow> TargetFlows(string client, string flowCodeOrTargetType, DateTime? beginTime = null, DateTime? endTime = null, bool? started = null, FlowResult? result = null, bool? completed = null, bool loadAllInfo = true)
        {
            var flows = flowSevr.LoadFlows(client, flowCodeOrTargetType, beginTime, endTime, started, completed, result, loadAllInfo);
            return flows;
        }

        [HttpGet]
        public TargetFlow TargetFlow(string client, string flowCodeOrTargetType, string targetId, bool loadAllInfo = true)
        {
            var flow = flowSevr.LoadFlow(client, flowCodeOrTargetType, targetId, loadAllInfo);
            return flow;
        }

        [HttpGet]
        public TargetFlow TargetFlow(string id, bool loadAllInfo = true)
        {
            var flow = flowSevr.LoadFlow(id, loadAllInfo);
            return flow;
        }
    }
}