using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DAF.Core.Data;
using DAF.Workflow.Models;
using DAF.Workflow.Info;

namespace DAF.Workflow
{
    public interface IStateFlowManager
    {
        IQueryable<BizFlow> GetFlows(Expression<Func<BizFlow, bool>> predicate);
        BizFlow GetFlow(Expression<Func<BizFlow, bool>> predicate, bool loadAllInfo = true);
        bool SaveFlow(BizFlow flow);
        bool SaveFlows(string clientId, string owner, string biz, IEnumerable<BizFlow> flows);
        bool SaveFlows(BizFlow[] newItems, BizFlow[] modifiedItems, BizFlow[] deletedItems);
        bool VerifyFlow(string flowId, out string message);
        FlowState GetState(string stateId, bool loadAllInfo = true);
        FlowOperation GetOperation(string operationId);
        FlowIncome GetIncome(string incomeId);
        FlowOutcome GetOutcome(string outcomeId);

        IQueryable<TargetFlow> LoadFlows();
        TargetFlow LoadFlow(string targetFlowId, bool loadAllInfo = true);
        TargetState GetTargetState(string targetFlowId, bool loadAllInfo = true);
    }
}
