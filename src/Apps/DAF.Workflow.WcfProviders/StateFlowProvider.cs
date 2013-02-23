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

        public BizFlowInfo GetBizFlow(string clientId, string flowCodeOrTargetType)
        {
            Assert.IsStringNotNullOrEmpty(clientId);
            Assert.IsStringNotNullOrEmpty(flowCodeOrTargetType);

            BizFlowInfo result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.GetBizFlow(clientId, flowCodeOrTargetType);
            });

            return result;
        }

        public BizFlowInfo GetBizFlow(string flowId)
        {
            Assert.IsStringNotNullOrEmpty(flowId);

            BizFlowInfo result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.GetBizFlow(flowId);
            });

            return result;
        }

        public TargetFlowInfo GetTargetFlow(string clientId, string flowCodeOrTargetType, string targetId)
        {
            Assert.IsStringNotNullOrEmpty(clientId);
            Assert.IsStringNotNullOrEmpty(flowCodeOrTargetType);

            TargetFlowInfo result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.GetTargetFlow(clientId, flowCodeOrTargetType, targetId);
            });

            return result;
        }

        public TargetFlowInfo GetTargetFlow(string targetFlowId)
        {
            Assert.IsStringNotNullOrEmpty(targetFlowId);

            TargetFlowInfo result = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                result = p.GetTargetFlow(targetFlowId);
            });

            return result;
        }

        public IEnumerable<TargetFlowInfo> GetTargetFlows(string client, string flowCodeOrTargetType, DateTime? beginTime = null, DateTime? endTime = null, bool? started = null, bool? completed = null, FlowResult? result = null)
        {
            Assert.IsStringNotNullOrEmpty(client);
            Assert.IsStringNotNullOrEmpty(flowCodeOrTargetType);

            IEnumerable<TargetFlowInfo> r = null;
            var chanel = CreateChannel();
            chanel.Call(p =>
            {
                r = p.GetTargetFlows(client, flowCodeOrTargetType, beginTime, endTime, started, completed, result);
            });

            return r;
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

        public bool Cancel(DoOperationInfo info)
        {
            Assert.IsNotNull(info);

            bool result = false;
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
