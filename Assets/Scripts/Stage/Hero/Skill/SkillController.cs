using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillController : MonoBehaviour
{		
	public Skill Skill;

	HeroBase me;
	Coroutine cSkill = null;	

	public void Init(HeroBase heroBase, HeroData data)
	{
		me = heroBase;
		List<HeroChart> heroCharts = CsvData.Ins.HeroChart[data.Id];
		HeroChart heroChart = null;
		for(int i = 0; i < heroCharts.Count; i++)
		{
			if (heroCharts[i].Grade == data.Grade)
				heroChart = heroCharts[i];
		}

		Skill = new Skill(heroChart.Skill, data.Grade);
		Skill.CoolTime = data.CurCT;		

		StartCoroutine(ProgressSkill());
		StartCoroutine(UseAutoSkill());
	}

	public void Stop()
	{		
		StopAllCoroutines();
	}

	IEnumerator ProgressSkill()
	{
		while (true)
		{
			if (cSkill == null)
			{
				Skill.ProgressCoolTime();
				me.Ui.SetCoolTimeFrame(Skill.CoolTime / Skill._CoolTime);
			}

			yield return null;
		}
	}

	IEnumerator UseAutoSkill()
	{
		while (true)
		{
			//여기 자동 스킬 사용 넣을 것
			if (StageManager.Ins.PlayerStat.UseAutoSkill)
			{
				if (UseSkill())
				{
					me.Ui.CloseSkillReadyText();
					//me.Ui.CloseSkillReadyFrame();
				}

				yield return new WaitForSeconds(1f);
			}
			else if (StageManager.Ins.PlayerStat.UseAutoSkillRate > 0f)
			{
				float randNo = Random.Range(0, 100f);

				if (randNo < StageManager.Ins.PlayerStat.UseAutoSkillRate)
				{
					if (UseSkill())
					{
						me.Ui.CloseSkillReadyText();
						//me.Ui.CloseSkillReadyFrame();
					}
				}

				yield return new WaitForSeconds(1f);
			}

			yield return null;
		}
	}

	public bool UseSkill()
	{
		if (Skill.CoolTime < Skill._CoolTime)
			return false;

		if(Skill.Data.Type == SkillType.ActiveEnemyTarget)
		{
			if (me.Range.Targets.Count <= 0 || EnemyBase.Enemies.Count <= 0)
			{
				if(!StageManager.Ins.PlayerStat.UseAutoSkill)
					DialogManager.Ins.OpenCautionBar("cant_use_skill_desc_1");				
				return false;
			}
		}		
		
		if (cSkill == null)
		{
			cSkill = StartCoroutine(SkillSequence(Skill));
			return true;
		}

		return false;
	}

	IEnumerator SkillSequence(Skill skill)
	{
		me.AttackCon.StopAttack();
		SkillChart data = skill.Data;

		if (data.CastFx != null)
			EffectManager.Ins.ShowFx(skill.Data.CastFx, me.Anchor);

		if (data.CastSfx != null)
			SoundManager.Ins.PlaySFX(skill.Data.CastSfx);

		if (data.BeginFx != null)
		{	
			EffectManager.Ins.ShowFx(skill.Data.BeginFx, me.Anchor);			
		}	

		if (data.Anim == "Attack")
		{
			if (me.Range.Targets.Count > 0)
				me.Tween.Skill(me.Stat.Spd > 1f ? me.Stat.Spd : 1f, me.Range.Targets[0]);
		}
		else
		{

		}

		float totalTime = skill.Data.TotalFrame / 30f;
		float progressTime = 0;
		
		for (int i = 0; i < data.FireFrame.Length; i++)
		{
			if (i == 0)
			{	
				progressTime += data.FireFrame[i] / 30f;
				yield return new WaitForSeconds(data.FireFrame[i] / 30f);
				
			}
			else
			{				
				progressTime += (data.FireFrame[i] - data.FireFrame[i - 1]) / 30f;
				yield return new WaitForSeconds((data.FireFrame[i] - data.FireFrame[i - 1]) / 30f);				

			}

			//리절트 그룹 실행            
			StartCoroutine(HitresultManager.Ins.ResultGroupSequence(CsvData.Ins.ResultGroupChart[data.ResultGroup[i]], me, data.Type));
		}

		if (totalTime - progressTime > 0)
		{
			yield return new WaitForSeconds(totalTime - progressTime);			
		}	
		
		skill.InitCoolTime(skill.Data.CoolDealy);
				
		cSkill = null;		
		me.AttackCon.Attack();
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

	public void InitCoolTime(float delay)
	{
		CoolTime = -delay;
	}

	public void CoolTimeDesc(float addTime)
	{
		if(CoolTime >= 0)
			CoolTime += addTime;

		if (CoolTime >= _CoolTime)
			CoolTime = _CoolTime;
	}
}