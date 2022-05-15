using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillController : MonoBehaviour
{	
	public Skill Attack;
	public Skill Skill;

	HeroBase me;    
	Coroutine cAttack = null;
	Coroutine cSkill = null;

	public void Init(HeroBase heroBase, HeroData data)
	{
		me = heroBase;
		HeroChart heroChart = CsvData.Ins.HeroChart[data.Id][data.Grade - 1];

		Attack = new Skill(heroChart.BasicAttack, data.Grade);
		Skill = new Skill(heroChart.Skill, data.Grade);

		StartCoroutine(UseAttack());
	}

	public void ReStart()
	{
		StartCoroutine(UseAttack());
	}

	public void Stop()
	{
		StopAllCoroutines();
	}

	public IEnumerator UseAttack()
	{
		while (true)
		{
			if (me.Range.Targets.Count > 0 && EnemyBase.Enemies.Count > 0)
			{
				if (cSkill == null)
				{                    
					if (cAttack == null)
					{
						cAttack = StartCoroutine(SkillSequence(Attack));                 
					}   
				}                
			}

			if(cSkill == null)
			{
				Skill.ProgressCoolTime();
				me.Ui.SetCoolTimeFrame(Skill.CoolTime / Skill._CoolTime);
			}   

			yield return null;
		}
	}

	public void UseSkill()
	{
		if (Skill.CoolTime < Skill._CoolTime)
			return;

		if(Skill.Data.Type == SkillType.ActiveEnemyTarget)
		{
			if (me.Range.Targets.Count <= 0 || EnemyBase.Enemies.Count <= 0)
			{
				DialogManager.Ins.OpenCautionBar("cant_use_skill_desc_1");				
				return;
			}
		}		
		
		if (cAttack != null)
		{
			StopCoroutine(cAttack);
			cAttack = null;
		}

		if (cSkill == null)
		{
			cSkill = StartCoroutine(SkillSequence(Skill));			
		}		
	}

	IEnumerator SkillSequence(Skill skill)
	{
		SkillChart data = skill.Data;

		if (data.CastFx != null)
			EffectManager.Ins.ShowFx(skill.Data.CastFx, me.Anchor);

		if (data.BeginFx != null)
		{	
			EffectManager.Ins.ShowFx(skill.Data.BeginFx, me.Anchor);			
		}	

		if (data.Anim == "Attack")
		{
			if (me.Range.Targets.Count > 0)
				me.Tween.Attack(me.Stat.Spd > 1f ? me.Stat.Spd : 1f, me.Range.Targets[0]);
		}
		else
		{

		}

		float totalTime = skill.Data.TotalFrame / 30f;
		float progressTime = 0;

		if (data.Type == SkillType.Attack)
		{
			totalTime = totalTime / me.Stat.Spd;			
		}

		for (int i = 0; i < data.FireFrame.Length; i++)
		{
			if (i == 0)
			{
				if (data.Type == SkillType.Attack)
				{
					progressTime += (data.FireFrame[i] / 30f) / me.Stat.Spd;
					yield return new WaitForSeconds((data.FireFrame[i] / 30f) / me.Stat.Spd);
				}
				else
				{
					progressTime += data.FireFrame[i] / 30f;
					yield return new WaitForSeconds(data.FireFrame[i] / 30f);
				}
			}
			else
			{
				if (data.Type == SkillType.Attack)
				{
					progressTime += ((data.FireFrame[i] - data.FireFrame[i - 1]) / 30f) / me.Stat.Spd;
					yield return new WaitForSeconds((data.FireFrame[i] - data.FireFrame[i - 1]) / 30f / me.Stat.Spd);
				}
				else
				{
					progressTime += (data.FireFrame[i] - data.FireFrame[i - 1]) / 30f;
					yield return new WaitForSeconds((data.FireFrame[i] - data.FireFrame[i - 1]) / 30f);
				}

			}

			//리절트 그룹 실행            
			StartCoroutine(HitresultManager.Ins.ResultGroupSequence(CsvData.Ins.ResultGroupChart[data.ResultGroup[i]], me, data.Type));
		}

		if (totalTime - progressTime > 0)
		{
			yield return new WaitForSeconds(totalTime - progressTime);			
		}	
		
		skill.InitCoolTime();

		cAttack = null;
		cSkill = null;        
	}

	

	

}

public class Skill
{
	public string Id;
	public int Grade;
	public float CoolTime;
	public float _CoolTime;
	public SkillChart Data;

	public Skill(string id, int grade)
	{
		Id = id;
		Grade = grade;
		CoolTime = 0;
		SetData();
		_CoolTime = Data.CoolTime;
	}

	void SetData()
	{
		List<SkillChart> charts = CsvData.Ins.SkillChart[Id];

		for(int i = 0; i < charts.Count; i++)
		{
			if(charts[i].Grade == Grade)
			{
				Data = charts[i];
			}
		}
	}
		
	public void ProgressCoolTime()
	{
		CoolTime += Time.deltaTime;

		if (CoolTime >= _CoolTime)
			CoolTime = _CoolTime;
	}

	public void InitCoolTime()
	{
		CoolTime = 0;
	}

}