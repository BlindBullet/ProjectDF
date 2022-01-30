using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DUtils
{
    public delegate void TransitionHandler();

    public class State<TS> where TS : struct, IConvertible, IComparable, IFormattable
    {
        public Func<FiniteStateMachine<TS>, IEnumerator> StateStarted;
        public Func<FiniteStateMachine<TS>, IEnumerator> StateExited;

        public void Set(Func<FiniteStateMachine<TS>, IEnumerator> enterSequence,
            Func<FiniteStateMachine<TS>, IEnumerator> exitSequence)
        {
            StateStarted = enterSequence;
            StateExited = exitSequence;
        }
    }

    /// <summary>
    ///     StateTransition class
    /// </summary>
    /// <typeparam name="TS">Fsm enum</typeparam>
    public class StateTransition<TS> : IEquatable<StateTransition<TS>>
        where TS : struct, IConvertible, IComparable, IFormattable
    {
        private TS _srcState;

        private TS _dstState;

        private StateTransition()
        {
        }

        public StateTransition(TS src, TS dst)
        {
            _srcState = src;
            _dstState = dst;
        }

        public bool Equals(StateTransition<TS> other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            return _srcState.Equals(other.GetSource()) && _dstState.Equals(other.GetDestination());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _srcState.GetHashCode();
                hash = hash * 23 + _dstState.GetHashCode();
                return hash;
            }
        }

        public TS GetSource()
        {
            return _srcState;
        }

        public TS GetDestination()
        {
            return _dstState;
        }
    }

    /// <summary>
    ///     A generic Finite state machine
    /// </summary>
    /// <typeparam name="TS"></typeparam>
    public class FiniteStateMachine<TS> where TS : struct, IConvertible, IComparable, IFormattable
    {
        private readonly MonoBehaviour _coroutineOwner;

        private Dictionary<TS, State<TS>> states;
        private Dictionary<StateTransition<TS>, List<TransitionHandler>> transitions;

        private ICoroutineBinder _fsmSequence;

        private bool _isRunning;

        public TS CurState
        {
            get;
            private set;
        }

        public TS PrevState
        {
            get;
            private set;
        }

        private FiniteStateMachine()
        {
        }

        public FiniteStateMachine(TS state, MonoBehaviour coroutineExecutor)
        {
            CurState = state;
            _coroutineOwner = coroutineExecutor;
            states = new Dictionary<TS, State<TS>>();
            transitions = new Dictionary<StateTransition<TS>, List<TransitionHandler>>();
            foreach (TS elem in Enum.GetValues(typeof(TS)))
            {
                var newState = new State<TS>();
                newState.StateStarted = DoNothingOnState;
                newState.StateExited = DoNothingOnState;
                states.Add(elem, newState);
            }
        }

        public void InitState(TS state,
            Func<FiniteStateMachine<TS>, IEnumerator> enterSequence = null,
            Func<FiniteStateMachine<TS>, IEnumerator> exitSequence = null)
        {
            if (enterSequence == null)
            {
                enterSequence = DoNothingOnState;
            }
            if (exitSequence == null)
            {
                exitSequence = DoNothingOnState;
            }
            states[state].Set(enterSequence, exitSequence);
        }

        private IEnumerator DoNothingOnState(FiniteStateMachine<TS> fsm)
        {
            yield return null;
        }

        public void MakeTransitionEvent(TS src, TS dst, TransitionHandler c)
        {
            var tr = new StateTransition<TS>(src, dst);

            if (!transitions.ContainsKey(tr))
            {
                transitions.Add(tr, new List<TransitionHandler>());
            }

            transitions[tr].Add(c);
        }

        public void MakeBidirectionalTransitionEvent(TS src, TS dst, TransitionHandler c)
        {
            MakeTransitionEvent(src, dst, c);
            MakeTransitionEvent(dst, src, c);
        }

        public void MakeOmmidirectionalTransitionEvent(TS dst, TransitionHandler c)
        {
            foreach (TS elem in Enum.GetValues(typeof(TS)))
            {
                MakeTransitionEvent(elem, dst, c);
            }
        }

        public void Run()
        {
            _isRunning = true;
            _coroutineOwner.StartCoroutine(ref this._fsmSequence, states[CurState].StateStarted(this));
        }

        public void To(TS nextState)
        {
            if (nextState.Equals(CurState))
            {
                if (!_isRunning)
                {
                    Run();
                }
                return;
            }
            Transition(nextState);
        }

        public void Back()
        {
            if (!_isRunning)
            {
                Run();
            }
            Transition(PrevState);
        }

        private void Transition(TS dstState)
        {
            var currentTransition = new StateTransition<TS>(CurState, dstState);

            PrevState = CurState;
            CurState = dstState;

            List<TransitionHandler> avaliableTransitions;
            if (transitions.TryGetValue(currentTransition, out avaliableTransitions) && avaliableTransitions != null)
            {
                for (int i = 0; i < avaliableTransitions.Count; i++)
                {
                    (avaliableTransitions[i])();
                }
            }
            _coroutineOwner.StartCoroutine(states[PrevState].StateExited(this));
            _coroutineOwner.StartCoroutine(ref this._fsmSequence, states[CurState].StateStarted(this));
        }
    }
}