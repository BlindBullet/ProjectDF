using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Lofle
{
	public class FsmBase<TFsm, TState> : StateBase<TFsm, TState>.Permission
		where TFsm : FsmBase<TFsm, TState>
		where TState : StateBase<TFsm, TState>
	{
		private Type _currentStateType = null;
		private TState _currentState = null;
		private Dictionary<Type, TState> _stateDic = new Dictionary<Type, TState>();
		private Coroutine _coroutine = null;

		public TState GetCurrentState()
		{
			return _currentState;
		}

		public Type GetStateType()
		{
			return _currentStateType;
		}

		public bool IsState<T>() where T : TState
		{
			//Debug.Log(typeof(T) + "      " + _currentStateType);
			return typeof(T) == _currentStateType;
		}

		/// <summary>
		/// 상태 전환
		/// </summary>
		public STATE Change<STATE>()
			where STATE : TState, new()
		{
			StopState(_currentStateType);
			return ChangeState<STATE>(_monoCache);
		}

		///// <summary>
		///// 상태머신의 라이프 사이클 처리, 특정 MonoBehaviour의 StartCoroutine를 사용이 필요 시 해당 함수 호출
		///// </summary>
		//public IEnumerator GetSequence<STATE>()
		//	where STATE : BASE_STATE_TYPE, new()
		//{
		//	ChangeState<STATE>();

		//	do
		//	{
		//		yield return GetSequence(_currentState);
		//	}
		//	while (_currentState != null && _currentState.isActive);
		//}

		protected MonoBehaviour _monoCache;
		protected ICoroutineBinder _coroutineSequence;
		protected ICoroutineBinder _updateSequence;

		public FsmBase<TFsm, TState> Begin<STATE>(MonoBehaviour mono) where STATE : TState, new()
		{
			_monoCache = mono;
			ChangeState<STATE>(mono);
			return this;
		}

		protected void ProcessSequence()
		{
			var curState = GetCurrentState();
			PermissionBegin(curState);
			_monoCache.StartCoroutine(ref _coroutineSequence, curState.Enter());
			_monoCache.StartCoroutine(ref _updateSequence, PermissionUpdate(curState));
		}

		/// <summary>
		/// Runner의 StartCoroutine으로 상태머신 동작
		/// </summary>
		//public void StartCoroutine<STATE>()
		//	where STATE : BASE_STATE_TYPE, new()
		//{
		//	_coroutine = Runner.Instance.StartCoroutine( GetSequence<STATE>() );
		//}

		protected virtual STATE ChangeState<STATE>(MonoBehaviour mono)
			where STATE : TState, new()
		{
			PermissionEnd(_currentState);
			StopState(_currentStateType);
			SetCurrentState<STATE>();
			return (STATE)_currentState;
		}

		private void StopState(Type type)
		{
			if (null != type && _stateDic.ContainsKey(type))
			{
				_stateDic[type].Stop();
			}

			_monoCache.StopCoroutine(_coroutineSequence);
			_monoCache.StopCoroutine(_updateSequence);
		}

		private void SetCurrentState<STATE>()
			where STATE : TState, new()
		{
			_currentStateType = typeof(STATE);

			if (!_stateDic.ContainsKey(_currentStateType))
			{
				var newState = new STATE();
				_stateDic.Add(_currentStateType, new STATE());

			}
			else
			{
				if (null == _stateDic[_currentStateType])
				{
					_stateDic[_currentStateType] = new STATE();
				}
			}

			_currentState = _stateDic[_currentStateType];
			SetOwnerStateMachine(_currentState, (TFsm)this);
			Ready(_currentState);
		}
	}

	///// <summary>
	///// Own 바로가기가 필요 없는 상태머신
	///// </summary>
	//public class StateMachine : FsmBase<StateMachine, State>
	//{


	//}

	/// <summary>
	/// Own 바로가기 기능이 추가된 상태머신, 상태머신 생성 시 대상 instance를 설정
	/// </summary>
	public class CustomFsm<OWNER> : FsmBase<CustomFsm<OWNER>, CustomFsmBase<OWNER>> where OWNER : MonoBehaviour
	{
		private OWNER _owner = default(OWNER);

		public CustomFsm()
		{

		}

		public CustomFsm(OWNER instance)
		{
			_owner = instance;
		}

		public OWNER Owner { get { return _owner; } }

		protected override STATE ChangeState<STATE>(MonoBehaviour mono)
		{
			STATE result = base.ChangeState<STATE>(mono);
			GetCurrentState().Own = (OWNER)mono;
			ProcessSequence();
			return result;
		}
	}
}