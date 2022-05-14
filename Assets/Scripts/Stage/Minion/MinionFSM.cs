using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofle;

public class MinionFSM : MonoBehaviour
{
	StateMachine<MinionFSM> _stateMachine = null;
	MinionBase me;

	public void SetFSM(MinionBase minionBase)
	{
		me = minionBase;
		_stateMachine = new StateMachine<MinionFSM>(this);
		StartCoroutine(_stateMachine.Coroutine<IdleState>());
	}

	private class IdleState : State<MinionFSM>
	{
		protected override void Begin()
		{
			Owner.me.SearchTarget();
		}

		protected override void Update()
		{			
			if(Owner.me.Target != null)
			{
				Invoke<MoveState>();
			}

			if (Owner.me.IsDie)
			{
				Invoke<DieState>();
			}
		}

		protected override void End()
		{

		}
	}

	private class MoveState : State<MinionFSM>
	{
		protected override void Begin()
		{
			Owner.me.Move();
		}

		protected override void Update()
		{
			if(Owner.me.Target == null)
			{
				Invoke<IdleState>();
			}

			if (Owner.me.IsDie)
			{
				Invoke<DieState>();
			}
		}

		protected override void End()
		{
			Owner.me.StopMove();
		}
	}

	private class AttackState : State<MinionFSM>
	{
		protected override void Begin()
		{

		}

		protected override void Update()
		{


			if (Owner.me.IsDie)
			{
				Invoke<DieState>();
			}
		}

		protected override void End()
		{

		}
	}

	private class DieState : State<MinionFSM>
	{
		protected override void Begin()
		{
			Owner.me.Unsummon();
		}

		protected override void Update()
		{

		}

		protected override void End()
		{

		}
	}

}
