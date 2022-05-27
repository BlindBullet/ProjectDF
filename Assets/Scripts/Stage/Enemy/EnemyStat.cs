using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

[System.Serializable]
public class EnemyStat
{
	public double MaxHp;
	public double CurHp;	
	public float Spd;
	public float Def;
	public double Gold;		
	public List<Attr> Immunes = new List<Attr>();
	public Attr Attr;
	EnemyChart chart;

	public void SetStat(EnemyChart data, int stageNo, bool isBoss)
	{
		PlayerStat playerStat = StageManager.Ins.PlayerStat;

		chart = data;
		Attr = data.Attr;
		MaxHp = ConstantData.GetEnemyHp(data.Hp, stageNo, isBoss);
		Gold = ConstantData.GetEnemyGold(data.Gold, stageNo, isBoss);
		Spd = data.Spd;
		Def = data.Def;

		if (isBoss)
		{
			MaxHp = MaxHp * (MaxHp + ((playerStat.BossEnemyHpInc[Attr] - playerStat.BossEnemyHpDec[Attr]) / 100f));
			Spd = Spd * (Spd + ((playerStat.BossEnemySpdInc[Attr] - playerStat.BossEnemySpdDec[Attr]) / 100f));
			Def = Def * (Def + ((playerStat.BossEnemySpdInc[Attr] - playerStat.BossEnemySpdDec[Attr]) / 100f));
			Gold = Gold * (Gold + ((playerStat.BossEnemyGoldInc[Attr] - playerStat.BossEnemyGoldDec[Attr]) / 100f));
		}
		else
		{
			MaxHp = MaxHp + (MaxHp * ((playerStat.NormalEnemyHpInc[Attr] - playerStat.NormalEnemyHpDec[Attr]) / 100f));
			Spd = Spd * (Spd + ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
			Def = Def * (Def + ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
			Gold = Gold * (Gold + ((playerStat.NormalEnemyGoldInc[Attr] - playerStat.NormalEnemyGoldDec[Attr]) / 100f));
		}
		
		CurHp = MaxHp;		
	}

	public void InitStat()
	{
		PlayerStat playerStat = StageManager.Ins.PlayerStat;
		Spd = Spd * (Spd + ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
		Def = Def * (Def + ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
		Immunes.Clear();
	}


}
