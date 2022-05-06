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

		if (buff.Data.DurationFx != null)
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
		Dictionary<Attr, float> attrDefs = new Dictionary<Attr, float>();
		List<Attr> immunes = new List<Attr>();
		
		for(int i = 0; i < Buffs.Count; i++)
		{
			switch (Buffs[i].Data.HitType)
			{
				case EnemySkillHitType.Buff:
					switch (Buffs[i].Data.Param1)
					{
						case "Spd":
							addSpd = float.Parse(Buffs[i].Data.Param2);
							break;
						case "Def":
							if(Buffs[i].Data.Param2 == null)
								addDef = float.Parse(Buffs[i].Data.Param3);
							else
							{
								switch (Buffs[i].Data.Param2)
								{
									case "Red":
										attrDefs[Attr.Red] = float.Parse(Buffs[i].Data.Param3);
										break;
									case "Green":
										attrDefs[Attr.Green] = float.Parse(Buffs[i].Data.Param3);
										break;
									case "Blue":
										attrDefs[Attr.Blue] = float.Parse(Buffs[i].Data.Param3);
										break;
								}
							}
							break;
					}
					break;
				case EnemySkillHitType.Immune:
					switch (Buffs[i].Data.Param1)
					{
						case "Red":
							immunes.Add(Attr.Red);
							break;
						case "Green":
							immunes.Add(Attr.Green);
							break;
						case "Blue":
							immunes.Add(Attr.Blue);
							break;
					}
					break;
			}
		}

		me.Stat.Spd = me.Stat.Spd + (me.Stat.Spd * (addSpd / 100f));		
		me.Stat.Def = addDef;
		me.Stat.Immunes = immunes;
		me.Stat.AttrDefs = attrDefs;
	}

	

}

public class EnemyBuff
{
	public EnemySkillChart Data;
	public float DurationTime;

	public EnemyBuff(string id)
	{
		Data = CsvData.Ins.EnemySkillChart[id];

		switch (Data.HitType)
		{
			case EnemySkillHitType.Buff:
				switch (Data.Param1)
				{
					case "Spd":
						DurationTime = float.Parse(Data.Param3);
						break;
					case "Def":
						DurationTime = float.Parse(Data.Param4);
						break;
				}
				break;
			case EnemySkillHitType.Immune:
				DurationTime = float.Parse(Data.Param2);
				break;
		}
	}

}