using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
	HeroBase me;
	List<BuffData> Buffs = new List<BuffData>();

	public void Init(HeroBase heroBase)
	{
		me = heroBase;
	}

	public void TakeBuff(BuffData buff)
	{		
		StartCoroutine(BuffSequence(buff));
	}

	void AddBuff(BuffData buff)
	{
		Buffs.Add(buff);
		CalcStat();
	}

	void RemoveBuff(BuffData buff)
	{
		Buffs.Remove(buff);
		CalcStat();
	}

	public void CalcStat()
	{
		me.Stat.CalcStat();

		float atkAdd = 0;
		float spdAdd = 0;
		float critChanceAdd = 0;
		float critDmgAdd = 0;
		int penCountAdd = 0;

		for(int i = 0; i < Buffs.Count; i++)
		{
			switch (Buffs[i].Data.StatType)
			{
				case StatType.Atk:
					switch (Buffs[i].Data.Type)
					{
						case HitType.Buff:
							atkAdd += Buffs[i].Data.ValuePercent;
							break;
						case HitType.Debuff:
							atkAdd -= Buffs[i].Data.ValuePercent;
							break;
					}
					break;
				case StatType.Spd:
					switch (Buffs[i].Data.Type)
					{
						case HitType.Buff:
							spdAdd += Buffs[i].Data.ValuePercent;
							break;
						case HitType.Debuff:
							spdAdd -= Buffs[i].Data.ValuePercent;
							break;
					}
					break;
				case StatType.CritChance:
					switch (Buffs[i].Data.Type)
					{
						case HitType.Buff:
							critChanceAdd += Buffs[i].Data.ValuePercent;
							break;
						case HitType.Debuff:
							critChanceAdd -= Buffs[i].Data.ValuePercent;
							break;
					}
					break;
				case StatType.CritDmg:
					switch (Buffs[i].Data.Type)
					{
						case HitType.Buff:
							critDmgAdd += Buffs[i].Data.ValuePercent;
							break;
						case HitType.Debuff:
							critDmgAdd -= Buffs[i].Data.ValuePercent;
							break;
					}
					break;
				case StatType.PenCount:
					switch (Buffs[i].Data.Type)
					{
						case HitType.Buff:
							penCountAdd += (int)Buffs[i].Data.Value;
							break;
						case HitType.Debuff:
							penCountAdd -= (int)Buffs[i].Data.Value;
							break;
					}
					break;
			}
		}

		me.Stat.Atk = me.Stat.Atk * (1 + (atkAdd / 100f));
		me.Stat.Spd = me.Stat.Spd * (1 + (spdAdd / 100f));
		me.Stat.CritChance = me.Stat.CritChance + critChanceAdd;
		me.Stat.CritDmg = me.Stat.CritDmg + critDmgAdd;

		for (int i = 0; i < StageManager.Ins.Slots.Length; i++)
		{
			if (StageManager.Ins.Slots[i].No == me.Data.SlotNo)
				StageManager.Ins.Slots[i].SetAtk(me.Stat.Atk);
		}
	}

	IEnumerator BuffSequence(BuffData buff)
	{
		AddBuff(buff);
				
		if (buff.Data.DurationFx != null)
			EffectManager.Ins.ShowFx(buff.Data.DurationFx, me.Anchor, buff.DurationTime);

		yield return new WaitForSeconds(buff.DurationTime);

		RemoveBuff(buff);
	}




}
