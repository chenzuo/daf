using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using DAF.Core;
using DAF.Core.IOC;
using DAF.Workflow;
using DAF.Workflow.Models;
using DAF.Workflow.Info;

namespace DAF.App.Workflow.Services
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“StateFlowService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 StateFlowService.svc 或 StateFlowService.svc.cs，然后开始调试。
    public class StateFlowService : DAF.Workflow.WcfProviders.IWcfStateFlowService
    {
        private IStateFlowService svr;

        public StateFlowService()
        {
            svr = IocInstance.Container.Resolve<IStateFlowService>();
        }

        public BizFlowInfo GetBizFlow(string clientId, string flowCodeOrTargetType)
        {
            return svr.GetBizFlow(clientId, flowCodeOrTargetType);
        }

        public BizFlowInfo GetBizFlow(string flowId)
        {
            return svr.GetBizFlow(flowId);
        }

        public TargetFlowInfo GetTargetFlow(string clientId, string flowCodeOrTargetType, string targetId)
        {
            return svr.GetTargetFlow(clientId, flowCodeOrTargetType, targetId);
        }

        public TargetFlowInfo GetTargetFlow(string targetFlowId)
        {
            return svr.GetTargetFlow(targetFlowId);
        }

        public IEnumerable<TargetFlowInfo> GetTargetFlows(string client, string flowCodeOrTargetType, DateTime? beginTime = null, DateTime? endTime = null, bool? started = null, bool? completed = null, FlowResult? result = null)
        {
            return svr.GetTargetFlows(client, flowCodeOrTargetType, beginTime, endTime, started, completed, result);
        }

        public TargetState StartFlow(StartFlowInfo info)
        {
            return svr.StartFlow(info);
        }

        public TargetState Plan(DoOperationInfo info)
        {
            return svr.Plan(info);
        }

        public TargetState Response(ResponseInfo info)
        {
            return svr.Response(info);
        }

        public TargetState Do(DoOperationInfo info)
        {
            return svr.Do(info);
        }

        public bool Cancel(DoOperationInfo info)
        {
            return svr.Cancel(info);
        }

        public bool UploadIncome(UploadInfo info)
        {
            return svr.UploadIncome(info);
        }

        public bool VerifyIncome(UploadInfo info)
        {
            return svr.VerifyIncome(info);
        }

        public bool RemoveIncome(UploadInfo info)
        {
            return svr.RemoveIncome(info);
        }

        public bool UploadOutcome(UploadInfo info)
        {
            return svr.UploadOutcome(info);
        }

        public bool VerifyOutcome(UploadInfo info)
        {
            return svr.VerifyOutcome(info);
        }

        public bool RemoveOutcome(UploadInfo info)
        {
            return svr.RemoveOutcome(info);
        }
    }
}
