using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffController : MonoBehaviour
{
	public List<EnemyBuff> Buffs = new List<EnemyBuff>();
	public List<Attr> Immunes = new List<Attr>();
	public Dictionary<Attr, float> DmgReductions = new Dictionary<Attr, float>();
	EnemyBase me;

	public void Setup(EnemyBase enemyBase)
	{
		me = enemyBase;

	}

	public void TakeBuff(EnemyBuff buff)
	{
		StartCoroutine(BuffSequence(buff));
	}

	IEnumerator BuffSequence(EnemyBuff buff)
	{
		AddBuff(buff);

		if (buff.DurationFx != null)
			EffectManager.Ins.ShowFx(buff.Data.DurationFx, me.transform.position);

		yield return new WaitForSeconds(buff.DurationTime);
				
		RemoveBuff(buff);
	}

	void AddBuff(EnemyBuff buff)
	{
		Buffs.Add(buff);
		CalcStat();
	}

	void RemoveBuff(EnemyBuff buff)
	{
		Buffs.Remove(buff);
		CalcStat();
	}

	public void CalcStat()
	{
		me.Stat.InitStat();

		float addSpd = 0f;
		float addDef = 0f;		
		List<Attr> immunes = new List<Attr>();
		
		for(int i = 0; i < Buffs.Count; i++)
		{
			if(Buffs[i].Data != null && Buffs[i].Hitresult == null)
			{
				switch (Buffs[i].Data.HitType)
				{
					case EnemySkillHitType.Buff:
						switch (Buffs[i].Data.Param1)
						{
							case "Spd":
								addSpd += float.Parse(Buffs[i].Data.Param2);
								break;
							case "Def":
								addDef += float.Parse(Buffs[i].Data.Param2);
								break;
						}
						break;
					case EnemySkillHitType.Debuff:
						switch (Buffs[i].Data.Param1)
						{
							case "Spd":
								addSpd -= float.Parse(Buffs[i].Data.Param2);
								break;
							case "Def":
								addDef -= float.Parse(Buffs[i].Data.Param2);
								break;
						}
						break;
					case EnemySkillHitType.Immune:
						switch (Buffs[i].Data.Param1)
						{
							case "Red":
								if (!immunes.Contains(Attr.Red))
									immunes.Add(Attr.Red);
								break;
							case "Green":
								if (!immunes.Contains(Attr.Green))
									immunes.Add(Attr.Green);
								break;
							case "Blue":
								if (!immunes.Contains(Attr.Blue))
									immunes.Add(Attr.Blue);
								break;
						}
						break;
				}
			}
			if (Buffs[i].Data == null && Buffs[i].Hitresult != null)
			{
				switch (Buffs[i].Hitresult.Type)
				{
					case HitType.Debuff:
						switch (Buffs[i].Hitresult.StatType)
						{
							case StatType.Spd:								
								addSpd -= Buffs[i].Hitresult.ValuePercent;
								break;
						}
						break;
				}
			}
		}

		me.Stat.AddSpd = addSpd;
		me.Stat.AddDef = addDef;
		me.Stat.Immunes = immunes;
		me.Stat.CalcStat();
	}

	

}

public class EnemyBuff
{	
	public EnemySkillChart Data;
	public HitresultChart Hitresult;
	public float DurationTime;
	public string DurationFx;

	public void SetEnemyBuff(string id)
	{
		Hitresult = null;
		Data = CsvData.Ins.EnemySkillChart[id];
		DurationFx = Data.DurationFx;

		switch (Data.HitType)
		{
			case EnemySkillHitType.Buff:
				switch (Data.Param1)
				{
					case "Spd":
						DurationTime = float.Parse(Data.Param3);
						break;
					case "Def":
						DurationTime = float.Parse(Data.Param3);
						break;
				}
				break;
			case EnemySkillHitType.Debuff:
				switch (Data.Param1)
				{
					case "Spd":
						DurationTime = float.Parse(Data.Param3);
						break;
					case "Def":
						DurationTime = float.Parse(Data.Param3);
						break;
				}
				break;
			case EnemySkillHitType.Immune:
				DurationTime = float.Parse(Data.Param2);
				break;
		}
	}

	public void SetHitresult(HitresultChart chart)
	{
		Data = null;
		Hitresult = chart;
		DurationTime = chart.DurationTime;
		DurationFx = chart.DurationFx;
	}

}