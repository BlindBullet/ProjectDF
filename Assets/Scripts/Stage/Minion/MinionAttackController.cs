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
	Vector3 dir;

	public void SetController(MinionBase minionBase, MinionChart chart)
	{
		me = minionBase;
		data = chart;
		dir = Vector3.up;
	}

	public void Attack()
	{
		if (cAttack == null)
		{
			cAttack = StartCoroutine(AttackSequence());
		}

		if (cAttackMove == null)
		{
			cAttackMove = StartCoroutine(AttackMoveSequence());
		}	
	}

	IEnumerator AttackSequence()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f / data.Spd);

			List<HitresultChart> hitresults = CsvData.Ins.HitresultChart[data.Hitresult];
			List<ProjectileChart> projectiles = CsvData.Ins.ProjectileChart[data.Projectile];

			for (int i = 0; i < projectiles.Count; i++)
			{
				ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/" + projectiles[i].Model) as GameObject);				
				projectile.transform.position = me.ProjectileAnchor.position.WithX(me.ProjectileAnchor.position.x + projectiles[i].PosX);
				projectile.Setup(projectiles[i], hitresults, me, dir, me.Target);
			}
		}
	}

	IEnumerator AttackMoveSequence()
	{	
		while (true)
		{
			if (me.Target != null && me.Target.Stat.CurHp > 0f)
			{
				dir = (me.Target.transform.position - me.transform.position).normalized;
				me.ModelTrf.up = dir;
								
				switch (data.MoveType)
				{
					case MoveType.None:
						break;
					default:
						if (!me.CalcRange())
						{
							DOTween.Kill("MinionMove1");
							me.transform.DOMove(me.Target.transform.position - (dir * 2f), 1f).SetEase(Ease.Linear).SetId("MinionMove1");
						}
						else
						{
							if (data.Range / 3f <= Vector2.Distance(me.transform.position, me.Target.transform.position))
							{
								DOTween.Kill("MinionMove2");
								me.transform.DOMove(me.transform.position - (dir * (data.Range / 6f)), 1f).SetEase(Ease.Linear).SetId("MinionMove2");
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
