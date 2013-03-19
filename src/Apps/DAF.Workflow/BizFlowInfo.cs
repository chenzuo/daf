using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAF.Core;
using DAF.Workflow.Models;

namespace DAF.Workflow
{
    [DataContract]
    [KnownType(typeof(BizFlow))]
    [KnownType(typeof(FlowState))]
    [KnownType(typeof(FlowOperation))]
    [KnownType(typeof(FlowIncome))]
    [KnownType(typeof(FlowOutcome))]
    [KnownType(typeof(FlowStateIncome))]
    [KnownType(typeof(FlowStateOutcome))]
    [KnownType(typeof(FlowStateOperation))]
    [KnownType(typeof(NextBizFlow))]
    public class BizFlowInfo
    {
        [DataMember]
        public BizFlow BizFlow { get; set; }
        [DataMember]
        public IEnumerable<FlowState> FlowStates { get; set; }
        [DataMember]
        public IEnumerable<FlowOperation> FlowOperations { get; set; }
        [DataMember]
        public IEnumerable<FlowIncome> FlowIncomes { get; set; }
        [DataMember]
        public IEnumerable<FlowOutcome> FlowOutcomes { get; set; }

        [DataMember]
        public IEnumerable<FlowStateIncome> StateIncomes { get; set; }
        [DataMember]
        public IEnumerable<FlowStateOutcome> StateOutcomes { get; set; }
        [DataMember]
        public IEnumerable<FlowStateOperation> StateOperations { get; set; }

        [DataMember]
        public IEnumerable<NextBizFlow> NextBizFlows { get; set; }
    }

    public static class BizFlowInfoExtensioins
    {
        public static BizFlow BuildBizFlow(this BizFlowInfo bfi)
        {
            if (bfi == null || bfi.BizFlow == null)
                return null;
            BizFlow flow = bfi.BizFlow;
            flow.States = bfi.FlowStates.Where(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.Operations = bfi.FlowOperations.Where(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.Incomes = bfi.FlowIncomes.Where(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.Outcomes = bfi.FlowOutcomes.Where(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.NextBizFlows = bfi.NextBizFlows.ToArray();

            flow.States.ForEach(it =>
            {
                it.Operations = bfi.StateOperations.Where(op => op.StateId == it.StateId).ToArray();
                it.Incomes = bfi.StateIncomes.Where(op => op.StateId == it.StateId).ToArray();
                it.Outcomes = bfi.StateOutcomes.Where(op => op.StateId == it.StateId).ToArray();

                it.Operations.ForEach(o =>
                {
                    o.Operation = flow.Operations.FirstOrDefault(op => op.OperationId == o.OperationId);
                });

                it.Incomes.ForEach(o =>
                {
                    o.Income = flow.Incomes.FirstOrDefault(d => d.IncomeId == o.IncomeId);
                });

                it.Outcomes.ForEach(o =>
                {
                    o.Outcome = flow.Outcomes.FirstOrDefault(d => d.OutcomeId == o.OutcomeId);
                });
            });

            return flow;
        }
    }
}
