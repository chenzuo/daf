using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DAF.Core.Logging;
using DAF.Core;
using DAF.Core.Data;
using DAF.Core.Generators;
using DAF.Core.Security;
using DAF.Core.Messaging;
using DAF.Workflow.Models;
using DAF.Workflow.Messages;
using DAF.Workflow.Info;

namespace DAF.Workflow
{
    public class RepositoryStateFlowService : IStateFlowService
    {
        private IIdGenerator idGenerator;
        private ITransactionManager trans;
        private IRepository<BizFlow> repoBizFlow;
        private IRepository<FlowState> repoFlowState;
        private IRepository<FlowIncome> repoFlowIncome;
        private IRepository<FlowOutcome> repoFlowOutcome;
        private IRepository<FlowOperation> repoFlowOp;
        private IRepository<FlowStateOperation> repoStateOp;
        private IRepository<FlowStateIncome> repoStateIncome;
        private IRepository<FlowStateOutcome> repoStateOutcome;
        private IRepository<TargetFlow> repoTargetFlow;
        private IRepository<TargetState> repoTargetState;
        private IRepository<TargetIncome> repoTargetIncome;
        private IRepository<TargetOutcome> repoTargetOutcome;
        private IRepository<NextBizFlow> repoNextFlow;
        private IRepository<NextTargetState> repoNextTargetState;

        public RepositoryStateFlowService(
            IIdGenerator idGenerator,
            ITransactionManager trans,
            IRepository<BizFlow> repoStateFlow,
            IRepository<FlowState> repoFlowState,
            IRepository<FlowIncome> repoFlowIncome,
            IRepository<FlowOutcome> repoFlowOutcome,
            IRepository<FlowOperation> repoFlowOp,
            IRepository<FlowStateOperation> repoStateOp,
            IRepository<FlowStateIncome> repoStateIncome,
            IRepository<FlowStateOutcome> repoStateOutcome,
            IRepository<TargetFlow> repoTargetFlow,
            IRepository<TargetState> repoTargetState,
            IRepository<TargetIncome> repoTargetIncome,
            IRepository<TargetOutcome> repoTargetOutcome,
            IRepository<NextBizFlow> repoNextFlow,
            IRepository<NextTargetState> repoNextTargetState)
        {
            this.idGenerator = idGenerator;
            this.trans = trans;
            this.repoBizFlow = repoStateFlow;
            this.repoFlowState = repoFlowState;
            this.repoFlowIncome = repoFlowIncome;
            this.repoFlowOutcome = repoFlowOutcome;
            this.repoFlowOp = repoFlowOp;
            this.repoStateOp = repoStateOp;
            this.repoStateIncome = repoStateIncome;
            this.repoStateOutcome = repoStateOutcome;
            this.repoTargetFlow = repoTargetFlow;
            this.repoTargetState = repoTargetState;
            this.repoTargetIncome = repoTargetIncome;
            this.repoTargetOutcome = repoTargetOutcome;
            this.repoNextFlow = repoNextFlow;
            this.repoNextTargetState = repoNextTargetState;

            Logger = NullLogger.Instance;
        }

        public BizFlowInfo GetBizFlow(string clientId, string flowCodeOrTargetType)
        {
            var flow = repoBizFlow.Query(o => o.ClientId == clientId && (o.Code == flowCodeOrTargetType || o.TargetType == flowCodeOrTargetType)).FirstOrDefault();
            return GetBizFlow(flow);
        }

        public BizFlowInfo GetBizFlow(string flowId)
        {
            var flow = repoBizFlow.Query(o => o.FlowId == flowId).FirstOrDefault();
            return GetBizFlow(flow);
        }

        private BizFlowInfo GetBizFlow(BizFlow flow)
        {
            if (flow != null)
            {
                BizFlowInfo bfi = new BizFlowInfo();
                bfi.BizFlow = flow;
                bfi.FlowStates = repoFlowState.Query(o => o.FlowId == flow.FlowId).ToArray();
                bfi.FlowOperations = repoFlowOp.Query(o => o.FlowId == flow.FlowId).ToArray();
                bfi.FlowIncomes = repoFlowIncome.Query(o => o.FlowId == flow.FlowId).ToArray();
                bfi.FlowOutcomes = repoFlowOutcome.Query(o => o.FlowId == flow.FlowId).ToArray();
                bfi.StateOperations = repoStateOp.Query(o => o.State.FlowId == flow.FlowId).ToArray();
                bfi.StateIncomes = repoStateIncome.Query(o => o.State.FlowId == flow.FlowId).ToArray();
                bfi.StateOutcomes = repoStateOutcome.Query(o => o.State.FlowId == flow.FlowId).ToArray();
                bfi.NextBizFlows = repoNextFlow.Query(o => o.FlowId == flow.FlowId).ToArray();
                bfi.NextBizFlows.ForEach(n =>
                {
                    n.FromBizFlow = bfi.BizFlow;
                    n.ToBizFlow = repoBizFlow.Query(o => o.FlowId == n.NextFlowId).FirstOrDefault();
                });
                return bfi;
            }
            return null;
        }

        public TargetFlowInfo GetTargetFlow(string clientId, string flowCodeOrTargetType, string targetId)
        {
            var tflow = repoTargetFlow.Query(o => o.Flow.ClientId == clientId && (o.Flow.Code == flowCodeOrTargetType || o.Flow.TargetType == flowCodeOrTargetType) && o.TargetId == targetId).FirstOrDefault();
            return GetTargetFlow(tflow);
        }

        public TargetFlowInfo GetTargetFlow(string targetFlowId)
        {
            var tflow = repoTargetFlow.Query(o => o.TargetFlowId == targetFlowId).FirstOrDefault();
            return GetTargetFlow(tflow);
        }

