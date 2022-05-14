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
			cAttack = StartCoroutine(AttackSequence());

		if (cAttackMove == null)
			cAttackMove = StartCoroutine(AttackMoveSequence());
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
				//projectile.transform.rotation = Quaternion.AngleAxis(angle - 90 + projectiles[i].Angle, Vector3.forward);
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
