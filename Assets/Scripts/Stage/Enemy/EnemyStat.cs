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
	public float AddSpd;
	public float Def;
	public float AddDef;
	public double Gold;
	public double AddGold;
	public List<Attr> Immunes = new List<Attr>();
	public Attr Attr;
	EnemyChart data;
	bool isBoss = false;
	
	public void SetStat(EnemyChart data, int stageNo, bool isBoss)
	{
		PlayerStat playerStat = StageManager.Ins.PlayerStat;

		this.isBoss = isBoss;
		this.data = data;
		Attr = data.Attr;
		MaxHp = ConstantData.GetEnemyHp(data.Hp, stageNo, isBoss);
		MaxHp = isBoss ? MaxHp + (MaxHp * ((playerStat.BossEnemyHpInc[Attr] - playerStat.BossEnemyHpDec[Attr]) / 100f)) :
			MaxHp + (MaxHp * ((playerStat.NormalEnemyHpInc[Attr] - playerStat.NormalEnemyHpDec[Attr]) / 100f));
		Gold = ConstantData.GetEnemyGold(data.Gold, stageNo, isBoss);
		Gold = isBoss ? Gold + (Gold * ((playerStat.BossEnemyGoldInc[Attr] - playerStat.BossEnemyGoldDec[Attr]) / 100f))
			: Gold + (Gold * ((playerStat.NormalEnemyGoldInc[Attr] - playerStat.NormalEnemyGoldDec[Attr]) / 100f));
		AddGold = 0;
		Spd = data.Spd;
		Spd = isBoss ? Spd + (Spd * ((playerStat.BossEnemySpdInc[Attr] - playerStat.BossEnemySpdDec[Attr]) / 100f))
			: Spd = Spd + (Spd * ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
		AddSpd = 0;
		Def = data.Def;
		Def = isBoss ? Def + (Def * ((playerStat.BossEnemyDefInc[Attr] - playerStat.BossEnemyDefDec[Attr]) / 100f))
			: Def = Def + (Def * ((playerStat.NormalEnemyDefInc[Attr] - playerStat.NormalEnemyDefDec[Attr]) / 100f));
		AddDef = 0;

		CurHp = MaxHp;		
	}

	public void InitStat()
	{
		PlayerStat playerStat = StageManager.Ins.PlayerStat;

		Spd = data.Spd;
		Def = data.Def;
		Gold = data.Gold;

		if (isBoss)
		{
			MaxHp = MaxHp + (MaxHp * ((playerStat.BossEnemyHpInc[Attr] - playerStat.BossEnemyHpDec[Attr]) / 100f));
			Spd = Spd + (Spd * ((playerStat.BossEnemySpdInc[Attr] - playerStat.BossEnemySpdDec[Attr]) / 100f));
			Def = Def + (Def * ((playerStat.BossEnemyDefInc[Attr] - playerStat.BossEnemyDefDec[Attr]) / 100f));
			Gold = Gold + (Gold * ((playerStat.BossEnemyGoldInc[Attr] - playerStat.BossEnemyGoldDec[Attr]) / 100f));
		}
		else
		{
			MaxHp = MaxHp + (MaxHp * ((playerStat.NormalEnemyHpInc[Attr] - playerStat.NormalEnemyHpDec[Attr]) / 100f));
			Spd = Spd + (Spd * ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
			Def = Def + (Def * ((playerStat.NormalEnemyDefInc[Attr] - playerStat.NormalEnemyDefDec[Attr]) / 100f));
			Gold = Gold + (Gold * ((playerStat.NormalEnemyGoldInc[Attr] - playerStat.NormalEnemyGoldDec[Attr]) / 100f));			
		}
		
		Spd = Spd + (Spd * ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
		Def = Def + (Def * ((playerStat.NormalEnemySpdInc[Attr] - playerStat.NormalEnemySpdDec[Attr]) / 100f));
		Gold = Gold + (Gold * ((playerStat.NormalEnemyGoldInc[Attr] - playerStat.NormalEnemyGoldDec[Attr]) / 100f));
		Immunes.Clear();
	}

	public void CalcStat()
	{
		Spd = Spd + (Spd * (AddSpd / 100f));		
		Def = Def + (Def * (AddDef / 100f));

	}


}
