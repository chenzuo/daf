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
using DAF.Workflow.Models;
using DAF.Workflow.Messages;

namespace DAF.Workflow
{
    public class RepositoryStateFlowManager : IStateFlowManager
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

        public RepositoryStateFlowManager(
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

        public IQueryable<BizFlow> GetFlows(Expression<Func<BizFlow, bool>> predicate)
        {
            return repoBizFlow.Query(predicate);
        }

        public BizFlow GetFlow(Expression<Func<BizFlow, bool>> predicate, bool loadAllInfo = true)
        {
            BizFlow flow = repoBizFlow.Query(predicate).FirstOrDefault();
            if (flow == null)
                throw new ArgumentNullException("Flow not defined!");

            if (loadAllInfo)
            {
                flow.States = repoFlowState.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
                flow.Operations = repoFlowOp.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
                flow.Incomes = repoFlowIncome.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
                flow.Outcomes = repoFlowOutcome.Query(o => o.FlowId == flow.FlowId).OrderBy(o => o.Code).ToArray();
                flow.NextBizFlows = repoNextFlow.Query(o => o.FlowId == flow.FlowId).ToArray();

                var stateOps = repoStateOp.Query(o => o.State.FlowId == flow.FlowId).ToArray();
                var stateIncomes = repoStateIncome.Query(o => o.State.FlowId == flow.FlowId).ToArray();
                var stateOutcomes = repoStateOutcome.Query(o => o.State.FlowId == flow.FlowId).ToArray();

                flow.States.ForEach(it =>
                {
                    it.Operations = stateOps.Where(op => op.StateId == it.StateId).ToArray();
                    it.Incomes = stateIncomes.Where(op => op.StateId == it.StateId).ToArray();
                    it.Outcomes = stateOutcomes.Where(op => op.StateId == it.StateId).ToArray();

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

            return flow;
        }

        public bool SaveFlow(BizFlow flow)
        {
            try
            {
                string flowId = flow.FlowId;
                IEnumerable<FlowState> states = flow.States;
                if (states == null)
                    states = Enumerable.Empty<FlowState>();
                IEnumerable<FlowOperation> operations = flow.Operations;
                if (operations == null)
                    operations = Enumerable.Empty<FlowOperation>();
                IEnumerable<FlowIncome> incomes = flow.Incomes;
                if (incomes == null)
                    incomes = Enumerable.Empty<FlowIncome>();
                IEnumerable<FlowOutcome> outcomes = flow.Outcomes;
                if (outcomes == null)
                    outcomes = Enumerable.Empty<FlowOutcome>();
                IEnumerable<NextBizFlow> nfs = flow.NextBizFlows;
                if (nfs == null)
                    nfs = Enumerable.Empty<NextBizFlow>();

                trans.BeginTransaction();

                // save states
                states.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.StateId))
                        o.StateId = idGenerator.NewId();
                });

                var ostates = repoFlowState.Query(o => o.FlowId == flowId).ToArray();
                repoFlowState.SaveAll(trans, ostates, states, (o, c) => o.StateId == c.StateId, false);

                // save operations
                operations.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.OperationId))
                        o.OperationId = idGenerator.NewId();
                });

                var oops = repoFlowOp.Query(o => o.FlowId == flowId).ToArray();
                repoFlowOp.SaveAll(trans, oops, operations, (o, c) => o.OperationId == c.OperationId, true);

                // save incomes
                incomes.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.IncomeId))
                        o.IncomeId = idGenerator.NewId();
                });

                var oincomes = repoFlowIncome.Query(o => o.FlowId == flowId).ToArray();
                repoFlowIncome.SaveAll(trans, oincomes, incomes, (o, c) => o.IncomeId == c.IncomeId, true);

                // save outcomes
                outcomes.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.OutcomeId))
                        o.OutcomeId = idGenerator.NewId();
                });

                var ooutcomes = repoFlowOutcome.Query(o => o.FlowId == flowId).ToArray();
                repoFlowOutcome.SaveAll(trans, ooutcomes, outcomes, (o, c) => o.OutcomeId == c.OutcomeId, true);

                // save next flows
                var onfs = repoNextFlow.Query(o => o.FlowId == flowId).ToArray();
                repoNextFlow.SaveAll(trans, onfs, nfs, (o, c) => o.FlowId == c.FlowId && o.NextFlowId == c.NextFlowId,
                    (repo, o) => { repo.Insert(o); },
                    null,
                    (repo, o) => { repo.Delete(o); });

                // save state incomes outcomes operations
                List<FlowStateIncome> newIncomes = new List<FlowStateIncome>();
                List<FlowStateOutcome> newOutcomes = new List<FlowStateOutcome>();
                List<FlowStateOperation> newOps = new List<FlowStateOperation>();
                states.ForEach(o =>
                {
                    if (o.Incomes != null && o.Incomes.Count > 0)
                        newIncomes.AddRange(o.Incomes);
                    if (o.Outcomes != null && o.Outcomes.Count > 0)
                        newOutcomes.AddRange(o.Outcomes);
                    if (o.Operations != null && o.Operations.Count > 0)
                        newOps.AddRange(o.Operations);
                });
                newIncomes.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.IncomeId))
                        o.IncomeId = idGenerator.NewId();
                });
                newOutcomes.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.OutcomeId))
                        o.OutcomeId = idGenerator.NewId();
                });
                newOps.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.OperationId))
                        o.OperationId = idGenerator.NewId();
                });

                var sincomes = repoStateIncome.Query(o => o.State.FlowId == flowId).ToArray();
                repoStateIncome.SaveAll(trans, sincomes, newIncomes, (o, c) => o.IncomeId == c.IncomeId && o.StateId == c.StateId, true);
                var soutcomes = repoStateOutcome.Query(o => o.State.FlowId == flowId).ToArray();
                repoStateOutcome.SaveAll(trans, soutcomes, newOutcomes, (o, c) => o.OutcomeId == c.OutcomeId && o.StateId == c.StateId, true);
                var ops = repoStateOp.Query(o => o.State.FlowId == flowId).ToArray();
                repoStateOp.SaveAll(trans, ops, newOps, (o, c) => o.OperationId == c.OperationId && o.StateId == c.StateId,
                    (repo, o) => { repo.Insert(o); },
                    null,
                    (repo, o) => { repo.Delete(o); });

                var deleteStates = ostates.Where(o => !states.Any(it => it.StateId == o.StateId));
                deleteStates.ForEach(o => repoFlowState.Delete(o));

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }

        public bool SaveFlows(string clientId, string owner, string biz, IEnumerable<BizFlow> flows)
        {
            try
            {
                var query = repoBizFlow.Query(null);
                if (!string.IsNullOrEmpty(clientId))
                    query = query.Where(o => o.ClientId == clientId);
                if (!string.IsNullOrEmpty(owner))
                    query = query.Where(o => o.Owner == owner);
                if (!string.IsNullOrEmpty(biz))
                    query = query.Where(o => o.BizGroup == biz);

                flows.ForEach(o =>
                {
                    if (string.IsNullOrEmpty(o.FlowId))
                        o.FlowId = idGenerator.NewId();
                });

                bool deleteNotFound = !string.IsNullOrEmpty(owner) && !string.IsNullOrEmpty(biz);

                var oldObjs = query.ToArray();

                return repoBizFlow.SaveAll(trans, oldObjs, flows, (o, c) => o.FlowId == c.FlowId, deleteNotFound);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveFlows(BizFlow[] newItems, BizFlow[] modifiedItems, BizFlow[] deletedItems)
        {
            return repoBizFlow.SaveAll(trans, newItems, modifiedItems, deletedItems);
        }

        public bool VerifyFlow(string flowId, out string message)
        {
            bool verified = true;
            message = string.Empty;
            BizFlow flow = GetFlow(o => o.FlowId == flowId, true);
            if (flow.States.Count <= 0)
            {
                verified = false;
                message += string.Format("There is no states in the flow!{0}", Environment.NewLine);
            }
            var starts = flow.States.Where(o => o.StateType == StateType.Begin);
            if (starts == null || starts.Count() <= 0)
            {
                verified = false;
                message += string.Format("There is no start states in the flow! Need one start state. {0}", Environment.NewLine);
            }
            if (starts.Count() > 1)
            {
                verified = false;
                message += string.Format("There is more than one start states in the flow! Need one start state only. {0}", Environment.NewLine);
            }
            if (flow.States.Any(o => o.StateType == StateType.End && o.Result == null))
            {
                verified = false;
                message += string.Format("There are some End states without Result.{0}", Environment.NewLine);
            }
            if (flow.States.Any(o => o.StateType == StateType.End) == false)
            {
                verified = false;
                message += string.Format("There is no finish states in the flow! At least one finish state needed.{0}", Environment.NewLine);
            }

            return verified;
        }

        public FlowState GetState(string stateId, bool loadAllInfo = true)
        {
            FlowState state = repoFlowState.Query(o => o.StateId == stateId).FirstOrDefault();
            if (state == null)
                throw new ArgumentNullException(string.Format("Flow state with Id {0} not defined!", stateId));

            if (loadAllInfo)
            {
                state.Flow = GetFlow(o => o.FlowId == state.FlowId, false);
                state.Operations = repoStateOp.Query(o => o.StateId == stateId).OrderBy(o => o.Operation.Code).ToArray();
                state.Incomes = repoStateIncome.Query(o => o.StateId == stateId).OrderBy(o => o.Income.Code).ToArray();
                state.Outcomes = repoStateOutcome.Query(o => o.StateId == stateId).OrderBy(o => o.Outcome.Code).ToArray();
            }

            return state;
        }

        public FlowOperation GetOperation(string operationId)
        {
            var op = repoFlowOp.Query(o => o.OperationId == operationId).FirstOrDefault();
            if (op == null)
                throw new ArgumentNullException(string.Format("Flow operation with Id {0} not defined!", operationId));
            return op;
        }

        public FlowIncome GetIncome(string incomeId)
        {
            var obj = repoFlowIncome.Query(o => o.IncomeId == incomeId).FirstOrDefault();
            if (obj == null)
                throw new ArgumentNullException(string.Format("Flow income with Id {0} not defined!", incomeId));

            return obj;
        }

        public FlowOutcome GetOutcome(string outcomeId)
        {
            var obj = repoFlowOutcome.Query(o => o.OutcomeId == outcomeId).FirstOrDefault();
            if (obj == null)
                throw new ArgumentNullException(string.Format("Flow outcome with Id {0} not defined!", outcomeId));

            return obj;
        }

        public IQueryable<TargetFlow> LoadFlows()
        {
            return repoTargetFlow.Query(null);
        }

        public TargetFlow LoadFlow(string targetFlowId, bool loadAllInfo = true)
        {
            var tflow = repoTargetFlow.Query(o => o.TargetFlowId == targetFlowId).FirstOrDefault();
            if (tflow != null && loadAllInfo)
            {
                tflow.Flow = GetFlow(o => o.FlowId == tflow.FlowId, true);
                tflow.TreatedStates = repoTargetState.Query(o => o.TargetFlowId == targetFlowId).ToArray();

                var tstateIncomes = repoTargetIncome.Query(o => o.TargetState.TargetFlowId == targetFlowId).ToArray();
                var tstateOutcomes = repoTargetOutcome.Query(o => o.TargetState.TargetFlowId == targetFlowId).ToArray();

                tflow.TreatedStates.ForEach(o =>
                    {
                        o.TargetIncomes = tstateIncomes.Where(d => d.TargetStateId == o.TargetStateId).ToArray();
                        o.TargetOutcomes = tstateOutcomes.Where(d => d.TargetStateId == o.TargetStateId).ToArray();
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
                    tflow.Flow = GetFlow(o => o.FlowId == tflow.LastTargetFlowId, false);
                }
            }

            return tflow;
        }

        public TargetState GetTargetState(string targetFlowId, bool loadAllInfo = true)
        {
            var tstate = repoTargetState.Query(o => o.TargetFlowId == targetFlowId && o.StateStatus == StateStatus.None).OrderByDescending(o => o.OperateTime).FirstOrDefault();
            if (tstate == null)
                tstate = repoTargetState.Query(o => o.TargetFlowId == targetFlowId && o.State.StateType == StateType.End).FirstOrDefault();
            if (tstate == null)
                tstate = repoTargetState.Query(o => o.TargetFlowId == targetFlowId && o.State.StateType == StateType.Begin).FirstOrDefault();

            if (tstate != null && loadAllInfo)
            {
                tstate.TargetFlow = repoTargetFlow.Query(o => o.TargetFlowId == targetFlowId).FirstOrDefault();
                tstate.State = repoFlowState.Query(o => o.StateId == tstate.StateId).FirstOrDefault();
                if (tstate.State != null)
                {
                    var flowOps = repoFlowOp.Query(o => o.FlowId == tstate.State.FlowId).ToArray();
                    var flowIncomes = repoFlowIncome.Query(o => o.FlowId == tstate.State.FlowId).ToArray();
                    var flowOutcomes = repoFlowOutcome.Query(o => o.FlowId == tstate.State.FlowId).ToArray();

                    tstate.State.Operations = repoStateOp.Query(o => o.StateId == tstate.StateId).ToList();
                    tstate.State.Operations.ForEach(op =>
                    {
                        op.State = tstate.State;
                        op.Operation = flowOps.Where(o => o.OperationId == op.OperationId).FirstOrDefault();
                    });

                    tstate.State.Incomes = repoStateIncome.Query(o => o.StateId == tstate.StateId).ToList();
                    tstate.State.Incomes.ForEach(obj =>
                    {
                        obj.State = tstate.State;
                        obj.Income = flowIncomes.Where(o => o.IncomeId == obj.IncomeId).FirstOrDefault();
                    });

                    tstate.State.Outcomes = repoStateOutcome.Query(o => o.StateId == tstate.StateId).ToList();
                    tstate.State.Outcomes.ForEach(obj =>
                    {
                        obj.State = tstate.State;
                        obj.Outcome = flowOutcomes.Where(o => o.OutcomeId == obj.OutcomeId).FirstOrDefault();
                    });
                }
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

            return tstate;
        }

        public ILogger Logger { get; set; }
    }
}
