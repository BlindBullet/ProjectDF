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
	public double Gold;	
	public Dictionary<Attr, float> AttrDefs = new Dictionary<Attr, float>();
	public List<Attr> Immunes = new List<Attr>();
	public Attr Attr;
	EnemyChart chart;

	public void SetStat(EnemyChart data, int stageNo, bool isBoss)
	{
		chart = data;
		MaxHp = ConstantData.GetEnemyHp(data.Hp, stageNo, isBoss);
		CurHp = MaxHp;
		Gold = ConstantData.GetEnemyGold(data.Gold, stageNo, isBoss);
		Spd = data.Spd;
		Attr = data.Attr;
		AttrDefs[Attr.Red] = 0f;
		AttrDefs[Attr.Blue] = 0f;
		AttrDefs[Attr.Green] = 0f;
	}

	public void InitStat()
	{
		Spd = chart.Spd;		
		AttrDefs.Clear();
		Immunes.Clear();
	}


}
