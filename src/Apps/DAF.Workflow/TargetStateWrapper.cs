using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Workflow.Models;

namespace DAF.Workflow
{
    public class TargetStateWrapper
    {
        private TargetFlowWrapper tflow;
        private List<TargetState> tstates;

        public TargetStateWrapper(TargetFlowWrapper tflow, TargetState tstate)
            : this(tflow, new TargetState[] { tstate })
        {
        }

        public TargetStateWrapper(TargetFlowWrapper tflow, IEnumerable<TargetState> tstates)
        {
            this.tflow = tflow;
            this.tstates = new List<TargetState>();
            this.tstates.AddRange(tstates);
        }

        public IEnumerable<TargetState> TargetStates
        {
            get { return tstates; }
        }

        private List<FlowOperation> operations;
        public IEnumerable<FlowOperation> Operations
        {
            get
            {
                if (operations != null)
                    return operations;
                operations = new List<FlowOperation>();
                if (TargetStates != null && TargetStates.Count() > 0)
                {
                    if (TargetStates.Count() == 1)
                    {
                        var tstate = TargetStates.First();
                        if (tstate.StateStatus == StateStatus.None || tstate.StateStatus == StateStatus.Responsed || tstate.StateStatus == StateStatus.Started)
                        {
                            if (tstate.State.StateType != StateType.End)
                            {
                                var ops = this.StateOperations();
                                if (tstate.State.StateType == StateType.ParallelStop)
                                {
                                    foreach (var op in ops)
                                    {
                                        var nextOpState = tflow.FlowInfo.BizFlow.FlowStates.First(o => o.StateId == op.DefaultNextStateId);
                                        if (nextOpState.StateType != StateType.ParallelEnd)
                                        {
                                            operations.Add(op);
                                        }
                                    }
                                }
                                else
                                {
                                    operations.AddRange(ops);
                                }
                            }
                        }
                    }
                    else if (TargetStates.Count() > 1)
                    {
                        if (Owner != null && Children != null && Children.Count > 0)
                        {
                            var parallelOwnerTargetState = Owner.TargetState();
                            var parallelOwnerState = parallelOwnerTargetState.State;
                            var allShouldDone = parallelOwnerState.AllParallelStateShouldBeEnd.HasValue && parallelOwnerState.AllParallelStateShouldBeEnd.Value;
                            Stack<FlowOperation> stOps = new Stack<FlowOperation>();
                            var ops = Owner.StateOperations();
                            ops.ForEach(o => stOps.Push(o));
                            while (stOps.Count > 0)
                            {
                                var sop = stOps.Pop();
                                var nextOpState = tflow.FlowInfo.BizFlow.FlowStates.First(o => o.StateId == sop.DefaultNextStateId);
                                if (nextOpState.StateType == StateType.ParallelEnd && 
                                    !tflow.FlowInfo.TargetStates.Any(o => o.OperationId == sop.OperationId) && 
                                    !operations.Any(o => o.OperationId == sop.OperationId))
                                {
                                    operations.Add(sop);
                                }
                                else
                                {
                                    var nextOps = tflow.FlowInfo.BizFlow.FlowOperations.Where(o => nextOpState.Operations.Any(p => p.OperationId == o.OperationId));
                                    nextOps.ForEach(o => stOps.Push(o));
                                }
                            }
                        }
                    }
                }
                return operations;
            }
        }

        public TargetStateWrapper Previous { get; internal set; }
        public TargetStateWrapper Next { get; internal set; }
        public TargetStateWrapper Owner { get; internal set; }
        public ICollection<TargetStateWrapper> Children { get; internal set; }
        public TargetFlowWrapper TargetFlow { get { return tflow; } }
    }

    public static class TargetStateWrapperExtensions
    {
        public static IEnumerable<FlowOperation> StateOperations(this TargetStateWrapper tsw)
        {
            var tstate = tsw.TargetState();
            if (tstate == null)
                return Enumerable.Empty<FlowOperation>();
            var state = tsw.TargetFlow.FlowInfo.BizFlow.FlowStates.First(o => o.StateId == tstate.StateId);
            var opids = tsw.TargetFlow.FlowInfo.BizFlow.StateOperations.Where(o => o.StateId == state.StateId).Select(o => o.OperationId);
            var ops = tsw.TargetFlow.FlowInfo.BizFlow.FlowOperations.Where(o => opids.Contains(o.OperationId));
            return ops;
        }

        public static TargetState TargetState(this TargetStateWrapper tsw)
        {
            if (tsw.TargetStates == null)
                return null;
            if (tsw.TargetStates.Count() == 1)
                return tsw.TargetStates.First();
            return null;
        }

        public static bool IsInParallel(this TargetStateWrapper tsw)
        {
            return tsw != null && tsw.Children != null && tsw.Children.Count() > 0;
        }

        public static IEnumerable<FlowOperation> ParallelOperations(this TargetStateWrapper tsw)
        {
            var parallelOwnerTargetState = tsw.Owner.TargetState();
            var parallelOwnerState = parallelOwnerTargetState.State;
            var ops = tsw.Owner.StateOperations();
            var doneOpIds = tsw.TargetFlow.FlowInfo.TargetStates.Where(o => !string.IsNullOrEmpty(o.OperationId)).Select(o => o.OperationId);
            return ops.Where(o => !doneOpIds.Contains(o.OperationId));
        }

        public static bool HasOperations(this TargetStateWrapper tsw)
        {
            return tsw != null && tsw.Operations != null && tsw.Operations.Count() > 0;
        }

        public static string Title(this TargetStateWrapper tsw)
        {
            var ts = tsw.TargetState();
            if (ts != null)
                return ts.Title;
            return string.Empty;
        }

        public static string StatusName(this TargetStateWrapper tsw)
        {
            var tstate = tsw.TargetState();
            if(tstate != null)
                return LocaleHelper.Localizer.Get("StateStatus_" + Enum.GetName(typeof(StateStatus), tstate.StateStatus), "DAF.Workflow");
            return string.Empty;
        }
    }
}
