using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

[System.Serializable]
public class Stat
{    
    public double Atk;
    public float Spd;
    public float Range;
    public float CritChance;
    public float CritDmg;    
    public Attr Attr;
    double BasicAtk;

    public void InitData(HeroData data, int lv)
    {
        HeroChart chart = CsvData.Ins.HeroChart[data.Id][data.Grade];

        Atk = chart.Atk;
        BasicAtk = chart.Atk;
        Attr = chart.Attr;        
        Spd = chart.Spd;
        Range = chart.Range;
        CritChance = 0f;
        CritDmg = 100f;
    }

    public void ChangeLv(int lv)
    {
        Atk = Atk * 1.2f;  
    }

    public void ChangeSpeed(float value)
    {
        Spd += value;
    }



}
