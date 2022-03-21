using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public static class ConstantData
{
    public static ObscuredString[] StartHeroes = { "10001", "10002", "10003", "10004", "10005", };
    public static ObscuredDouble StartGold = 100f;
    public static ObscuredFloat LvUpFactor = 1.2f;

    public static double GetLvUpCost(int lv)
    {
        return StartGold * Mathf.Pow(LvUpFactor, lv);
    }

}
