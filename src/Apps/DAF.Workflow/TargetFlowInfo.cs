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
    [KnownType(typeof(TargetFlow))]
    [KnownType(typeof(TargetState))]
    [KnownType(typeof(TargetIncome))]
    [KnownType(typeof(TargetOutcome))]
    [KnownType(typeof(NextTargetState))]
    [KnownType(typeof(BizFlowInfo))]
    public class TargetFlowInfo
    {
        [DataMember]
        public BizFlowInfo BizFlow { get; set; }

        [DataMember]
        public TargetFlow TargetFlow { get; set; }
        [DataMember]
        public IEnumerable<TargetState> TargetStates { get; set; }
        [DataMember]
        public IEnumerable<TargetIncome> TargetIncomes { get; set; }
        [DataMember]
        public IEnumerable<TargetOutcome> TargetOutcomes { get; set; }
        [DataMember]
        public IEnumerable<NextTargetState> NextTargetStates { get; set; }
    }

    public static class TargetFlowInfoExtensioins
    {
        public static TargetFlow BuildTargetFlow(this TargetFlowInfo tfi)
        {
            if (tfi == null || tfi.TargetFlow == null)
                return null;
            TargetFlow tflow = tfi.TargetFlow;

            tflow.Flow = tfi.BizFlow.BuildBizFlow();
            tflow.TreatedStates = tfi.TargetStates.Where(o => o.TargetFlowId == tflow.TargetFlowId).ToArray();
            tflow.TreatedStates.ForEach(o =>
            {
                o.TargetIncomes = tfi.TargetIncomes.Where(d => d.TargetStateId == o.TargetStateId).ToArray();
                o.TargetOutcomes = tfi.TargetOutcomes.Where(d => d.TargetStateId == o.TargetStateId).ToArray();
                o.State = tflow.Flow.States.FirstOrDefault(s => s.StateId == o.StateId);
                o.Operation = tflow.Flow.Operations.FirstOrDefault(p => p.OperationId == o.OperationId);
                o.TargetIncomes.ForEach(obj =>
                {
                    obj.Income = tflow.Flow.Incomes.FirstOrDefault(d => d.IncomeId == obj.IncomeId);
                });
                o.TargetOutcomes.ForEach(obj =>
                {
                    obj.Outcome = tflow.Flow.Outcomes.FirstOrDefault(d => d.OutcomeId == obj.OutcomeId);
                });
            });

            return tflow;
        }
    }
}
