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
			Owner.me.IdleMove();
		}

		protected override void Update()
		{			
			if(Owner.me.Target != null)
			{
				if (Owner.me.CalcRange())
				{
					Invoke<AttackState>();
				}
				else
				{
					Invoke<MoveState>();
				}
			}

			if (Owner.me.IsDie)
			{
				Invoke<DieState>();
			}
		}

		protected override void End()
		{
			Owner.me.StopIdleMove();
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

			if (Owner.me.Target != null && Owner.me.CalcRange())
			{				
				Invoke<AttackState>();
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
			Owner.me.AttackCon.Attack();
		}

		protected override void Update()
		{
			if (Owner.me.Target == null || Owner.me.Target.Stat.CurHp <= 0f)
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
			Owner.me.AttackCon.StopAttack();
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
