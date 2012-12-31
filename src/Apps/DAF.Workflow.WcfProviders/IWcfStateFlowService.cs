using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DAF.Core;
using DAF.Workflow.Models;
using DAF.Workflow.Info;

namespace DAF.Workflow.WcfProviders
{
    [ServiceContract]
    public interface IWcfStateFlowService
    {
        [OperationContract]
        IEnumerable<TargetFlow> LoadFlows(string appName, string flowCodeOrTargetType, DateTime? beginTime, DateTime? endTime, bool? started, bool? completed, FlowResult? result, bool loadAllInfo);
        [OperationContract]
        TargetFlow LoadFlow(string appName, string flowCodeOrTargetType, string targetId, bool loadAllInfo);
        [OperationContract]
        TargetState LoadState(string targetStateId, bool loadAllInfo);
        [OperationContract]
        TargetState GetCurrentState(string appName, string targetFlowId, bool loadAllInfo);

        [OperationContract]
        TargetState StartFlow(StartFlowInfo info);
        [OperationContract]
        TargetState Plan(DoOperationInfo info);
        [OperationContract]
        TargetState Response(ResponseInfo info);
        [OperationContract]
        TargetState Do(DoOperationInfo info);
        [OperationContract]
        TargetState Cancel(DoOperationInfo info);

        [OperationContract]
        bool UploadIncome(UploadInfo info);
        [OperationContract]
        bool VerifyIncome(UploadInfo info);
        [OperationContract]
        bool RemoveIncome(UploadInfo info);

        [OperationContract]
        bool UploadOutcome(UploadInfo info);
        [OperationContract]
        bool VerifyOutcome(UploadInfo info);
        [OperationContract]
        bool RemoveOutcome(UploadInfo info);
    }
}