        private TargetFlowInfo GetTargetFlow(TargetFlow tflow)
        {
            if (tflow != null)
            {
                if (!string.IsNullOrEmpty(tflow.LastTargetFlowId))
                    tflow.LastTargetFlow = repoTargetFlow.Query(o => o.TargetFlowId == tflow.LastTargetFlowId).FirstOrDefault();
                TargetFlowInfo tfi = new TargetFlowInfo();
                tfi.TargetFlow = tflow;
                tfi.BizFlow = GetBizFlow(tfi.TargetFlow.FlowId);
                tfi.TargetStates = repoTargetState.Query(o => o.TargetFlowId == tflow.TargetFlowId).ToArray();
                tfi.TargetIncomes = repoTargetIncome.Query(o => o.TargetState.TargetFlowId == tflow.TargetFlowId).ToArray();
                tfi.TargetOutcomes = repoTargetOutcome.Query(o => o.TargetState.TargetFlowId == tflow.TargetFlowId).ToArray();
                tfi.NextTargetStates = repoNextTargetState.Query(o => o.FromTargetState.TargetFlowId == tflow.TargetFlowId).ToArray();
                return tfi;
            }
            return null;
        }

        public IEnumerable<TargetFlowInfo> GetTargetFlows(string client, string flowCodeOrTargetType, DateTime? beginTime = null, DateTime? endTime = null, bool? started = null, bool? completed = null, FlowResult? result = null)
        {
            var query = repoTargetFlow.Query(o => o.Flow.ClientId == client && (o.Flow.Code == flowCodeOrTargetType || o.Flow.TargetType == flowCodeOrTargetType));

            if (beginTime.HasValue)
                query = query.Where(o => o.CreateTime >= beginTime.Value);
            if (endTime.HasValue)
                query = query.Where(o => o.CreateTime <= endTime.Value);

            if (started.HasValue)
                query = query.Where(o => o.HasStarted == started.Value);
            if (completed.HasValue)
                query = query.Where(o => o.HasCompleted == completed.Value);
            if (result.HasValue)
                query = query.Where(o => o.Result == result.Value);

            query = query.OrderByDescending(o => o.CreateTime);
            var tflows = query.ToArray();
            return tflows.Select(o => GetTargetFlow(o));
        }

        /*
        public BizFlow GetFlow(string flowId, bool loadAllInfo = true)
        {
            BizFlow flow = repoBizFlow.Query(o => o.FlowId == flowId).FirstOrDefault();
            if (flow == null)
                throw new NullReferenceException(string.Format("Flow {0} not found!", flowId));

            if (loadAllInfo)
            {
                LoadBizFlowAllInfo(flow);
            }

            return flow;
        }

        private void LoadBizFlowAllInfo(BizFlow flow)
        {
            flow.States = repoFlowState.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.Operations = repoFlowOp.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.Incomes = repoFlowIncome.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.Outcomes = repoFlowOutcome.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
            flow.NextBizFlows = repoNextFlow.Query(o => o.FlowId == flow.FlowId).ToArray();

            flow.States.ForEach(it =>
            {
                it.Operations = repoStateOp.Query(op => op.StateId == it.StateId).ToArray();
                it.Incomes = repoStateIncome.Query(op => op.StateId == it.StateId).ToArray();
                it.Outcomes = repoStateOutcome.Query(op => op.StateId == it.StateId).ToArray();

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

            flow.NextBizFlows.ForEach(it =>
            {
                it.FromBizFlow = flow;
                it.ToBizFlow = repoBizFlow.Query(o => o.FlowId == it.NextFlowId).FirstOrDefault();
            });
        }

        public IEnumerable<TargetFlow> LoadFlows(string clientId, string flowCodeOrTargetType, DateTime? beginTime, DateTime? endTime, bool? started, bool? completed, FlowResult? result, bool loadAllInfo = true)
        {
            var query = repoTargetFlow.Query(o => o.Flow.ClientId == clientId && (o.Flow.Code == flowCodeOrTargetType || o.Flow.TargetType == flowCodeOrTargetType));
            if (beginTime.HasValue)
            {
                query = query.Where(o => o.CreateTime >= beginTime.Value);
            }
            if (endTime.HasValue)
            {
                query = query.Where(o => o.CreateTime <= endTime.Value);
            }
            if (started.HasValue)
            {
                query = query.Where(o => o.HasStarted == started.Value);
            }
            if (completed.HasValue)
            {
                query = query.Where(o => o.HasCompleted == completed.Value);
            }
            if (result.HasValue)
            {
                query = query.Where(o => o.Result.Value == result.Value);
            }
            var tflows = query.ToArray();
            if (loadAllInfo)
            {
                tflows.ForEach(o => LoadTargetFlowAllInfo(o));
            }
            return tflows;
        }

        public TargetFlow LoadFlow(string clientId, string flowCodeOrTargetType, string targetId, bool loadAllInfo = true)
        {
            var tflowId = repoTargetFlow.Query(o => o.Flow.ClientId == clientId && (o.Flow.Code == flowCodeOrTargetType || o.Flow.TargetType == flowCodeOrTargetType) && o.TargetId == targetId).Select(o => o.TargetFlowId).FirstOrDefault();
            if (string.IsNullOrEmpty(tflowId))
                return null;

            return LoadFlow(tflowId, loadAllInfo);
        }

        public TargetFlow LoadFlow(string targetFlowId, bool loadAllInfo = true)
        {
            var tflow = repoTargetFlow.Query(o => o.TargetFlowId == targetFlowId).FirstOrDefault();
            if (tflow == null)
                return null;

            if (tflow != null && loadAllInfo)
            {
                LoadTargetFlowAllInfo(tflow);
            }

            return tflow;
        }

        private void LoadTargetFlowAllInfo(TargetFlow tflow)
        {
            tflow.Flow = GetFlow(tflow.FlowId, true);
            tflow.TreatedStates = repoTargetState.Query(o => o.TargetFlowId == tflow.TargetFlowId).ToArray();
            tflow.TreatedStates.ForEach(o =>
            {
                o.TargetIncomes = repoTargetIncome.Query(d => d.TargetStateId == o.TargetStateId).ToArray();
                o.TargetOutcomes = repoTargetOutcome.Query(d => d.TargetStateId == o.TargetStateId).ToArray();
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
            if (!string.IsNullOrEmpty(tflow.LastTargetFlowId))
            {
                tflow.LastTargetFlow = repoTargetFlow.Query(o => o.TargetFlowId == tflow.LastTargetFlowId).FirstOrDefault();
            }
        }

        public TargetState LoadState(string targetStateId, bool loadAllInfo = true)
        {
            var tstate = repoTargetState.Query(o => o.TargetStateId == targetStateId).OrderByDescending(o => o.OperateTime).FirstOrDefault();
            if (tstate == null)
                throw new NullReferenceException(string.Format("Target Flow state {0} not found!", targetStateId));

            if (loadAllInfo)
            {
                LoadTargetStateAllInfo(tstate);
            }

            return tstate;
        }

        private void LoadTargetStateAllInfo(TargetState tstate)
        {
            tstate.TargetFlow = repoTargetFlow.Query(o => o.TargetFlowId == tstate.TargetFlowId).FirstOrDefault();
            tstate.State = repoFlowState.Query(o => o.StateId == tstate.StateId).FirstOrDefault();
            if (tstate.State != null)
            {
                tstate.State.Operations = repoStateOp.Query(o => o.StateId == tstate.StateId).ToArray();
                tstate.State.Operations.ForEach(op =>
                {
                    op.State = tstate.State;
                    op.Operation = repoFlowOp.Query(o => o.OperationId == op.OperationId).FirstOrDefault();
                });

                tstate.State.Incomes = repoStateIncome.Query(o => o.StateId == tstate.StateId).ToArray();
                tstate.State.Incomes.ForEach(obj =>
                {
                    obj.State = tstate.State;
                    obj.Income = repoFlowIncome.Query(o => o.IncomeId == obj.IncomeId).FirstOrDefault();
                });

                tstate.State.Outcomes = repoStateOutcome.Query(o => o.StateId == tstate.StateId).ToArray();
                tstate.State.Outcomes.ForEach(obj =>
                {
                    obj.State = tstate.State;
                    obj.Outcome = repoFlowOutcome.Query(o => o.OutcomeId == obj.OutcomeId).FirstOrDefault();
                });
            }
            tstate.FromTargetStates = repoNextTargetState.Query(o => o.NextTargetStateId == tstate.TargetStateId).ToList();
            tstate.FromTargetStates.ForEach(obj =>
            {
                obj.ToTargetState = tstate;
                obj.FromTargetState = repoTargetState.Query(o => o.TargetStateId == obj.TargetStateId).FirstOrDefault();
            });
            tstate.ToTargetStates = repoNextTargetState.Query(o => o.TargetStateId == tstate.TargetStateId).ToList();
            tstate.ToTargetStates.ForEach(obj =>
            {
                obj.FromTargetState = tstate;
                obj.ToTargetState = repoTargetState.Query(o => o.TargetStateId == obj.NextTargetStateId).FirstOrDefault();
            });
            tstate.TargetIncomes = repoTargetIncome.Query(o => o.TargetStateId == tstate.TargetStateId).ToList();
            tstate.TargetOutcomes = repoTargetOutcome.Query(o => o.TargetStateId == tstate.TargetStateId).ToList();
            if (!string.IsNullOrEmpty(tstate.OperationId))
                tstate.Operation = repoFlowOp.Query(o => o.OperationId == tstate.OperationId).FirstOrDefault();
        }
        */

