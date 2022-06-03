using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitresultManager : MonoSingleton<HitresultManager>
{
	public void RunResultGroup(List<ResultGroupChart> datas, Vector2 pos, HeroBase caster)
	{
		StartCoroutine(ResultGroupSequence(datas, pos, caster));
	}

	public void RunResultGroup(List<ResultGroupChart> datas, Vector2 pos, MinionBase minion)
	{
		StartCoroutine(ResultGroupSequence(datas, pos, minion));
	}

	public IEnumerator ResultGroupSequence(List<ResultGroupChart> datas, Vector2 pos, HeroBase caster, SkillType type)
	{
		for (int i = 0; i < datas.Count; i++)
		{   
			switch (datas[i].TargetType)
			{
				case TargetType.Enemy:                    
					yield return new WaitForSeconds(datas[i].DelayTime);

					if (datas[i].DelayBeginFx != null)
						EffectManager.Ins.ShowFx(datas[i].DelayBeginFx, pos);

					List<EnemyBase> enemyTargets = SearchEnemyTargets(datas[i], pos);

					//범위에 따라 타겟을 지정
					for (int k = 0; k < enemyTargets.Count; k++)
					{
						//히트리절트 전달						
						SendResultGroup(datas[i], caster, type, enemyTargets[k]);
					}
					break;
			}
		}
	}

	public IEnumerator ResultGroupSequence(List<ResultGroupChart> datas, Vector2 pos, HeroBase caster)
	{
		for (int i = 0; i < datas.Count; i++)
		{
			switch (datas[i].TargetType)
			{
				case TargetType.Enemy:					
					yield return new WaitForSeconds(datas[i].DelayTime);

					if (datas[i].DelayBeginFx != null)
						EffectManager.Ins.ShowFx(datas[i].DelayBeginFx, pos);

					List<EnemyBase> enemyTargets = SearchEnemyTargets(datas[i], pos);

					//범위에 따라 타겟을 지정
					for (int k = 0; k < enemyTargets.Count; k++)
					{
						//히트리절트 전달
						SendResultGroup(datas[i], caster, SkillType.None, enemyTargets[k]);
					}
					break;
			}
		}
	}

	public IEnumerator ResultGroupSequence(List<ResultGroupChart> datas, Vector2 pos, MinionBase minion)
	{
		for (int i = 0; i < datas.Count; i++)
		{
			switch (datas[i].TargetType)
			{
				case TargetType.Enemy:					
					yield return new WaitForSeconds(datas[i].DelayTime);

					if (datas[i].DelayBeginFx != null)
						EffectManager.Ins.ShowFx(datas[i].DelayBeginFx, pos);

					List<EnemyBase> enemyTargets = SearchEnemyTargets(datas[i], pos);

					//범위에 따라 타겟을 지정
					for (int k = 0; k < enemyTargets.Count; k++)
					{
						//히트리절트 전달
						SendResultGroup(datas[i], minion, SkillType.None, enemyTargets[k]);
					}
					break;
			}
		}
	}

	public IEnumerator ResultGroupSequence(List<ResultGroupChart> datas, HeroBase caster, SkillType type)
	{
		for (int i = 0; i < datas.Count; i++)
		{
			switch (datas[i].TargetType)
			{
				case TargetType.Enemy:
					List<EnemyBase> enemyTargets = SearchEnemyTargets(datas[i], caster);

					if (datas[i].RangeType == RangeType.None)
					{						
						//히트리절트 전달
						if(enemyTargets.Count > 0)
						{
							for(int k = 0; k < enemyTargets.Count; k++)
							{				
								SendResultGroup(datas[i], caster, type, enemyTargets[k]);
							}
						}
						else
						{
							SendResultGroup(datas[i], caster, type);
						}
					}
					else
					{
						yield return new WaitForSeconds(datas[i].DelayTime);

						if (datas[i].DelayBeginFx != null)
							EffectManager.Ins.ShowFx(datas[i].DelayBeginFx);

						if (datas[i].AddDelayTarget)
						{
							//범위에 따라 다시 타겟을 지정
							for (int k = 0; k < enemyTargets.Count; k++)
							{
								List<EnemyBase> delayTargets = caster.Range.SearchTarget(datas[i], enemyTargets[i]);

								if (delayTargets.Count > 0)
								{
									for (int j = 0; j < delayTargets.Count; j++)
									{
										//히트리절트 전달
										SendResultGroup(datas[i], caster, type, delayTargets[j]);
									}
								}
							}
						}
						else
						{
							//히트리절트 전달
							if (enemyTargets.Count > 0)
							{
								for (int k = 0; k < enemyTargets.Count; k++)
								{
									SendResultGroup(datas[i], caster, type, enemyTargets[k]);
								}
							}
							else
							{
								SendResultGroup(datas[i], caster, type);
							}
						}
					}
					break;
				case TargetType.Hero:
					List<HeroBase> heroTargets = SearchHeroTargets(datas[i], caster);
					for (int k = 0; k < heroTargets.Count; k++)
					{
						//히트리절트 전달
						SendHitresult(datas[i], caster, heroTargets[k]);
					}
					break;
				case TargetType.Me:
					//히트리절트 전달
					SendHitresult(datas[i], caster, caster);
					break;
			}
		}
	}

	public void SendResultGroup(ResultGroupChart data, HeroBase caster, SkillType type, EnemyBase target = null)
	{
		List<HitresultChart> hitresults = CsvData.Ins.HitresultChart[data.Hitresult];

		if (data.Hitresult != null && data.Projectile == null && target != null)
		{
			//히트리절트만 전달
			SendHitresult(hitresults, target, caster);
		}
		else
		{			
			Vector2 dir = Vector2.up;
			
			switch (type)
			{
				case SkillType.None:
					dir = Vector2.up;
					break;				
				case SkillType.Active:
					dir = Vector2.up;
					break;
				case SkillType.ActiveEnemyTarget:
					if (target != null)
						dir = target.transform.position - caster.ProjectileAnchor.position;
					break;
			}

			dir.Normalize();
			
			if(data.Projectile != null)
			{
				//프로젝타일 발사
				List<ProjectileChart> projectiles = CsvData.Ins.ProjectileChart[data.Projectile];

				for (int i = 0; i < projectiles.Count; i++)
				{
					ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/" + projectiles[i].Model) as GameObject);
					projectile.transform.position = caster.ProjectileAnchor.position.WithX(caster.ProjectileAnchor.position.x + projectiles[i].PosX);
					projectile.Setup(projectiles[i], hitresults, caster, dir, target);
				}
			}
		}		
	}

	public void SendResultGroup(ResultGroupChart data, MinionBase minion, SkillType type, EnemyBase target = null)
	{
		List<HitresultChart> hitresults = CsvData.Ins.HitresultChart[data.Hitresult];

		if (data.Hitresult != null && data.Projectile == null && target != null)
		{
			//히트리절트만 전달
			SendHitresult(hitresults, target, minion);
		}
		else
		{
			Vector2 dir = Vector2.up;

			switch (type)
			{
				case SkillType.None:
					dir = Vector2.up;
					break;				
				case SkillType.Active:
					dir = Vector2.up;
					break;
				case SkillType.ActiveEnemyTarget:
					if (target != null)
						dir = target.transform.position - minion.ProjectileAnchor.position;
					break;
			}

			//float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			//caster.ProjectileAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			dir.Normalize();

			if(data.Projectile != null)
			{
				//프로젝타일 발사
				List<ProjectileChart> projectiles = CsvData.Ins.ProjectileChart[data.Projectile];

				for (int i = 0; i < projectiles.Count; i++)
				{
					ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/" + projectiles[i].Model) as GameObject);					
					projectile.transform.position = minion.ProjectileAnchor.position.WithX(minion.ProjectileAnchor.position.x + projectiles[i].PosX);
					projectile.Setup(projectiles[i], hitresults, minion, dir, target);
				}
			}
		}
	}

	public void SendHitresult(ResultGroupChart data, HeroBase caster, HeroBase target)
	{
		List<HitresultChart> hitresults = CsvData.Ins.HitresultChart[data.Hitresult];
		
		for(int i = 0; i < hitresults.Count; i++)
		{
			float randNo = Random.Range(0, 100);

			if (hitresults[i].Prob >= randNo)
			{
				if (hitresults[i].HitFx != null)
				{
					EffectManager.Ins.ShowFx(hitresults[i].HitFx, target.Anchor);
				}
								
				switch (hitresults[i].Type)
				{
					case HitType.Buff:
						BuffData buff = new BuffData();
						buff.SetData(hitresults[i]);
						target.BuffCon.TakeBuff(buff);
						break;
					case HitType.Debuff:
						BuffData debuff = new BuffData();
						debuff.SetData(hitresults[i]);
						target.BuffCon.TakeBuff(debuff);
						break;
					case HitType.Summon:						
						for(int k = 0; k < hitresults[i].SummonIds.Length; k++)
						{
							MinionChart minionChart = CsvData.Ins.MinionChart[hitresults[i].SummonIds[k]];
							MinionBase minion = ObjectManager.Ins.Pop<MinionBase>(Resources.Load("Prefabs/Characters/Minions/MinionObj") as GameObject);
							minion.transform.localScale = new Vector2(minionChart.Size, minionChart.Size);
							minion.transform.position = new Vector2(target.ProjectileAnchor.position.x + hitresults[i].SummonPosX[k], target.ProjectileAnchor.position.y + hitresults[i].SummonPosY[k]);
							minion.Init(minionChart, target, hitresults[i].DurationTime);							
						}						
						break;
				}
			}
			else
			{
				Debug.Log("미스");
			}
		}
	}

	public void SendHitresult(List<HitresultChart> hitresults, EnemyBase target, MinionBase minion)
	{
		for (int i = 0; i < hitresults.Count; i++)
		{
			float randNo = Random.Range(0, 100);

			if (hitresults[i].Prob >= randNo)
			{
				if (hitresults[i].HitFx != null)
				{
					EffectManager.Ins.ShowFx(hitresults[i].HitFx, target.transform);
				}

				switch (hitresults[i].Type)
				{
					case HitType.Dmg:
						switch (hitresults[i].FactorOwner)
						{
							case FactorOwner.Caster:
								double dmg = hitresults[i].Value + (minion.Stat.Atk * (hitresults[i].ValuePercent / 100f));
								bool isCrit = Random.Range(0f, 100f) < minion.Stat.CritChance ? true : false;
								double resultDmg = 0;

								if (isCrit)
									dmg = dmg * (1 + (minion.Stat.CritDmg / 100f));

								Vector3 pos = target.transform.position;
								resultDmg = target.TakeDmg(dmg, minion.Stat.Attr, isCrit, hitresults[i].StiffTime);
								FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToCurrencyString(), isCrit);
								break;
							case FactorOwner.Target:

								break;
						}
						break;
					case HitType.Push:
						target.Push(hitresults[i].Value, hitresults[i].DurationTime);
						break;
					case HitType.Stun:
						target.Stun(hitresults[i].DurationTime);
						break;
				}
			}
			else
			{
				Debug.Log("미스");
			}
		}
	}	

	public void SendHitresult(List<HitresultChart> hitresults, EnemyBase target, HeroBase caster)
	{
		for (int i = 0; i < hitresults.Count; i++)
		{
			float randNo = Random.Range(0, 100);

			if (hitresults[i].Prob >= randNo)
			{
				if (hitresults[i].HitFx != null)
				{                    
					EffectManager.Ins.ShowFx(hitresults[i].HitFx, target.transform);
				}

				switch (hitresults[i].Type)
				{
					case HitType.Dmg:
						switch (hitresults[i].FactorOwner)
						{
							case FactorOwner.Caster:
								double dmg = hitresults[i].Value + (caster.Stat.Atk * (hitresults[i].ValuePercent / 100f));
								bool isCrit = Random.Range(0f, 100f) < caster.Stat.CritChance ? true : false;
								double resultDmg = 0;

								if (isCrit)
									dmg = dmg * (1 + (caster.Stat.CritDmg / 100f));
								
								Vector3 pos = target.transform.position;
								resultDmg = target.TakeDmg(dmg, caster.Stat.Attr, isCrit, hitresults[i].StiffTime);
								FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToCurrencyString(), isCrit);
								break;
							case FactorOwner.Target:

								break;
						}
						break;
					case HitType.Debuff:
						EnemyBuff enemyBuff = new EnemyBuff();
						enemyBuff.SetHitresult(hitresults[i]);
						target.BuffCon.TakeBuff(enemyBuff);
						break;
					case HitType.Push:                        
						target.Push(hitresults[i].Value, hitresults[i].DurationTime);                        
						break;
					case HitType.Stun:
						target.Stun(hitresults[i].DurationTime);
						break;
				}
			}
			else
			{
				Debug.Log("미스");
			}
		}
	}

	List<HeroBase> SearchHeroTargets(ResultGroupChart data, HeroBase caster)
	{
		List<HeroBase> targets = new List<HeroBase>();

		switch (data.TargetDetail)
		{
			case TargetDetail.All:
				targets = HeroBase.Heroes;
				break;
			case TargetDetail.SideAndMe:
				for(int i = 0; i < HeroBase.Heroes.Count; i++)
				{
					if (HeroBase.Heroes[i] == caster || HeroBase.Heroes[i].Data.SlotNo == caster.Data.SlotNo - 1
						|| HeroBase.Heroes[i].Data.SlotNo == caster.Data.SlotNo + 1)
						targets.Add(HeroBase.Heroes[i]);
				}
				break;
			case TargetDetail.Side:
				for (int i = 0; i < HeroBase.Heroes.Count; i++)
				{
					if (HeroBase.Heroes[i].Data.SlotNo == caster.Data.SlotNo - 1 || HeroBase.Heroes[i].Data.SlotNo == caster.Data.SlotNo + 1)
						targets.Add(HeroBase.Heroes[i]);
				}
				break;
		}

		return targets;
	}

	List<EnemyBase> SearchEnemyTargets(ResultGroupChart data, HeroBase caster)
	{
		List<EnemyBase> targets = new List<EnemyBase>();

		switch (data.TargetDetail)
		{
			case TargetDetail.All:
				for (int i = 0; i < caster.Range.Targets.Count; i++)
				{
					targets.Add(caster.Range.Targets[i]);
				}
				break;
			case TargetDetail.Closest:
				for (int i = 0; i < data.TargetCount; i++)
				{
					if (i < caster.Range.Targets.Count)
						targets.Add(caster.Range.Targets[i]);
				}
				break;
		}

		return targets;
	}

	List<EnemyBase> SearchEnemyTargets(ResultGroupChart data, Vector2 pos)
	{   
		List<EnemyBase> targets = new List<EnemyBase>();
		List<EnemyBase> results = new List<EnemyBase>();
		Collider2D[] colls = null;
		
		switch (data.RangeType)
		{
			case RangeType.Circle:
				colls = Physics2D.OverlapCircleAll(pos, data.RnageSize[0]);
				break;
		}

		for (int i = 0; i < colls.Length; i++)
		{
			if(colls[i].CompareTag("Enemy"))
				targets.Add(colls[i].GetComponent<EnemyBase>());
		}

		switch (data.TargetDetail)
		{
			case TargetDetail.All:
				results = targets;
				break;
			case TargetDetail.Closest:
				targets.Sort(delegate (EnemyBase A, EnemyBase B)
				{
					float distA = Vector2.Distance(pos, A.transform.position);
					float distB = Vector2.Distance(pos, B.transform.position);

					if(distA < distB)
					{
						return -1;
					}
					else
					{
						return 1;
					}
				});

				for(int i = 0; i < data.TargetCount; i++)
				{
					results.Add(targets[i]);
				}
				break;
		}

		return results;
	}

}
