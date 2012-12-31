using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Workflow.Models;
using DAF.Workflow.Info;

namespace DAF.Workflow
{
    public interface IStateFlowProvider
    {
        IEnumerable<TargetFlow> LoadFlows(string clientId, string flowCodeOrTargetType, DateTime? beginTime, DateTime? endTime, bool? started, bool? completed, FlowResult? result, bool loadAllInfo = true);
        TargetFlow LoadFlow(string clientId, string flowCodeOrTargetType, string targetId, bool loadAllInfo = true);
        TargetState LoadState(string targetStateId, bool loadAllInfo = true);
        TargetState GetCurrentState(string clientId, string targetFlowId, bool loadAllInfo = true);

        TargetState StartFlow(StartFlowInfo info);
        TargetState Plan(DoOperationInfo info);
        TargetState Response(ResponseInfo info);
        TargetState Do(DoOperationInfo info);
        TargetState Cancel(DoOperationInfo info);

        bool UploadIncome(UploadInfo info);
        bool VerifyIncome(UploadInfo info);
        bool RemoveIncome(UploadInfo info);

        bool UploadOutcome(UploadInfo info);
        bool VerifyOutcome(UploadInfo info);
        bool RemoveOutcome(UploadInfo info);
    }
}
