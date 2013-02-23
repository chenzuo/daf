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
        BizFlowInfo GetBizFlow(string clientId, string flowCodeOrTargetType);
        [OperationContract]
        BizFlowInfo GetBizFlow(string flowId);
        [OperationContract]
        TargetFlowInfo GetTargetFlow(string clientId, string flowCodeOrTargetType, string targetId);
        [OperationContract]
        TargetFlowInfo GetTargetFlow(string targetFlowId);
        [OperationContract]
        IEnumerable<TargetFlowInfo> GetTargetFlows(string client, string flowCodeOrTargetType, DateTime? beginTime = null, DateTime? endTime = null, bool? started = null, bool? completed = null, FlowResult? result = null);

        [OperationContract]
        TargetState StartFlow(StartFlowInfo info);
        [OperationContract]
        TargetState Plan(DoOperationInfo info);
        [OperationContract]
        TargetState Response(ResponseInfo info);
        [OperationContract]
        TargetState Do(DoOperationInfo info);
        [OperationContract]
        bool Cancel(DoOperationInfo info);

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
