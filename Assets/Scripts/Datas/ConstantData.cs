using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public static class ConstantData
{
	public static ObscuredString[] StartHeroes = { "10001", "10002", "10003", "10004", "10005", };
	public static ObscuredDouble StartGold = 100f;
	public static ObscuredFloat LvUpGoldGR = 1.2f;
	public static ObscuredFloat HeroAtkGR = 1.2f;
	public static ObscuredFloat EnemyHpGR = 1.3f;	
	public static ObscuredFloat EnemyGoldGR = 1.2f;
	public static ObscuredInt PossibleAscensionStage = 2;
	public static ObscuredDouble AscensionBasicReward = 50f;
	public static ObscuredFloat AscensionRewardFactor = 1.1f;

	public static double GetLvUpCost(int lv)
	{
		return CalcValue(StartGold, LvUpGoldGR, lv);		
	}

	public static double GetHeroAtk(double basicAtk, int lv)
	{
		return CalcValue(basicAtk, HeroAtkGR, lv);
	}

	public static double GetEnemyHp(double basicHp, int stageNo, bool isBoss)
	{
		if (isBoss)
		{

		}
		else
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
		return CalcValue(AscensionBasicReward, AscensionRewardFactor, stageNo - PossibleAscensionStage);
	}

	public static double GetHeroUpgradeCost(int currentGrade)
	{
		return currentGrade * 100f;
	}

	static double CalcValue(double beginValue, float growthRate, int lv)
	{
		return Math.Round(beginValue * (Mathf.Pow(growthRate, lv) - 1) / (growthRate - 1));
	}

}
