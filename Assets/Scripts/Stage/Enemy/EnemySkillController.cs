using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


		skill.InitCoolTime();

		yield return null;
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