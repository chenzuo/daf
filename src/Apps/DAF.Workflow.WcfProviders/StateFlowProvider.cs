using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DAF.Core;
using DAF.Workflow;
using DAF.Workflow.Models;
using DAF.Workflow.Info;

namespace DAF.Workflow.WcfProviders
{
    public class StateFlowProvider : IStateFlowProvider
    {
        private ChannelFactory<IWcfStateFlowService> CreateChannel()
        {
            return WcfService.CreateChannel<IWcfStateFlowService>("IWcfStateFlowService");
        }

        public IEnumerable<TargetFlow> LoadFlows(string appName, string flowCodeOrTargetType, DateTime? beginTime, DateTime? endTime, bool? started, bool? completed, FlowResult? result, bool loadAllInfo = true)
        {
            Assert.IsStringNotNullOrEmpty(appName);

            IEnumerable<TargetFlow> tflows = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                tflows = p.LoadFlows(appName, flowCodeOrTargetType, beginTime, endTime, started, completed, result, loadAllInfo);
            });

            return tflows;
        }

        public TargetFlow LoadFlow(string appName, string flowCodeOrTargetType, string targetId, bool loadAllInfo = true)
        {
            Assert.IsStringNotNullOrEmpty(appName);
            
            TargetFlow result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.LoadFlow(appName, flowCodeOrTargetType, targetId, loadAllInfo);
            });

            return result;
        }

        public TargetState LoadState(string targetStateId, bool loadAllInfo = true)
        {
            Assert.IsStringNotNullOrEmpty(targetStateId);

            TargetState result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.LoadState(targetStateId, loadAllInfo);
            });

            return result;
        }

        public TargetState GetCurrentState(string appName, string targetFlowId, bool loadAllInfo = true)
        {
            Assert.IsStringNotNullOrEmpty(appName);

            TargetState result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.GetCurrentState(appName, targetFlowId, loadAllInfo);
            });

            return result;
        }

        public TargetState StartFlow(StartFlowInfo info)
        {
            Assert.IsNotNull(info);

            TargetState result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.StartFlow(info);
            });

            return result;
        }

        public TargetState Plan(DoOperationInfo info)
        {
            Assert.IsNotNull(info);

            TargetState result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.Plan(info);
            });

            return result;
        }

        public TargetState Response(ResponseInfo info)
        {
            Assert.IsNotNull(info);

            TargetState result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.Response(info);
            });

            return result;
        }

        public TargetState Do(DoOperationInfo info)
        {
            Assert.IsNotNull(info);

            TargetState result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.Do(info);
            });

            return result;
        }

        public TargetState Cancel(DoOperationInfo info)
        {
            Assert.IsNotNull(info);

            TargetState result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.Cancel(info);
            });

            return result;
        }

        public bool UploadIncome(UploadInfo info)
        {
            Assert.IsNotNull(info);

            bool result = false;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.UploadIncome(info);
            });

            return result;
        }

        public bool VerifyIncome(UploadInfo info)
        {
            Assert.IsNotNull(info);

            bool result = false;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.VerifyIncome(info);
            });

            return result;
        }

        public bool RemoveIncome(UploadInfo info)
        {
            Assert.IsNotNull(info);

            bool result = false;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.RemoveIncome(info);
            });

            return result;
        }

        public bool UploadOutcome(UploadInfo info)
        {
            Assert.IsNotNull(info);

            bool result = false;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.UploadOutcome(info);
            });

            return result;
        }

        public bool VerifyOutcome(UploadInfo info)
        {
            Assert.IsNotNull(info);

            bool result = false;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.VerifyOutcome(info);
            });

            return result;
        }

        public bool RemoveOutcome(UploadInfo info)
        {
            Assert.IsNotNull(info);

            bool result = false;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.RemoveOutcome(info);
            });

            return result;
        }
    }
}
