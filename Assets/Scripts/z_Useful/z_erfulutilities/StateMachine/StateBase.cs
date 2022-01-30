using System.Collections;
using UnityEngine;

namespace Lofle
{
	public abstract class StateBase<TFsm, TState>
		where TFsm : FsmBase<TFsm, TState>
		where TState : StateBase<TFsm, TState>
	{
		protected bool _bActive = false;

		private TFsm _ownerStateMachine = default(TFsm);

		public bool isActive { get { return _bActive; } }

		/// <summary>
		/// 현재 상태를 제어중인 상태머신,
		/// </summary>
		public TFsm OwnerStateMachine { get { return _ownerStateMachine; } }

		/// <summary>
		/// 상태 종료
		/// </summary>
		public void Stop() { _bActive = false; }

		/// <summary>
		/// 상태 진입 시 최초로 호출
		/// </summary>
		virtual protected void Begin() { }

		/// <summary>
		/// Begin 다음으로 호출
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerator Enter() { yield break; }

		/// <summary>
		/// 상태 유지시 지속적인 호출
		/// </summary>
		virtual protected void Update() { }

		/// <summary>
		/// 상태가 끝나는 경우 자동으로 호출
		/// </summary>
		virtual protected void End() { }

		/// <summary>
		/// 상태 전환
		/// </summary>
		/// <typeparam name="STATE">전환 상태</typeparam>
		/// <returns>다음 상태에 전달 값 처리용도</returns>
		protected STATE ChangeState<STATE>()
			where STATE : TState, new()
		{
			return OwnerStateMachine.Change<STATE>();
		}

		private void SetOwnerStateMachine(TFsm machine)
		{
			_ownerStateMachine = machine;
		}

		private void Ready()
		{
			_bActive = true;
		}

		public class Permission
		{
			static protected void SetOwnerStateMachine(TState state, TFsm machine)
			{
				state.SetOwnerStateMachine(machine);
			}

			static protected void Ready(TState state)
			{
				state.Ready();
			}

			static protected void PermissionBegin(TState state)
			{
				state.Begin();
			}

			static protected void PermissionEnd(TState state)
			{
				state?.End();
			}

			static protected IEnumerator PermissionUpdate(TState state)
			{
				while (true)
				{
					state.Update();
					yield return null;
				}
			}

			static protected IEnumerator GetSequence(TState state)
			{
				yield return state.Enter();
			}
		}
	}

	///// <summary>
	///// Own 바로가기가 필요 없는 상태 타입
	///// </summary>
	//public abstract class State : StateBase<StateMachine, State> { }

	/// <summary>
	/// Own 바로가기 기능이 추가된 상태 타입
	/// </summary>
	public abstract class CustomFsmBase<OWNER> : StateBase<CustomFsm<OWNER>, CustomFsmBase<OWNER>> where OWNER : MonoBehaviour
	{
		private OWNER _own = default(OWNER);

		public OWNER Own
		{
			get { return _own; }
			set { _own = value; }
		}
	}
}