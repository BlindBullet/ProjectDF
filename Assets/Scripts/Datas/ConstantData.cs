using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public static class ConstantData
{
    public static ObscuredString[] StartHeroes = { "10001", "10002", "10003", "10004", "10005", };
    public static ObscuredDouble StartGold = 100f;
    public static ObscuredFloat LvUpFactor = 1.2f;
    public static ObscuredFloat AtkLvUpFactor = 1.1f;
    public static ObscuredFloat EnemyLvUpFactor = 1.2f;
    public static ObscuredFloat EnemyHpFactor = 1.1f;
    public static ObscuredFloat EnemyGoldFactor = 1.1f;
    public static ObscuredInt PossibleAscensionStage = 30;
    public static ObscuredDouble AscensionBasicReward = 50f;
    public static ObscuredFloat AscensionRewardFactor = 1.1f;

    public static double GetLvUpCost(int lv)
    {
        return StartGold * Mathf.Pow(LvUpFactor, lv);
    }

    public static double GetEnemyHp(double basicHp, int stageNo, bool isBoss)
    {
        return Math.Round((basicHp * (Mathf.Pow(EnemyHpFactor, stageNo - 1) * Mathf.Pow(EnemyLvUpFactor, stageNo / 10))));
    }

    public static double GetEnemyGold(double basicGold, int stageNo, bool isBoss)
    {
        return Math.Round(basicGold * Mathf.Pow(EnemyGoldFactor, stageNo - 1));
    }

}
