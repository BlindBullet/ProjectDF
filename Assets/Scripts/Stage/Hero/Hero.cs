using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

[System.Serializable]
public class Hero
{
    public string Id;
    public int Grade;
    public int Lv;
    public double Atk;
    public float Spd;
    public float Range;
    public float CritChance;
    public float CritDmg;
    public int AtkLv;
    public int SkillLv;

    public void InitData(HeroChart chart)
    {
        Id = chart.Id;        
        Grade = chart.Grade;
        Lv = 1;
        Atk = chart.Atk;
        Spd = chart.Spd;
        Range = chart.Range;
        CritChance = 0f;
        CritDmg = 100f;
        AtkLv = 1;
        SkillLv = 1;
    }

    public bool LevelUp()
    {
        double cost = ConstantData.GetLvUpCost(Lv);

        if (StageManager.Ins.PlayerData.Gold >= cost)
        {
            StageManager.Ins.ChangeGold(-cost);
            Lv++;
            Atk = Math.Round(Atk * 1.2f);
            return true;
        }
        
        return false;        
    }

    public void ChangeSpeed(float value)
    {
        Spd += value;
    }



}
