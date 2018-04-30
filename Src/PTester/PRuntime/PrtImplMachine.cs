﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace P.Runtime
{
    public enum PrtMachineStatus
    {
        Enabled,        // The state machine is enabled
        Blocked,        // The state machine is blocked on a dequeue or receive
        Halted,         // The state machine has halted
    };

    public enum PrtNextStatemachineOperation
    {
        ExecuteFunctionOperation,
        DequeueOperation,
        HandleEventOperation,
        ReceiveOperation
    };

    public enum PrtStateExitReason
    {
        NotExit,
        OnTransition,
        OnTransitionAfterExit,
        OnPopStatement,
        OnGotoStatement,
        OnUnhandledEvent
    };

    public enum PrtDequeueReturnStatus { SUCCESS, NULL, BLOCKED };

    public abstract class PrtImplMachine : PrtMachine
    {
        #region Fields
        public static int DefaultMaxBufferSize = int.MaxValue;
        public PrtEventBuffer eventQueue;
        public HashSet<PrtValue> receiveSet;
        public int maxBufferSize;
        public bool doAssume;
        public PrtInterfaceValue self;

        public static int k = 0;  // queue size bound (like maxBufferSize, but static). '0' means 'unbounded'

        #endregion

        #region Clone
        public PrtImplMachine Clone(StateImpl app)
        {
            var clonedMachine = MakeSkeleton();

            //base class fields
            clonedMachine.instanceNumber = this.instanceNumber;
            foreach (var fd in fields)
            {
                clonedMachine.fields.Add(fd.Clone());
            }
            clonedMachine.eventValue = this.eventValue;
            clonedMachine.stateStack = this.stateStack.Clone();
            clonedMachine.invertedFunStack = this.invertedFunStack.Clone();
            clonedMachine.continuation = this.continuation.Clone();
            clonedMachine.currentTrigger = this.currentTrigger;
            clonedMachine.currentPayload = this.currentPayload.Clone();

            clonedMachine.currentStatus = this.currentStatus;
            clonedMachine.nextSMOperation = this.nextSMOperation;
            clonedMachine.stateExitReason = this.stateExitReason;
            clonedMachine.sends = this.sends;
            clonedMachine.renamedName = this.renamedName;
            clonedMachine.isSafe = this.isSafe;
            clonedMachine.stateImpl = app;

            clonedMachine.destOfGoto = this.destOfGoto;

            //impl class fields
            clonedMachine.eventQueue = this.eventQueue.Clone();
            foreach (var ev in this.receiveSet)
            {
                clonedMachine.receiveSet.Add(ev);
            }
            clonedMachine.maxBufferSize = this.maxBufferSize;
            clonedMachine.doAssume = this.doAssume;
            clonedMachine.self = new PrtInterfaceValue(clonedMachine, this.self.permissions);


            return clonedMachine;
        }
        #endregion

        // ToString: use same recursive descent as for GetHashCode
        public override string ToString()
        {
            return base.ToString() + ";" + eventQueue.ToString();
        }

        public new string ToPrettyString(string indent = "") {
            string result = "";
            result += indent + "Base:\n";
            result += base.ToPrettyString(indent + "  ");
            result += indent + "Queue:\n";
            result += eventQueue.ToPrettyString(indent + "  ");
            return result; }

        #region Constructor
        public abstract PrtImplMachine MakeSkeleton();

        public PrtImplMachine() : base()
        {
            this.maxBufferSize = 0;
            this.doAssume = false;
            this.eventQueue = new PrtEventBuffer();
            this.receiveSet = new HashSet<PrtValue>();
        }

        public PrtImplMachine(StateImpl app, int maxBuff, bool assume) : base()
        {
            this.instanceNumber = this.NextInstanceNumber(app);
            this.eventQueue = new PrtEventBuffer();
            this.receiveSet = new HashSet<PrtValue>();
            this.maxBufferSize = maxBuff;
            this.doAssume = assume;
            this.stateImpl = app;
            this.self = new PrtInterfaceValue(this, new List<PrtEventValue>());
            //Push the start state function on the funStack.
            PrtPushState(StartState);
        }
        #endregion

        public override int GetHashCode()
        {
            var base_hash = base.GetHashCode();
            var evtQ_hash = eventQueue.GetHashCode();
            return Hashing.Hash(base_hash, evtQ_hash);
        }

        public override void Resolve(StateImpl state)
        {
            base.Resolve(state);
            eventQueue.Resolve(state);
        }

        public override void DbgCompare(PrtMachine machine)
        {
            base.DbgCompare(machine);
            Debug.Assert(eventQueue.GetHashCode() == (machine as PrtImplMachine).eventQueue.GetHashCode());
            Debug.Assert(GetHashCode() == machine.GetHashCode());
        }

        #region getters and setters
        public abstract int NextInstanceNumber(StateImpl app);
        #endregion

        #region State machine helper functions
        public void PrtResetTriggerAndPayload()
        {
            currentTrigger = PrtValue.@null;
            currentPayload = PrtValue.@null;
        }

        public override void PrtEnqueueEvent(PrtValue e, PrtValue arg, PrtMachine source, PrtMachineValue target = null)
        {
            PrtEventValue ev = e as PrtEventValue;
            if (ev.Equals(PrtValue.@null))
            {
                throw new PrtIllegalEnqueueException("Enqueued event must not be null");
            }

            //check if the sent event is in source send set
            if (source.sends != null && !source.sends.Contains(e as PrtEventValue))
            {
                throw new PrtIllegalEnqueueException(String.Format("Machine {0} sending event {1} not in its sends list", source.Name, e.ToString()));
            }

            //check if the sent event is in target permissions
            if (target is PrtInterfaceValue)
            {
                if ((target as PrtInterfaceValue).permissions != null && !(target as PrtInterfaceValue).permissions.Contains(e as PrtEventValue))
                {
                    throw new PrtIllegalEnqueueException(String.Format("Event {0} is not in the permission set of the target", e.ToString()));
                }
            }

            PrtType prtType = ev.evt.payloadType;

            //assertion to check if argument passed inhabits the payload type.
            if (prtType is PrtNullType)
            {
                if (!arg.Equals(PrtValue.@null))
                {
                    throw new PrtIllegalEnqueueException("Did not expect a payload value");
                }
            }
            else if (!PrtValue.PrtInhabitsType(arg, prtType))
            {
                throw new PrtInhabitsTypeException(String.Format("Payload <{0}> does not match the expected type <{1}> with event <{2}>", arg.ToString(), prtType.ToString(), ev.evt.name));
            }


            //add action to the trace
            if(StateImpl.visibleEvents.Contains(ev.evt.name))
            {
                stateImpl.currentVisibleTrace.AddAction(new Tuple<string, int>(target.mach.renamedName, target.mach.instanceNumber), ev, arg);
            }

            if (currentStatus == PrtMachineStatus.Halted)
            {
                stateImpl.TraceLine(
                    @"<EnqueueLog> Machine {0}-{1} has been halted and Event '{2}' is dropped",
                    this.Name, this.instanceNumber, ev.evt.name);
            }
            else
            {
                stateImpl.TraceLine(
                    @"<EnqueueLog> Enqueued Event <{0},{1}> in machine {2}-{3} by machine {4}-{5}",
                    ev.evt.name, arg.ToString(), this.Name, this.instanceNumber, source.Name, source.instanceNumber);

                // k-bounded queue semantics
                if (k > 0 && eventQueue.Size() == k)
                {
                    // Console.WriteLine("PrtImplMachine::PrtEnqueueEvent: warning: queue bound {0} reached in machine {1}; rejecting send event", k, Name);
                    throw new PrtAssumeFailureException();
                }
                else
                {
                    this.eventQueue.EnqueueEvent(e, arg, source.Name, source.CurrentState.name);
                    // Console.WriteLine("Queue size of machine {0} = {1}", Name, eventQueue.Size());
                    if (this.maxBufferSize != DefaultMaxBufferSize && this.eventQueue.Size() > this.maxBufferSize)
                    {
                        if (this.doAssume)
                        {
                            throw new PrtAssumeFailureException();
                        }
                        else
                        {
                            throw new PrtMaxBufferSizeExceededException(String.Format(@"<EXCEPTION> Event Buffer Size Exceeded {0} in Machine {1}-{2}", this.maxBufferSize, this.Name, this.instanceNumber));
                        }
                    }
                }
                if (currentStatus == PrtMachineStatus.Blocked && this.eventQueue.IsEnabled(this))
                {
                    currentStatus = PrtMachineStatus.Enabled;
                }
            }

            //Announce it to all the spec machines
            stateImpl.Announce(e as PrtEventValue, arg, source);
        }

        public PrtDequeueReturnStatus PrtDequeueEvent(bool hasNullTransition)
        {
            if (eventQueue.DequeueEvent(this))
            {
                if (currentStatus == PrtMachineStatus.Blocked)
                {
                    throw new PrtInternalException("Internal error: Tyring to execute blocked machine");
                }

                stateImpl.TraceLine(
                    "<DequeueLog> Dequeued Event <{0},{1}> at Machine {2}-{3}",
                    (currentTrigger as PrtEventValue).evt.name, currentPayload.ToString(), Name, instanceNumber);
                stateImpl.DequeueCallback?.Invoke(this, (currentTrigger as PrtEventValue).evt.name, currentTriggerSenderInfo.Item1, currentTriggerSenderInfo.Item2);

                receiveSet = new HashSet<PrtValue>();
                return PrtDequeueReturnStatus.SUCCESS;
            }
            else if (hasNullTransition || receiveSet.Contains(PrtValue.@null))
            {
                if (currentStatus == PrtMachineStatus.Blocked)
                {
                    throw new PrtInternalException("Internal error: Tyring to execute blocked machine");
                }
                stateImpl.TraceLine(
                    "<NullTransLog> Null transition taken by Machine {0}-{1}",
                    Name, instanceNumber);
                currentPayload = PrtValue.@null;
                currentTrigger = PrtValue.@null;
                receiveSet = new HashSet<PrtValue>();
                return PrtDequeueReturnStatus.NULL;
            }
            else if(CurrentActionSet.Contains(PrtValue.@null))
            {
                if (currentStatus == PrtMachineStatus.Blocked)
                {
                    throw new PrtInternalException("Internal error: Tyring to execute blocked machine");
                }
                stateImpl.TraceLine(
                    "<NullActionLog> Null action taken by Machine {0}-{1}",
                    Name, instanceNumber);
                currentPayload = PrtValue.@null;
                currentTrigger = PrtValue.@null;
                receiveSet = new HashSet<PrtValue>();
                return PrtDequeueReturnStatus.NULL;
            }
            else
            {
                if (currentStatus == PrtMachineStatus.Blocked)
                {
                    throw new PrtAssumeFailureException();
                }
                currentStatus = PrtMachineStatus.Blocked;
                if (stateImpl.Deadlock)
                {
                    throw new PrtDeadlockException("Deadlock detected");
                }
                return PrtDequeueReturnStatus.BLOCKED;
            }
        }

        public bool PrtIsPushTransitionPresent(PrtValue ev)
        {
            if (CurrentState.transitions.ContainsKey(ev))
                return CurrentState.transitions[ev].isPushTran;
            else
                return false;
        }

        public bool PrtHasNullReceiveCase()
        {
            return receiveSet.Contains(PrtValue.halt);
        }
        #endregion


        public void PrtRunStateMachine()
        {
            int numOfStepsTaken = 0;
            Debug.Assert(currentStatus == PrtMachineStatus.Enabled, "Invoked PrtRunStateMachine on a blocked or a halted machine");

            try
            {
                while (PrtStepStateMachine())
                {
                    if (    nextSMOperation == PrtNextStatemachineOperation.DequeueOperation
                         || nextSMOperation == PrtNextStatemachineOperation.ReceiveOperation) // changed: break if the next SMop dequeues. This prevents multiple dequeues from happening in one step
                    {
                        break;
                    }

                    if (numOfStepsTaken > 100000)
                    {
                        throw new PrtInfiniteRaiseLoop();
                    }
                    numOfStepsTaken++;
                }
            }
            catch (PrtException ex)
            {
                stateImpl.Exception = ex;
            }
        }

        public bool PrtStepStateMachine()
        {
            PrtValue currEventValue;
            PrtFun currAction;
            bool hasMoreWork = false;

            switch (nextSMOperation)
            {
                case PrtNextStatemachineOperation.ExecuteFunctionOperation:
                    goto DoExecuteFunction;
                case PrtNextStatemachineOperation.DequeueOperation:
                    goto DoDequeue;
                case PrtNextStatemachineOperation.HandleEventOperation:
                    goto DoHandleEvent;
                case PrtNextStatemachineOperation.ReceiveOperation:
                    goto DoReceive;
            }

            DoExecuteFunction:
            /*
             * Note that we have made an assumption that when a state is pushed on state stack or a transition is taken (update to a state)
             * the action set and deferred set is updated appropriately
             */
            if (invertedFunStack.TopOfStack == null)
            {
                stateImpl.TraceLine("<StateLog> Machine {0}-{1} entering State {2}", this.Name, this.instanceNumber, CurrentState.name);
                if (CurrentState.entryFun.IsAnonFun)
                    PrtPushFunStackFrame(CurrentState.entryFun, CurrentState.entryFun.CreateLocals(currentPayload));
                else
                    PrtPushFunStackFrame(CurrentState.entryFun, CurrentState.entryFun.CreateLocals());
            }
            //invoke the function
            invertedFunStack.TopOfStack.fun.Execute(stateImpl, this);
            goto CheckFunLastOperation;

            DoAction:
            currAction = PrtFindActionHandler(eventValue);
            if (currAction == PrtFun.IgnoreFun)
            {
                stateImpl.TraceLine("<ActionLog> Machine {0}-{1} ignoring Event '{2}' in State {3}", this.Name, this.instanceNumber, eventValue, CurrentState.name);
                PrtResetTriggerAndPayload();
                nextSMOperation = PrtNextStatemachineOperation.DequeueOperation;
                hasMoreWork = true;
                goto Finish;
            }
            else
            {
                if (invertedFunStack.TopOfStack == null)
                {
                    stateImpl.TraceLine("<ActionLog> Machine {0}-{1} executing action for Event '{2}' in State {3}", this.Name, this.instanceNumber, eventValue, CurrentState.name);
                    if (currAction.IsAnonFun)
                    {
                        PrtPushFunStackFrame(currAction, currAction.CreateLocals(currentPayload));
                    }
                    else
                    {
                        PrtPushFunStackFrame(currAction, currAction.CreateLocals());
                    }
                }
                //invoke the action handler
                invertedFunStack.TopOfStack.fun.Execute(stateImpl, this);
            }
            goto CheckFunLastOperation;

            CheckFunLastOperation:

            switch (continuation.reason)
            {
                case PrtContinuationReason.Pop:
                    {
                        //clear the fun stack on pop
                        invertedFunStack.Clear();
                        stateExitReason = PrtStateExitReason.OnPopStatement;
                        nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                        PrtPushExitFunction();
                        goto DoExecuteFunction;
                    }
                case PrtContinuationReason.Goto:
                    {
                        //clear fun stack on goto
                        invertedFunStack.Clear();
                        stateExitReason = PrtStateExitReason.OnGotoStatement;
                        nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                        PrtPushExitFunction();
                        goto DoExecuteFunction;
                    }
                case PrtContinuationReason.Raise:
                    {
                        //clear fun stack on raise
                        invertedFunStack.Clear();
                        nextSMOperation = PrtNextStatemachineOperation.HandleEventOperation;
                        hasMoreWork = true;
                        goto Finish;
                    }
                case PrtContinuationReason.NewMachine:
                    {
                        hasMoreWork = false;
                        nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                        goto Finish;
                    }
                case PrtContinuationReason.Nondet:
                    {
                        stateImpl.SetPendingChoicesAsBoolean(this);
                        continuation.nondet = ((Boolean)stateImpl.GetSelectedChoiceValue(this));
                        hasMoreWork = false;
                        goto Finish;
                    }
                case PrtContinuationReason.Receive:
                    {
                        nextSMOperation = PrtNextStatemachineOperation.ReceiveOperation;
                        hasMoreWork = true;
                        goto Finish;
                    }
                case PrtContinuationReason.Send:
                    {
                        hasMoreWork = false;
                        nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                        goto Finish;
                    }
                case PrtContinuationReason.Return:
                    {
                        switch (stateExitReason)
                        {
                            case PrtStateExitReason.NotExit:
                                {
                                    nextSMOperation = PrtNextStatemachineOperation.DequeueOperation;
                                    hasMoreWork = true;
                                    goto Finish;
                                }
                            case PrtStateExitReason.OnPopStatement:
                                {
                                    var cs = CurrentState;
                                    hasMoreWork = !PrtPopState(true);
                                    stateImpl.StateTransitionCallback?.Invoke(this, cs, CurrentState, "pop");

                                    nextSMOperation = PrtNextStatemachineOperation.DequeueOperation;
                                    stateExitReason = PrtStateExitReason.NotExit;
                                    goto Finish;
                                }
                            case PrtStateExitReason.OnGotoStatement:
                                {
                                    stateImpl.StateTransitionCallback?.Invoke(this, CurrentState, destOfGoto, "goto");
                                    PrtChangeState(destOfGoto);
                                    nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                                    stateExitReason = PrtStateExitReason.NotExit;
                                    hasMoreWork = true;
                                    goto Finish;
                                }
                            case PrtStateExitReason.OnUnhandledEvent:
                                {
                                    hasMoreWork = !PrtPopState(false);
                                    nextSMOperation = PrtNextStatemachineOperation.HandleEventOperation;
                                    stateExitReason = PrtStateExitReason.NotExit;
                                    goto Finish;
                                }
                            case PrtStateExitReason.OnTransition:
                                {
                                    stateImpl.StateTransitionCallback?.Invoke(this, CurrentState, CurrentState.transitions[eventValue].gotoState, eventValue.ToString());
                                    stateExitReason = PrtStateExitReason.OnTransitionAfterExit;
                                    nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                                    PrtPushTransitionFun(eventValue);
                                    goto DoExecuteFunction;
                                }
                            case PrtStateExitReason.OnTransitionAfterExit:
                                {
                                    stateImpl.StateTransitionCallback?.Invoke(this, CurrentState, CurrentState.transitions[eventValue].gotoState, eventValue.ToString());

                                    // The parameter to an anonymous transition function is always passed as swap.
                                    // Update currentPayload to the latest value of the parameter so that the correct
                                    // value gets passed to the entry function of the target state.
                                    PrtTransition transition = CurrentState.transitions[eventValue];
                                    PrtFun transitionFun = transition.transitionFun;
                                    if (transitionFun.IsAnonFun)
                                    {
                                        currentPayload = continuation.retLocals[0];
                                    }
                                    PrtChangeState(transition.gotoState);
                                    stateExitReason = PrtStateExitReason.NotExit;
                                    nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                                    hasMoreWork = true;
                                    goto Finish;
                                }
                            default:
                                {
                                    Debug.Assert(false, "Unexpected value for exit reason");
                                    goto Finish;
                                }
                        }

                    }
                default:
                    {
                        Debug.Assert(false, "Unexpected value for continuation reason");
                        goto Finish;
                    }
            }

            DoDequeue:
            Debug.Assert(receiveSet.Count == 0, "Machine cannot be blocked at receive when here");
            var dequeueStatus = PrtDequeueEvent(CurrentState.hasNullTransition);
            if (dequeueStatus == PrtDequeueReturnStatus.BLOCKED)
            {
                nextSMOperation = PrtNextStatemachineOperation.DequeueOperation;
                hasMoreWork = false;
                goto Finish;

            }
            else if (dequeueStatus == PrtDequeueReturnStatus.SUCCESS)
            {
                nextSMOperation = PrtNextStatemachineOperation.HandleEventOperation;
                hasMoreWork = true;
                goto Finish;
            }
            else // NULL transition
            {
                nextSMOperation = PrtNextStatemachineOperation.HandleEventOperation;
                hasMoreWork = false;
                eventValue = PrtValue.@null;
                goto Finish;
            }

            DoHandleEvent:
            Debug.Assert(receiveSet.Count == 0, "The machine must not be blocked on a receive");
            if (!currentTrigger.Equals(PrtValue.@null))
            {
                currEventValue = currentTrigger;
                currentTrigger = PrtValue.@null;
            }
            else
            {
                currEventValue = eventValue;
            }

            if (PrtIsPushTransitionPresent(currEventValue))
            {
                eventValue = currEventValue;
                stateImpl.StateTransitionCallback?.Invoke(this, CurrentState, CurrentState.transitions[currEventValue].gotoState, currEventValue.ToString());
                PrtPushState(CurrentState.transitions[currEventValue].gotoState);
                goto DoExecuteFunction;
            }
            else if (PrtIsTransitionPresent(currEventValue))
            {
                stateExitReason = PrtStateExitReason.OnTransition;
                nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                eventValue = currEventValue;
                PrtPushExitFunction();
                goto DoExecuteFunction;
            }
            else if (PrtIsActionInstalled(currEventValue))
            {
                eventValue = currEventValue;
                goto DoAction;
            }
            else
            {
                stateExitReason = PrtStateExitReason.OnUnhandledEvent;
                nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                eventValue = currEventValue;
                PrtPushExitFunction();
                goto DoExecuteFunction;
            }

            DoReceive:
            if (receiveSet.Count == 0)
            {
                stateExitReason = PrtStateExitReason.NotExit;
                nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                goto DoExecuteFunction;
            }
            dequeueStatus = PrtDequeueEvent(false);
            if (dequeueStatus == PrtDequeueReturnStatus.BLOCKED)
            {
                nextSMOperation = PrtNextStatemachineOperation.ReceiveOperation;
                hasMoreWork = false;
                goto Finish;
            }
            else if (dequeueStatus == PrtDequeueReturnStatus.SUCCESS)
            {
                stateExitReason = PrtStateExitReason.NotExit;
                nextSMOperation = PrtNextStatemachineOperation.ExecuteFunctionOperation;
                goto DoExecuteFunction;
            }
            else // NULL case
            {
                nextSMOperation = PrtNextStatemachineOperation.ReceiveOperation;
                hasMoreWork = false;
                goto Finish;
            }

            Finish:
            Debug.Assert(!hasMoreWork || currentStatus == PrtMachineStatus.Enabled, "hasMoreWork is true but the statemachine is blocked");
            return hasMoreWork;
        }

        // to abstract a machine currently means to abstract its queue
        public void abstract_me()
        {
            eventQueue.abstract_tail();
        }
    }
}
