using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class HeroStat
{    
	public double Atk;	
	public float Spd;
	public float Range;
	public float CritChance;
	public float CritDmg;    
	public Attr Attr;    
	public float AtkInc;
	public float AtkDec;
	public float SpdInc;
	public float SpdDec;
	public float RangeInc;
	public float RangeDec;
	public float CooltimeDec;
	int lv;
	HeroChart chart = null;

	public void InitData(HeroData data, int lv)
	{
		chart = CsvData.Ins.HeroChart[data.Id][data.Grade];

		Atk = chart.Atk;		
		Attr = chart.Attr;        
		Spd = chart.Spd;
		Range = chart.Range;
		CritChance = 0f;
		CritDmg = 100f;
		AtkInc = 0;
		AtkDec = 0;
		SpdInc = 0;
		SpdDec = 0;
		RangeInc = 0;
		RangeDec = 0;
		CooltimeDec = 0;

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
		Range = chart.Range * (1 + (RangeInc / 100f));
	}

}
