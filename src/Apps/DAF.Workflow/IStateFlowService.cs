using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core.Data;
using DAF.Workflow.Models;
using DAF.Workflow.Info;

namespace DAF.Workflow
{
    public interface IStateFlowService
    {
        BizFlowInfo GetBizFlow(string clientId, string flowCodeOrTargetType);
        BizFlowInfo GetBizFlow(string flowId);
        TargetFlowInfo GetTargetFlow(string clientId, string flowCodeOrTargetType, string targetId);
        TargetFlowInfo GetTargetFlow(string targetFlowId);
        IEnumerable<TargetFlowInfo> GetTargetFlows(string client, string flowCodeOrTargetType, DateTime? beginTime = null, DateTime? endTime = null, bool? started = null, bool? completed = null, FlowResult? result = null);

        TargetState StartFlow(StartFlowInfo info);
        TargetState Plan(DoOperationInfo info);
        TargetState Response(ResponseInfo info);
        TargetState Do(DoOperationInfo info);
        bool Cancel(DoOperationInfo info);

        bool UploadIncome(UploadInfo info);
        bool VerifyIncome(UploadInfo info);
        bool RemoveIncome(UploadInfo info);

        bool UploadOutcome(UploadInfo info);
        bool VerifyOutcome(UploadInfo info);
        bool RemoveOutcome(UploadInfo info);
    }
}
