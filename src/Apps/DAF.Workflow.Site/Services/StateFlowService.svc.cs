using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using Autofac;
using DAF.Core;
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
            svr = IOC.Current.Resolve<IStateFlowService>();
        }

        public IEnumerable<TargetFlow> LoadFlows(string appName, string flowCodeOrTargetType, DateTime? beginTime, DateTime? endTime, bool? started, bool? completed, FlowResult? result, bool loadAllInfo)
        {
            return svr.LoadFlows(appName, flowCodeOrTargetType, beginTime, endTime, started, completed, result, loadAllInfo);
        }

        public TargetFlow LoadFlow(string appName, string flowCodeOrTargetType, string targetId, bool loadAllInfo)
        {
            return svr.LoadFlow(appName, flowCodeOrTargetType, targetId, loadAllInfo);
        }

        public TargetState LoadState(string targetStateId, bool loadAllInfo)
        {
            return svr.LoadState(targetStateId, loadAllInfo);
        }

        public TargetState GetCurrentState(string appName, string targetFlowId, bool loadAllInfo)
        {
            return svr.GetCurrentState(appName, targetFlowId, loadAllInfo);
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

        public TargetState Cancel(DoOperationInfo info)
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
