using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class HeroData
{
    public string Id;
    public int Lv;
    public double Atk;
    public float Spd;
    public float Range;
    public float CritChance;
    public float CritDmg;

    public void InitData(HeroChart chart)
    {
        Id = chart.Id;
        Lv = 1;
        Atk = chart.Atk;
        Spd = chart.Spd;
        Range = chart.Range;
        CritChance = 50f;
        CritDmg = 100f;
    }

    public bool LevelUp()
    {
        double cost = ConstantData.GetLvUpCost(Lv);

        if (StageManager.Ins.PlayerData.Gold >= cost)
        {
            StageManager.Ins.ChangeGold(-cost);
            Lv++;
            return true;
        }
        
        return false;        
    }

    public void ChangeSpeed(float value)
    {
        Spd += value;
    }



}
