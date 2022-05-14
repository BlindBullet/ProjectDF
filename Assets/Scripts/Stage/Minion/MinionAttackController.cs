using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
				Vector3 dir = (me.Target.transform.position - me.transform.position).normalized;
				me.ModelTrf.up = dir;

				//���ݽ� ����
				//���� �����Ÿ����� �־��� �� ���󰡱�
				//���� ���� ���� ���Ϸ� ��������� �ڷ� ��������
				switch (data.MoveType)
				{
					case MoveType.None:

						break;
					default:
						if (!me.CalcRange())
						{
							me.transform.DOMove(me.Target.transform.position - (dir * 2f), 1f).SetEase(Ease.Linear);
						}
						else
						{
							if (data.Range / 3f <= Vector2.Distance(me.transform.position, me.Target.transform.position))
							{
								me.transform.DOMove(me.transform.position - (dir * (data.Range / 6f)), 1f).SetEase(Ease.Linear);
								yield return new WaitForSeconds(1f);
							}
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
