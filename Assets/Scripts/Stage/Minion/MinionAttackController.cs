using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttackController : MonoBehaviour
{
	MinionBase me;
	MinionChart data;
	Coroutine cAttack = null;
	Coroutine cAttackMove = null;

	public void SetController(MinionBase minionBase, MinionChart chart)
	{
		me = minionBase;
		data = chart;
	}

	public void Attack()
	{
		if (cAttack == null)
			cAttack = StartCoroutine(AttackSequence());

		if (cAttackMove == null)
			cAttackMove = StartCoroutine(AttackMoveSequence());
	}

	IEnumerator AttackSequence()
	{
		
		yield return null;
				
	}

	IEnumerator AttackMoveSequence()
	{	
		while (true)
		{
			if (me.Target != null && me.Target.Stat.CurHp > 0f)
			{
				Vector2 dir = (me.Target.transform.position - me.transform.position).normalized;
				me.ModelTrf.up = dir;

				//공격시 무빙
				//적이 사정거리보다 멀어질 때 따라가기
				//적이 일정 범위 이하로 가까워지면 뒤로 물러서기
				switch (data.MoveType)
				{
					case MoveType.None:

						break;
					default:
						if (!me.CalcRange())
						{
							me.transform.Translate(dir * 5f * Time.deltaTime);
						}
						break;
				}
			}

			yield return null;
		}
	}

	public void StopAttack()
	{
		if (cAttack != null)
		{
			StopCoroutine(cAttack);			
			cAttack = null;
		}

		if(cAttackMove != null)
		{
			StopCoroutine(cAttackMove);
			cAttackMove = null;
		}
	}



}
