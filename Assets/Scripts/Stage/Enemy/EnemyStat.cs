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

    public void SetStat(EnemyChart data, int stageNo, bool isBoss)
    {
        MaxHp = ConstantData.GetEnemyHp(data.Hp, stageNo, isBoss);
        CurHp = MaxHp;
        Gold = ConstantData.GetEnemyGold(data.Gold, stageNo, isBoss);
        Spd = data.Spd;
    }
}