        /*
        public IEnumerable<TargetState> GetCurrentStates(string targetFlowId, bool loadAllInfo = true)
        {
            var tflow = LoadFlow(targetFlowId, loadAllInfo);
            if (tflow == null)
                throw new NullReferenceException(string.Format("Target Flow {0} not found!", targetFlowId));

            var tstates = repoTargetState.Query(o => o.TargetFlowId == tflow.TargetFlowId && o.StateStatus == StateStatus.None).OrderByDescending(o => o.OperateTime).ToArray();
            if (tstates == null || tstates.Count() <= 0)
                tstates = repoTargetState.Query(o => o.TargetFlowId == tflow.TargetFlowId && o.State.StateType == StateType.End).ToArray();
            if (tstates == null || tstates.Count() <= 0)
                tstates = repoTargetState.Query(o => o.TargetFlowId == tflow.TargetFlowId && o.State.StateType == StateType.Begin).ToArray();

            if (loadAllInfo)
            {
                tstates.ForEach(o => LoadTargetStateAllInfo(o));
            }

            return tstates;
        }

        public IEnumerable<FlowOperation> GetNextOperations(string targetFlowId)
        {
            var tStates = GetCurrentStates(targetFlowId, false);
            List<FlowOperation> operations = new List<FlowOperation>();
            TargetState paralleTargetState = null;
            foreach(var tstate in tStates)
            {
                switch(tstate.StateStatus)
                {
                    case StateStatus.Started:
                        var ops = GetStateOperations(tstate.StateId);
                        operations.AddRange(ops);
                        break;
                    case StateStatus.Finished:
                        break;
                    case StateStatus.None:
                        var state = repoFlowState.Query(o => o.StateId == tstate.StateId).FirstOrDefault();
                        if(state.StateType == StateType.ParallelStop)
                        {
                            paralleTargetState = tstate;
                            if (!state.AllParallelStateShouldBeEnd.HasValue || !state.AllParallelStateShouldBeEnd.Value)
                            {
                                var currOps = GetStateOperations(tstate.StateId);
                                operations.AddRange(currOps);
                            }
                        }
                        break;
                }
            }
            // 添加未开始执行的并行操作
            if (paralleTargetState != null)
            {
                var nts = repoNextTargetState.Query(o => o.NextTargetStateId == paralleTargetState.TargetStateId).FirstOrDefault();
                var beginParallelTState = repoTargetState.Query(o => o.TargetStateId == nts.ParallelTargetStateId).FirstOrDefault();
                var beginParallelOpids = repoStateOp.Query(o => o.StateId == beginParallelTState.StateId).Select(o => o.OperationId);
                var startedOpids = repoTargetState.Query(o => o.TargetFlowId == paralleTargetState.TargetFlowId && beginParallelOpids.Contains(o.OperationId)).Select(o => o.OperationId);
                var notStartedOpids = beginParallelOpids.Except(startedOpids);
                var notStartedOps = repoFlowOp.Query(o => notStartedOpids.Contains(o.OperationId)).ToArray();
                operations.AddRange(notStartedOps);
            }

            return operations;
        }

        private IEnumerable<FlowOperation> GetStateOperations(string stateId)
        {
            var opids = repoStateOp.Query(o => o.StateId == stateId).Select(o => o.OperationId);
            var ops = repoFlowOp.Query(o => opids.Contains(o.OperationId)).ToArray();
            return ops;
        }
        */

