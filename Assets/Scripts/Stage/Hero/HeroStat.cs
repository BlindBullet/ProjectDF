using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

[System.Serializable]
public class HeroStat
{    
	public double Atk;	
	public float Spd;	
	public float CritChance;
	public float CritDmg;    
	public Attr Attr;    
	public float AtkInc;
	public float AtkDec;
	public float SpdInc;
	public float SpdDec;
	public float CritChanceInc;
	public float CritChanceDec;
	public float CritDmgInc;
	public float CritDmgDec;
	public float CooltimeDec;
	public int PenCount;
	public int PenCountInc;
	public int PenCountDec;

	int lv;
	HeroChart chart = null;

	public void InitData(HeroData data, int lv)
	{
		//HeroChart chart = CsvData.Ins.HeroChart[data.Id][data.Grade - 1];
		List<HeroChart> charts = CsvData.Ins.HeroChart[data.Id];
		chart = null;

		for (int i = 0; i < charts.Count; i++)
		{
			if (data.Grade == charts[i].Grade)
				chart = charts[i];
		}

		Atk = chart.Atk;		
		Attr = chart.Attr;        
		Spd = chart.Spd;		
		CritChance = 0f;
		CritDmg = 100f;
		AtkInc = 0;
		AtkDec = 0;
		SpdInc = 0;
		SpdDec = 0;
		CritChanceInc = 0;
		CritChanceDec = 0;
		CritDmgInc = 0;
		CritDmgDec = 0;
		CooltimeDec = 0;
		PenCount = chart.PenCount;
		PenCountInc = 0;
		PenCountDec = 0;

		ChangeLv(lv);
	}

	public void ChangeLv(int lv)
	{		
		this.lv = lv;
		CalcStat();
	}

	public void CalcStat()
	{
		Atk = ConstantData.GetHeroAtk(chart.Atk, lv) * (1 + (AtkInc / 100f));
		Spd = chart.Spd * (1 + (SpdInc / 100f));
		CritChance = CritChanceInc - CritChanceDec;
		CritDmg = CritDmgInc - CritDmgDec;
		PenCount = chart.PenCount + PenCountInc - PenCountDec;
	}

}
