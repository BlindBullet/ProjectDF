using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public static class ConstantData
{
	public static ObscuredString[] StartHeroes = { "10001", "10002", "10003", "10004", "10005", };
	public static ObscuredDouble StartLvUpGold = 10f;
	public static ObscuredFloat LvUpGoldGR = 1.25f;
	public static ObscuredDouble StartEnchantCost = 10f;
	public static ObscuredFloat EnchantGR = 1.25f;
	public static ObscuredFloat HeroAtkGR = 1.2f;
	public static ObscuredFloat EnemyHpGR = 1.3f;	
	public static ObscuredFloat EnemyGoldGR = 1.15f;
	public static ObscuredInt PossibleAscensionStage = 10;
	public static ObscuredDouble AscensionBasicReward = 50f;
	public static ObscuredFloat AscensionRewardFactor = 1.1f;
	public static ObscuredInt[] QuestLvPerClearCount = { 5, 20, 40, 60, 80, 100, 150,  };
	public static ObscuredDouble TimeGold = 10f;
	public static ObscuredInt KillEnemiesCount1Min = 15;

	public static double GetLvUpCost(int lv)
	{		
		return CalcValue(StartLvUpGold, LvUpGoldGR, lv);		
	}

	public static double GetHeroAtk(double basicAtk, int lv, int enchantLv)
	{
		return CalcValue(basicAtk, HeroAtkGR, lv + enchantLv);
	}

	public static double GetEnemyHp(double basicHp, int stageNo, bool isBoss)
	{
		if (isBoss)
		{

		}

		return CalcValue(basicHp, EnemyHpGR, stageNo);		
	}

	public static double GetEnemyGold(double basicGold, int stageNo, bool isBoss)
	{
		if (isBoss)
		{

		}
		else
		{

		}

		return CalcValue(basicGold, EnemyGoldGR, stageNo);
	}

	public static double GetAscensionMagicite(int stageNo)
	{
		return CalcValue(AscensionBasicReward, AscensionRewardFactor, stageNo - PossibleAscensionStage + 1);
	}

	public static double GetHeroUpgradeCost(int currentGrade)
	{
		return currentGrade * 100f;
	}

	public static double GetHeroEnchantCost(int currentEnchantLv)
	{
		return CalcValue(StartEnchantCost, EnchantGR, currentEnchantLv + 1);
	}

	public static double GetGoldFromTime(float min, int stageNo)
	{
		return CalcValue(TimeGold, EnemyGoldGR, stageNo) * KillEnemiesCount1Min * min;
	}

	static double CalcValue(double beginValue, float growthRate, int lv)
	{
		return Math.Round(beginValue * (Mathf.Pow(growthRate, lv) - 1) / (growthRate - 1));
	}

}