        public TargetState StartFlow(StartFlowInfo info)
        {
            BizFlow flow = repoBizFlow.Query(o => o.ClientId == info.ClientId && (o.Code == info.FlowCodeOrTargetType || o.TargetType == info.FlowCodeOrTargetType)).FirstOrDefault();
            if (flow == null)
                throw new NullReferenceException(string.Format("Flow {0} not found!", info.FlowCodeOrTargetType));

            TargetFlow tflow = repoTargetFlow.Query(o => o.FlowId == flow.FlowId && o.TargetId == info.TargetId).FirstOrDefault();
            if (tflow == null)
            {
                tflow = new TargetFlow()
                {
                    TargetFlowId = idGenerator.NewId(),
                    FlowId = flow.FlowId,
                    TargetId = info.TargetId,
                    FlowCode = info.TargetFlowCode,
                    Title = info.FlowTitle,
                    Message = info.FlowMessage,
                    HasStarted = false,
                    HasCompleted = false,
                    CreatorId = info.UserId,
                    CreatorName = info.UserName,
                    CreateTime = info.OperationTime
                };

                if (!string.IsNullOrEmpty(info.LastTargetFlowId))
                {
                    tflow.LastTargetFlowId = info.LastTargetFlowId;

                    NextTargetFlowCreatedMessage msg = new NextTargetFlowCreatedMessage()
                    {
                        FinishedTargetFlow = repoTargetFlow.Query(o => o.TargetFlowId == info.LastTargetFlowId).FirstOrDefault(),
                        CreatedTargetFlow = tflow,
                        OperateTime = info.OperationTime,
                        OperatorId = info.UserId,
                        OperatorName = info.UserName
                    };
                    try
                    {
                        MessageManager.Publish<NextTargetFlowCreatedMessage>(msg);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                }

                repoTargetFlow.Insert(tflow);
            }

            var startState = repoFlowState.Query(o => o.FlowId == tflow.FlowId && o.StateType == StateType.Begin).FirstOrDefault();
            if (startState == null)
                throw new NullReferenceException(string.Format("Flow {0} has no start state defined.", tflow.FlowId));

            TargetState tstate = new TargetState()
            {
                TargetStateId = idGenerator.NewId(),
                TargetFlowId = tflow.TargetFlowId,
                StateId = startState.StateId,
                Title = info.StartTitle,
                Message = info.StartMessage,
                StateStatus = StateStatus.Started,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };

            if (startState.IntervalType.HasValue)
            {
                if (startState.ResponseIntervalValue.HasValue)
                    tstate.ResponseExpiryTime = info.OperationTime.Interval(startState.IntervalType, startState.ResponseIntervalValue);
                if (startState.TreatIntervalValue.HasValue)
                    tstate.TreatExpiryTime = info.OperationTime.Interval(startState.IntervalType, startState.ResponseIntervalValue);
            }

            tflow.HasStarted = true;

            try
            {
                trans.BeginTransaction();
                repoTargetFlow.Update(tflow);
                repoTargetState.Insert(tstate);
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }

            TargetStateChangedMessage tscMsg = new TargetStateChangedMessage()
            {
                OldTargetStates = null,
                NewTargetState = tstate,
                OperationId = null,
                DataOperation = DataOperation.Insert,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };
            try
            {
                MessageManager.Publish<TargetStateChangedMessage>(tscMsg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            //tstate.State = startState;
            //tstate.TargetFlow = tflow;
            //tstate.TargetFlow.Flow = flow;
            return tstate;
        }

        public TargetState Plan(DoOperationInfo info)
        {
            var tflow = repoTargetFlow.Query(o => o.TargetFlowId == info.TargetFlowId).FirstOrDefault();
            var operation = repoFlowOp.Query(o => o.FlowId == tflow.FlowId && o.OperationId == info.OperationId).FirstOrDefault();
            if (operation == null)
                throw new NullReferenceException(string.Format("Flow operation {0} not found.", info.OperationId));

            if (operation.CanPlanned == false)
                throw new Exception(string.Format("The operation {0} cannot be planned.", operation.Name));

            TargetState tstate = new TargetState()
            {
                TargetStateId = idGenerator.NewId(),
                TargetFlowId = tflow.TargetFlowId,
                StateId = operation.DefaultNextStateId,
                Title = info.Title,
                Message = info.Message,
                StateStatus = StateStatus.Planned,
                PlanTreatTime = info.OperationTime,
                PlannerId = info.UserId,
                PlannerName = info.UserName,
            };

            NextTargetState nts = new NextTargetState()
            {
                TargetStateId = info.TargetStateId,
                NextTargetStateId = tstate.TargetStateId
            };

            try
            {
                trans.BeginTransaction();
                repoTargetState.Insert(tstate);
                repoNextTargetState.Insert(nts);
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }

            TargetStateChangedMessage msg = new TargetStateChangedMessage()
            {
                OldTargetStates = null,
                NewTargetState = tstate,
                OperationId = null,
                DataOperation = DataOperation.Insert,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };
            try
            {
                MessageManager.Publish<TargetStateChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return tstate;
        }

        public TargetState Response(ResponseInfo info)
        {
            var tstate = repoTargetState.Query(o => o.TargetStateId == info.TargetStateId).FirstOrDefault();
            if (tstate == null)
                throw new ArgumentNullException(string.Format("Flow State {0} not found!", info.TargetStateId));

            tstate.ResponsorId = info.UserId;
            tstate.ResponsorName = info.UserName;
            tstate.ResponseTime = info.OperationTime;
            tstate.StateStatus = StateStatus.Responsed;

            repoTargetState.Update(tstate);
            TargetStateChangedMessage msg = new TargetStateChangedMessage()
            {
                OldTargetStates = new TargetState[] { tstate },
                NewTargetState = null,
                OperationId = null,
                DataOperation = DataOperation.Update,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };
            try
            {
                MessageManager.Publish<TargetStateChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            return tstate;
        }

        public TargetState Do(DoOperationInfo info)
        {
            var tflow = repoTargetFlow.Query(o => o.TargetFlowId == info.TargetFlowId).FirstOrDefault();
            if (tflow == null)
                throw new NullReferenceException(string.Format("Target flow {0} not found.", info.TargetFlowId));
            var flow = repoBizFlow.Query(o => o.FlowId == tflow.FlowId).FirstOrDefault();
            if (flow == null)
                throw new NullReferenceException(string.Format("Flow {0} not found.", tflow.FlowId));

            var fop = repoFlowOp.Query(o => o.OperationId == info.OperationId).FirstOrDefault();
            if (fop == null)
                throw new ArgumentNullException(string.Format("Flow operation {0} is not found.", info.OperationId));

                        FlowState nextState = null;
            if (!string.IsNullOrEmpty(info.NextStateIdOrCode))
                nextState = repoFlowState.Query(o => o.FlowId == flow.FlowId && (o.StateId == info.NextStateIdOrCode || o.Code == info.NextStateIdOrCode)).FirstOrDefault();

            if (nextState == null)
                nextState = repoFlowState.Query(o => o.StateId == fop.DefaultNextStateId).FirstOrDefault();
            if (nextState == null)
                throw new NullReferenceException(string.Format("Operation {0} doesn't define next state.", fop.OperationId));

            List<TargetState> needUpdatedTargetStates = new List<TargetState>();
            if(!string.IsNullOrEmpty(info.TargetStateId))
            {
                var tstate = repoTargetState.Query(o => o.TargetStateId == info.TargetStateId).FirstOrDefault();
                needUpdatedTargetStates.Add(tstate);
            }

            // 当前状态为并行结束结点，需要判断当前并行点的所有操作是否都已经完成。
            if (nextState.StateType == StateType.ParallelEnd)
            {
                bool allShouldDone = nextState.AllParallelStateShouldBeEnd.HasValue && nextState.AllParallelStateShouldBeEnd.Value;
                bool canExecute = true;
                var ts = repoTargetState.Query(o => o.TargetFlowId == tflow.TargetFlowId && o.State.StateType == StateType.ParallelStop && (int)o.StateStatus < (int)StateStatus.TreatedNormal).ToArray();
                foreach (var t in ts)
                {
                    var prevStateHasThisOperation = repoStateOp.Query(o => o.StateId == t.StateId && o.OperationId == fop.OperationId).Any();
                    if (allShouldDone)
                        canExecute = canExecute && prevStateHasThisOperation;
                    else
                        canExecute = prevStateHasThisOperation;
                }
                    
                // 必须所有并行的操作都达到下一状态（并行结束ParallelEnd结点），才可以进行后续的操作。
                if (canExecute)
                {
                    // 其他操作中，所有其他操作都已执行且执行后的状态都允许进行后续的操作。即本次操作是并行操作中的最后一个操作。
                    needUpdatedTargetStates.AddRange(ts);
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Target flow {0} is waiting for finishing parallel operations.", info.TargetFlowId));
                }
            }

            List<NextTargetState> nextTargetStates = new List<NextTargetState>();
            string nextTargetStateId = idGenerator.NewId();

            foreach (var tstate in needUpdatedTargetStates)
            {
                var state = repoFlowState.Query(o => o.StateId == tstate.StateId).FirstOrDefault();
                if (state == null)
                    throw new NullReferenceException(string.Format("State {0} not found.", tstate.StateId));

                StateStatus stateStatus = StateStatus.TreatedNormal;
                var sincomes = repoStateIncome.Query(o => o.StateId == tstate.StateId && o.IsRequired == true);
                if (sincomes.Any())
                {
                    var cincomes = repoTargetIncome.Query(o => o.TargetStateId == info.TargetStateId && sincomes.Any(d => d.IncomeId == o.IncomeId) && o.FileStatus != FileStatus.InValid);
                    if (sincomes.Count() > cincomes.Count())
                    {
                        if (flow.StopWhenIncomeRequired)
                        {
                            throw new ApplicationException("Some required incomes are not offered.");
                        }
                        else
                        {
                            stateStatus = StateStatus.TreatedError;
                        }
                    }
                    else
                    {
                        if (cincomes.Any(o => o.FileStatus == FileStatus.Draft))
                            stateStatus = StateStatus.TreatedWarn;
                    }
                }
                var soutcomes = repoStateOutcome.Query(o => o.StateId == tstate.StateId && o.IsRequired == true);
                if (soutcomes.Any())
                {
                    var coutcomes = repoTargetOutcome.Query(o => o.TargetStateId == tstate.TargetStateId && soutcomes.Any(d => d.OutcomeId == o.OutcomeId) && o.FileStatus != FileStatus.InValid);
                    if (soutcomes.Count() > coutcomes.Count())
                    {
                        if (flow.StopWhenIncomeRequired)
                        {
                            throw new ApplicationException("Some required outcomes are not offered.");
                        }
                        else
                        {
                            stateStatus = StateStatus.TreatedError;
                        }
                    }
                    else
                    {
                        if (coutcomes.Any(o => o.FileStatus == FileStatus.Draft))
                            stateStatus = StateStatus.TreatedWarn;
                    }
                }


                tstate.TreatTime = info.OperationTime;
                tstate.StateStatus = stateStatus;
                tstate.TreaterId = info.UserId;
                tstate.TreaterName = info.UserName;

                var nts = repoNextTargetState.Query(o => o.TargetStateId == tstate.TargetStateId && o.ToTargetState.StateId == nextState.StateId).FirstOrDefault();
                if (nts == null)
                {
                    nts = new NextTargetState()
                    {
                        TargetStateId = tstate.TargetStateId,
                        NextTargetStateId = nextTargetStateId
                    };
                }
                else
                {
                    nextTargetStateId = nts.NextTargetStateId;
                }

                if (state.StateType == StateType.ParallelBegin)
                {
                    nts.ParallelTargetStateId = tstate.TargetStateId;
                }
                else if (state.StateType == StateType.ParallelStop || state.StateType == StateType.ParallelEnd)
                {
                    var lastnts = repoNextTargetState.Query(o => o.NextTargetStateId == tstate.TargetStateId).FirstOrDefault();
                    nts.ParallelTargetStateId = lastnts.ParallelTargetStateId;
                }
                nextTargetStates.Add(nts);
            }

            var nextTargetState = repoTargetState.Query(o => nextTargetStateId == o.TargetStateId && o.StateId == nextState.StateId).FirstOrDefault();
            if (nextTargetState == null)
            {
                nextTargetState = new TargetState()
                {
                    TargetStateId = nextTargetStateId,
                    TargetFlowId = tflow.TargetFlowId,
                    StateId = nextState.StateId,
                };
            }

            nextTargetState.Title = info.Title;
            nextTargetState.Message = info.Message;
            nextTargetState.OperateTime = info.OperationTime;
            nextTargetState.StateStatus = StateStatus.None;
            nextTargetState.OperationId = info.OperationId;
            nextTargetState.OperatorId = info.UserId;
            nextTargetState.OperatorName = info.UserName;

            if (nextState.IntervalType.HasValue)
            {
                if (nextState.ResponseIntervalValue.HasValue)
                    nextTargetState.ResponseExpiryTime = info.OperationTime.Interval(nextState.IntervalType, nextState.ResponseIntervalValue);
                if (nextState.TreatIntervalValue.HasValue)
                    nextTargetState.TreatExpiryTime = info.OperationTime.Interval(nextState.IntervalType, nextState.TreatIntervalValue);
            }
            try
            {
                trans.BeginTransaction();
                if (nextState.StateType == StateType.End)
                {
                    nextTargetState.StateStatus = StateStatus.TreatedNormal;
                    tflow.HasCompleted = true;
                    tflow.Result = nextState.Result;
                    repoTargetFlow.Update(tflow);
                }
                needUpdatedTargetStates.ForEach(o => repoTargetState.Update(o));
                repoTargetState.Insert(nextTargetState);
                nextTargetStates.ForEach(o => repoNextTargetState.Save(t => t.TargetStateId == o.TargetStateId && t.NextTargetStateId == o.NextTargetStateId,o));
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            TargetStateChangedMessage msg = new TargetStateChangedMessage()
            {
                OldTargetStates = needUpdatedTargetStates,
                NewTargetState = nextTargetState,
                OperationId = fop.OperationId,
                DataOperation = DataOperation.Update,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };

            try
            {
                MessageManager.Publish<TargetStateChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return nextTargetState;
        }

        public bool Cancel(DoOperationInfo info)
        {
            var tstate = repoTargetState.Query(o => o.TargetStateId == info.TargetStateId).FirstOrDefault();
            if (tstate == null)
                throw new NullReferenceException(string.Format("Target state {0} not found.", info.TargetStateId));
            var operation = repoFlowOp.Query(o => o.OperationId == info.OperationId).FirstOrDefault();
            if (operation == null)
                throw new NullReferenceException(string.Format("Flow operation {0} not found.", info.OperationId));

            if (operation.CanCancelled == false)
                throw new Exception(string.Format("The operation {0} cannot be cancelled.", operation.Name));

            var state = repoFlowState.Query(o => o.StateId == tstate.StateId).FirstOrDefault();
            if (state == null)
                throw new NullReferenceException(string.Format("Flow state {0} not found.", tstate.StateId));

            if (state.StateType == StateType.Begin && !string.IsNullOrEmpty(tstate.OperatorId) && tstate.OperatorId != info.UserId)
                throw new Exception(string.Format("The state {0} can only be cancelled by the operator {1}.", state.Name, tstate.OperatorName));
            else if (!string.IsNullOrEmpty(tstate.TreaterId) && tstate.TreaterId != info.UserId)
                throw new Exception(string.Format("The state {0} can only be cancelled by the operator {1}.", state.Name, tstate.TreaterName));

            var tflow = repoTargetFlow.Query(o => o.TargetFlowId == tstate.TargetFlowId).First();

            if (tstate.StateStatus == StateStatus.Started)
            {
                try
                {
                    trans.BeginTransaction();
                    tflow.HasStarted = false;
                    repoTargetFlow.Update(tflow);
                    repoTargetIncome.DeleteBatch(o => o.TargetStateId == tstate.TargetStateId);
                    repoTargetOutcome.DeleteBatch(o => o.TargetStateId == tstate.TargetStateId);
                    repoTargetState.Delete(tstate);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            else
            {
                var lnts = repoNextTargetState.Query(o => o.NextTargetStateId == tstate.TargetStateId).Select(o => o.TargetStateId);
                var lastTStates = repoTargetState.Query(o => lnts.Contains(o.TargetStateId)).Select(o => o.TargetStateId);
                try
                {
                    trans.BeginTransaction();
                    tflow.HasCompleted = false;
                    tflow.Result = null;
                    repoTargetFlow.Update(tflow);
                    repoTargetIncome.DeleteBatch(o => o.TargetStateId == tstate.TargetStateId);
                    repoTargetOutcome.DeleteBatch(o => o.TargetStateId == tstate.TargetStateId);

                    repoTargetState.UpdateBatch(o => lastTStates.Contains(o.TargetStateId), o => new TargetState()
                    {
                        StateStatus = StateStatus.None,
                        TreaterId = null,
                        TreaterName = null,
                        TreatTime = null
                    });

                    repoNextTargetState.DeleteBatch(o => o.NextTargetStateId == tstate.TargetStateId);
                    repoTargetState.Delete(tstate);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            TargetStateChangedMessage msg = new TargetStateChangedMessage()
            {
                OldTargetStates = new TargetState[] { tstate },
                NewTargetState = null,
                OperationId = null,
                DataOperation = DataOperation.Delete,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = state.StateType == StateType.Begin ? tstate.OperatorName : tstate.TreaterName
            };

            try
            {
                MessageManager.Publish<TargetStateChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return true;
        }

        public bool UploadIncome(UploadInfo info)
        {
            if (string.IsNullOrEmpty(info.FileUrl))
                throw new Exception("no file provided!");
            var targetState = repoTargetState.Query(o => o.TargetStateId == info.TargetStateId).FirstOrDefault();
            if (targetState == null)
                throw new ArgumentNullException(string.Format("Target with state {0} not found!", info.TargetStateId));
            var tobj = repoTargetIncome.Query(o => o.TargetStateId == info.TargetStateId && o.IncomeId == info.UploadId).FirstOrDefault();
            if (tobj == null)
            {
                var obj = repoFlowIncome.Query(o => o.IncomeId == info.UploadId).FirstOrDefault();
                if (obj == null)
                    throw new ArgumentNullException(string.Format("Income width {0} in state {1} not defined!", info.UploadId, targetState.StateId));

                tobj = new TargetIncome()
                {
                    TargetIncomeId = idGenerator.NewId(),
                    TargetStateId = info.TargetStateId,
                    IncomeId = info.UploadId,
                    Name = obj.Name,
                    Remark = info.Remark,
                    UploadTime = info.OperationTime,
                    FileType = obj.FileType,
                    FileUrl = info.FileUrl,
                    FileStatus = FileStatus.Draft,
                    UploaderId = info.UserId,
                    UploaderName = info.UserName
                };

                bool result = repoTargetIncome.Insert(tobj);

                TargetIncomeChangedMessage msg = new TargetIncomeChangedMessage()
                {
                    TargetIncome = tobj,
                    DataOperation = DataOperation.Insert,
                    OperateTime = info.OperationTime,
                    OperatorId = info.UserId,
                    OperatorName = info.UserName
                };
                try
                {
                    MessageManager.Publish<TargetIncomeChangedMessage>(msg);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                return result;
            }
            else
            {
                if (!tobj.Verified.HasValue || !tobj.Verified.Value)
                {
                    tobj.Remark = info.Remark;
                    tobj.FileUrl = info.FileUrl;
                    tobj.UploadTime = info.OperationTime;
                    tobj.Remark = tobj.Remark + Environment.NewLine + info.Remark;

                    tobj.UploaderId = info.UserId;
                    tobj.UploaderName = info.UserName;
                    bool result = repoTargetIncome.Update(tobj);

                    TargetIncomeChangedMessage msg = new TargetIncomeChangedMessage()
                    {
                        TargetIncome = tobj,
                        DataOperation = DataOperation.Insert,
                        OperateTime = info.OperationTime,
                        OperatorId = info.UserId,
                        OperatorName = info.UserName
                    };
                    try
                    {
                        MessageManager.Publish<TargetIncomeChangedMessage>(msg);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    return result;
                }
                else
                {
                    throw new ApplicationException(string.Format("Income {0} has been verified!", tobj.TargetIncomeId));
                }
            }
        }

        public bool VerifyIncome(UploadInfo info)
        {
            if (string.IsNullOrEmpty(info.FileUrl))
                throw new Exception("no file provided!");
            var targetState = repoTargetState.Query(o => o.TargetStateId == info.TargetStateId).FirstOrDefault();
            if (targetState == null)
                throw new ArgumentNullException(string.Format("Target with state {0} not found!", info.TargetStateId));
            var tobj = repoTargetIncome.Query(o => o.TargetStateId == info.TargetStateId && o.IncomeId == info.UploadId).FirstOrDefault();
            if (tobj == null)
                throw new ArgumentNullException(string.Format("Income {0} in state {1} not found!", info.UploadId, info.TargetStateId));

            tobj.Verified = info.Verified;
            tobj.Remark = tobj.Remark + Environment.NewLine + info.Remark;
            tobj.VerifierId = info.UserId;
            tobj.VerifierName = info.UserName;
            tobj.VerifierTime = info.OperationTime;
            tobj.FileStatus = info.Verified.Value ? FileStatus.Verified : FileStatus.InValid;

            bool result = repoTargetIncome.Update(tobj);

            TargetIncomeChangedMessage msg = new TargetIncomeChangedMessage()
            {
                TargetIncome = tobj,
                DataOperation = DataOperation.Update,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };
            try
            {
                MessageManager.Publish<TargetIncomeChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return result;
        }

        public bool RemoveIncome(UploadInfo info)
        {
            var tobj = repoTargetIncome.Query(o => o.TargetStateId == info.TargetStateId && o.IncomeId == info.UploadId).FirstOrDefault();
            if (tobj == null)
                throw new ArgumentNullException(string.Format("Target income {0} not found!", info.UploadId));

            bool result = repoTargetIncome.Delete(tobj);

            TargetIncomeChangedMessage msg = new TargetIncomeChangedMessage()
            {
                TargetIncome = tobj,
                DataOperation = DataOperation.Delete,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };
            try
            {
                MessageManager.Publish<TargetIncomeChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return result;
        }

        public bool UploadOutcome(UploadInfo info)
        {
            if (string.IsNullOrEmpty(info.FileUrl))
                throw new Exception("no file provided!");
            var targetState = repoTargetState.Query(o => o.TargetStateId == info.TargetStateId).FirstOrDefault();
            if (targetState == null)
                throw new ArgumentNullException(string.Format("Target with state {0} not found!", info.TargetStateId));
            var tobj = repoTargetOutcome.Query(o => o.TargetStateId == info.TargetStateId && o.OutcomeId == info.UploadId).FirstOrDefault();
            if (tobj == null)
            {
                var obj = repoFlowOutcome.Query(o => o.OutcomeId == info.UploadId).FirstOrDefault();
                if (obj == null)
                    throw new ArgumentNullException(string.Format("Outcome width {0} in state {1} not defined!", info.UploadId, targetState.StateId));

                tobj = new TargetOutcome()
                {
                    TargetOutcomeId = idGenerator.NewId(),
                    TargetStateId = info.TargetStateId,
                    OutcomeId = info.UploadId,
                    Name = obj.Name,
                    Remark = info.Remark,
                    UploadTime = info.OperationTime,
                    FileType = obj.FileType,
                    FileUrl = info.FileUrl,
                    FileStatus = FileStatus.Draft,
                    UploaderId = info.UserId,
                    UploaderName = info.UserName
                };

                bool result = repoTargetOutcome.Insert(tobj);

                TargetStateChangedMessage msg = new TargetStateChangedMessage()
                {
                    OldTargetStates = new TargetState[] { targetState },
                    NewTargetState = null,
                    OperationId = null,
                    DataOperation = DataOperation.Update,
                    OperateTime = info.OperationTime,
                    OperatorId = info.UserId,
                    OperatorName = info.UserName
                };
                try
                {
                    MessageManager.Publish<TargetStateChangedMessage>(msg);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                return result;
            }
            else
            {
                if (!tobj.Verified.HasValue || !tobj.Verified.Value)
                {
                    tobj.Remark = info.Remark;
                    tobj.FileUrl = info.FileUrl;
                    tobj.UploadTime = info.OperationTime;
                    tobj.Remark = tobj.Remark + Environment.NewLine + info.Remark;

                    tobj.UploaderId = info.UserId;
                    tobj.UploaderName = info.UserName;
                    bool result = repoTargetOutcome.Update(tobj);

                    TargetOutcomeChangedMessage msg = new TargetOutcomeChangedMessage()
                    {
                        TargetOutcome = tobj,
                        DataOperation = DataOperation.Insert,
                        OperateTime = info.OperationTime,
                        OperatorId = info.UserId,
                        OperatorName = info.UserName
                    };
                    try
                    {
                        MessageManager.Publish<TargetOutcomeChangedMessage>(msg);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    return result;
                }
                else
                {
                    throw new ApplicationException(string.Format("Outcome {0} has been verified!", tobj.TargetOutcomeId));
                }
            }
        }

        public bool VerifyOutcome(UploadInfo info)
        {
            if (string.IsNullOrEmpty(info.FileUrl))
                throw new Exception("no file provided!");
            var targetState = repoTargetState.Query(o => o.TargetStateId == info.TargetStateId).FirstOrDefault();
            if (targetState == null)
                throw new ArgumentNullException(string.Format("Target with state {0} not found!", info.TargetStateId));
            var tobj = repoTargetOutcome.Query(o => o.TargetStateId == info.TargetStateId && o.OutcomeId == info.UploadId).FirstOrDefault();
            if (tobj == null)
                throw new ArgumentNullException(string.Format("Outcome {0} in state {1} not found!", info.UploadId, info.TargetStateId));

            tobj.Verified = info.Verified;
            tobj.Remark = tobj.Remark + Environment.NewLine + info.Remark;
            tobj.VerifierId = info.UserId;
            tobj.VerifierName = info.UserName;
            tobj.VerifierTime = info.OperationTime;
            tobj.FileStatus = info.Verified.Value ? FileStatus.Verified : FileStatus.InValid;

            bool result = repoTargetOutcome.Update(tobj);

            TargetOutcomeChangedMessage msg = new TargetOutcomeChangedMessage()
            {
                TargetOutcome = tobj,
                DataOperation = DataOperation.Update,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };
            try
            {
                MessageManager.Publish<TargetOutcomeChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return result;
        }

        public bool RemoveOutcome(UploadInfo info)
        {
            var tobj = repoTargetOutcome.Query(o => o.TargetStateId == info.TargetStateId && o.OutcomeId == info.UploadId).FirstOrDefault();
            if (tobj == null)
                throw new ArgumentNullException(string.Format("Target outcome {0} not found!", info.UploadId));

            bool result = repoTargetOutcome.Delete(tobj);

            TargetOutcomeChangedMessage msg = new TargetOutcomeChangedMessage()
            {
                TargetOutcome = tobj,
                DataOperation = DataOperation.Delete,
                OperateTime = info.OperationTime,
                OperatorId = info.UserId,
                OperatorName = info.UserName
            };
            try
            {
                MessageManager.Publish<TargetOutcomeChangedMessage>(msg);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return result;
        }

        public ILogger Logger { get; set; }
    }
}
