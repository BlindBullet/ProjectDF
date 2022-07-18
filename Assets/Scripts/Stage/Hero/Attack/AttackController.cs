using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
	HeroBase me;	
	SlotData slotData;
	AttackData data;
	Coroutine cAttack;

	public void Init(HeroBase heroBase, SlotData slotData)
	{
		me = heroBase;
		this.slotData = slotData;
		data = slotData.AtkData;
		Attack();
	}

	public void Attack()
	{
		if(cAttack == null)
			cAttack = StartCoroutine(AttackSequence());
	}

	IEnumerator AttackSequence()
	{
		List<HeroChart> charts = CsvData.Ins.HeroChart[me.Data.Id];
		HeroChart chart = null;

		for (int i = 0; i < charts.Count; i++)
		{
			if (charts[i].Grade == me.Data.Grade)
				chart = charts[i];
		}

		while (true)
		{
			if (me.Range.Targets.Count > 0)
			{
				EnemyBase target = me.Range.Targets[0];
				me.Tween.AttackMove(me.Stat.Spd > 1f ? me.Stat.Spd : 1f, target);

				yield return new WaitForSeconds(0.1f / me.Stat.Spd);

				Vector2 dir;
				dir = target.transform.position - me.ProjectileAnchor.position;
				dir.Normalize();
				
				if(data.Multi > 0)
				{
					for (int i = 0; i < data.Multi + 1; i++)
					{
						CalcAttackData(chart.BasicAtkProjectile, dir, target);
						yield return new WaitForSeconds(0.1f / me.Stat.Spd);
					}
				}
				else
				{
					CalcAttackData(chart.BasicAtkProjectile, dir, target);
				}

				me.Tween.BackToOriginPos(me.Stat.Spd > 1f ? me.Stat.Spd : 1f);

				yield return new WaitForSeconds((1f - (0.1f / me.Stat.Spd)) / me.Stat.Spd);
			}
			else
			{
				yield return null;
			}
		}
	}

	void CalcAttackData(string model, Vector2 dir, EnemyBase target = null)
	{
		switch (data.Front)
		{
			case 0:
				CreateProjectile(model, 0, 0, dir, target);
				break;
			case 1:
				CreateProjectile(model, -0.3f, 0, dir, target);
				CreateProjectile(model, 0.3f, 0, dir, target);
				break;
			case 2:
				CreateProjectile(model, -0.35f, 0, dir, target);
				CreateProjectile(model, 0, 0, dir, target);
				CreateProjectile(model, 0.35f, 0, dir, target);
				break;
			case 3:
				CreateProjectile(model, -0.6f, 0, dir, target);
				CreateProjectile(model, -0.25f, 0, dir, target);
				CreateProjectile(model, 0.25f, 0, dir, target);
				CreateProjectile(model, 0.6f, 0, dir, target);
				break;
			case 4:
				CreateProjectile(model, -0.7f, 0, dir, target);
				CreateProjectile(model, -0.35f, 0, dir, target);
				CreateProjectile(model, 0, 0, dir, target);
				CreateProjectile(model, 0.35f, 0, dir, target);
				CreateProjectile(model, 0.7f, 0, dir, target);
				break;			
		}

		switch (data.Diagonal)
		{
			case 1:
				CreateProjectile(model, 0, 10, dir, target);
				CreateProjectile(model, 0, -10, dir, target);
				break;
			case 2:
				CreateProjectile(model, 0, 10, dir, target);
				CreateProjectile(model, 0, -10, dir, target);
				CreateProjectile(model, 0, 20, dir, target);
				CreateProjectile(model, 0, -20, dir, target);
				break;
			case 3:
				CreateProjectile(model, 0, 10, dir, target);
				CreateProjectile(model, 0, -10, dir, target);
				CreateProjectile(model, 0, 20, dir, target);
				CreateProjectile(model, 0, -20, dir, target);
				CreateProjectile(model, 0, 30, dir, target);
				CreateProjectile(model, 0, -30, dir, target);
				break;			
		}		
	}

	void CreateProjectile(string model, float posX, float angle, Vector2 dir, EnemyBase target = null)
	{		
		ProjectileController projectile = ObjectPooler.SpawnFromPool<ProjectileController>(model, me.ProjectileAnchor.position.WithX(me.ProjectileAnchor.position.x + posX));
		//projectile.transform.position = me.ProjectileAnchor.position.WithX(me.ProjectileAnchor.position.x + posX);
		float sizeUp = Mathf.Pow(ConstantData.SizeIncP / 100f, data.Size);
		projectile.transform.localScale = Vector3.one * sizeUp;
		projectile.Setup(me, data, angle, dir, target);
	}
	
	public void StopAttack()
	{
		if(cAttack != null)
		{
			StopCoroutine(cAttack);
			cAttack = null;
		}	
	}

	public void Stop()
	{
		StopAllCoroutines();
	}

}
