using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySkillController : MonoBehaviour
{
	public List<EnemySkill> Skills = new List<EnemySkill>();
	public List<EnemySkill> DieSkills = new List<EnemySkill>();
	EnemyBase me;

	public void Setup(EnemyBase enemyBase, EnemyChart chart)
	{
		me = enemyBase;

		if (chart.Skill == null)
			return;

		for(int i = 0; i < chart.Skill.Length; i++)
		{
			if (chart.Skill[i] == "")
				break;

			EnemySkill skill = new EnemySkill(CsvData.Ins.EnemySkillChart[chart.Skill[i]]);

			if (skill.Data.UseConType == EnemySkillUseConType.Die)
				DieSkills.Add(skill);
			else
				Skills.Add(skill);
		}

		StartCoroutine(SkillSequence());
	}

	IEnumerator SkillSequence()
	{
		while (true)
		{
			if (!me.isStunned)
			{
				for(int i = 0; i < Skills.Count; i++)
				{
					if (Skills[i].ProgressCoolTime())
					{
						StartCoroutine(UseSkill(Skills[i]));
					}
				}
			}

			yield return null;
		}
	}

	public void UseDieSkill()
	{
		for(int i = 0; i < DieSkills.Count; i++)
		{	
			StartCoroutine(UseSkill(DieSkills[i]));			
		}
	}

	IEnumerator UseSkill(EnemySkill skill)
	{
		Debug.Log(skill.Data.Id + " »ç¿ë");

		if (skill.Data.BeginFx != null)
			EffectManager.Ins.ShowFx(skill.Data.BeginFx, this.transform);

		List<EnemyBase> targets = SearchTarget(skill);

		yield return new WaitForSeconds(skill.Data.FireFrame / 30f);

		for (int i = 0; i < targets.Count; i++)
		{
			SendHitresult(targets[i], skill);
		}

		yield return new WaitForSeconds((skill.Data.TotalFrame - skill.Data.FireFrame) / 30f);
		
		skill.InitCoolTime();
	}

	void SendHitresult(EnemyBase target, EnemySkill skill)
	{
		if (skill.Data.HitFx != null)
			EffectManager.Ins.ShowFx(skill.Data.HitFx, target.transform);

		switch (skill.Data.HitType)
		{
			case EnemySkillHitType.Heal:
				target.TakeHeal(float.Parse(skill.Data.Param1));
				break;
			case EnemySkillHitType.Immune:
				target.BuffCon.TakeBuff(new EnemyBuff(skill.Data.Id));
				break;
			case EnemySkillHitType.Buff:
				target.BuffCon.TakeBuff(new EnemyBuff(skill.Data.Id));
				break;
			case EnemySkillHitType.Summon:
				Debug.Log("a");
				break;
		}
	}

	List<EnemyBase> SearchTarget(EnemySkill skill)
	{
		List<EnemyBase> result = new List<EnemyBase>();

		switch (skill.Data.Target)
		{
			case EnemySkillTargetType.Me:
				result.Add(me);
				break;
			case EnemySkillTargetType.All:
				result = EnemyBase.Enemies;
				break;
			case EnemySkillTargetType.AllNotMe:
				for(int i = 0; i < EnemyBase.Enemies.Count; i++)
				{
					if (EnemyBase.Enemies[i] != me)
						result.Add(me);
				}
				break;
			case EnemySkillTargetType.Close:
				List<EnemyBase> _targets = EnemyBase.Enemies.OrderBy(a => Vector2.Distance(me.transform.position, a.transform.position)).ToList();

				for(int i = 0; i < skill.Data.TargetCount + 1; i++)
				{
					if(_targets.Count >= i + 1)
					{
						if (_targets[i] != me)
							result.Add(_targets[i]);
					}
				}				
				break;
		}

		return result;
	}


}

public class EnemySkill
{
	public EnemySkillChart Data;
	public float CoolTime;
	public float _CoolTime;

	public EnemySkill(EnemySkillChart chart)
	{
		Data = chart;
		CoolTime = 0f;
		_CoolTime = chart.UseConValue;
	}

	public bool ProgressCoolTime()
	{
		CoolTime += Time.deltaTime;

		if (CoolTime >= _CoolTime)
		{
			CoolTime = _CoolTime;
			return true;
		}

		return false;
	}

	public void InitCoolTime()
	{
		CoolTime = 0f;
	}
	
}