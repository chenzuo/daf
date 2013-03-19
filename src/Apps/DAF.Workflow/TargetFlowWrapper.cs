using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAF.Core;
using DAF.Workflow.Models;

namespace DAF.Workflow
{
    public class TargetFlowWrapper
    {
        public TargetFlowWrapper(TargetFlowInfo FlowInfo)
        {
            this.FlowInfo = FlowInfo;
        }

        public void Build()
        {
            FlowInfo.BuildTargetFlow();
            var bs = FlowInfo.TargetStates.FirstOrDefault(o => o.State.StateType == StateType.Begin);
            if (bs != null)
            {
                BeginState = new TargetStateWrapper(this, bs);
                Stack<TargetStateWrapper> st = new Stack<TargetStateWrapper>();
                st.Push(BeginState);
                while (st.Count > 0)
                {
                    var prev = st.Pop();
                    // 普通节点、并行开始节点、并行操作节点、并行结束节点等
                    if (prev.TargetStates.Count() == 1)
                    {
                        var nextTStateIds = FlowInfo.NextTargetStates.Where(o => o.TargetStateId == prev.TargetStates.First().TargetStateId).Select(o => o.NextTargetStateId);
                        var ntstates = FlowInfo.TargetStates.Where(o => nextTStateIds.Contains(o.TargetStateId));
                        var ntsw = new TargetStateWrapper(this, ntstates);
                        if (ntstates.Count() == 1)
                        {
                            var nstate = FlowInfo.BizFlow.FlowStates.First(o => o.StateId == ntstates.First().StateId);
                            switch (nstate.StateType)
                            {
                                case StateType.ParallelBegin:
                                    prev.Next = ntsw;
                                    ntsw.Previous = prev;
                                    break;
                                case StateType.ParallelStop:
                                    var prevStateType = prev.TargetState().State.StateType;
                                    if (prevStateType == StateType.ParallelBegin)
                                    {
                                        var parallel = prev.Next;
                                        if (parallel == null)
                                        {
                                            parallel = new TargetStateWrapper(this, ntstates);
                                            prev.Next = parallel;
                                            parallel.Children = new List<TargetStateWrapper>();
                                            parallel.Owner = prev;
                                            parallel.Previous = prev;
                                            prev.Next = parallel;
                                        }
                                        ntsw.Owner = prev;
                                        parallel.Children.Add(ntsw);
                                    }
                                    else if (prevStateType == StateType.ParallelStop)
                                    {
                                        ntsw.Owner = prev.Owner;
                                        prev.Next = ntsw;
                                        ntsw.Previous = prev;
                                    }
                                    break;
                                case StateType.ParallelEnd:
                                    prev.Owner.Next.Next = ntsw;
                                    ntsw.Previous = prev.Owner.Next;
                                    break;
                                default:
                                    prev.Next = ntsw;
                                    ntsw.Previous = prev;
                                    break;
                            }
                        }
                        else if (ntstates.Count() > 1)
                        {
                            ntsw.Children = new List<TargetStateWrapper>();
                            ntsw.Owner = prev;
                            ntsw.Previous = prev;
                            prev.Next = ntsw;
                        }
                        st.Push(ntsw);
                    }
                    else if (prev.TargetStates.Count() > 1) // 并行虚拟节点
                    {
                        foreach (var ts in prev.TargetStates)
                        {
                            var ntsw = new TargetStateWrapper(this, ts);
                            ntsw.Owner = prev.Owner;
                            prev.Children.Add(ntsw);
                            st.Push(ntsw);
                        }
                    }
                }
            }
        }

        public TargetFlowInfo FlowInfo { get; private set; }
        public TargetStateWrapper BeginState { get; private set; }
    }

    public static class TargetFlowWrapperExtensions
    {
        public static bool HasNextBizFlow(this TargetFlowWrapper tfw)
        {
            return tfw != null && tfw.FlowInfo != null && tfw.FlowInfo.BizFlow != null && tfw.FlowInfo.BizFlow.BizFlow.NextBizFlows != null && tfw.FlowInfo.BizFlow.BizFlow.NextBizFlows.Count > 0;
        }
    }
}
