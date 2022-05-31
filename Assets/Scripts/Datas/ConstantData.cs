using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public static class ConstantData
{
	//���� ���� ID
	public static ObscuredString[] StartHeroes = { "10001", "10002", "10003", "10004", "10005", };
	//������ ��� ���� ���
	public static ObscuredDouble StartLvUpGold = 10f;
	//������ ��� ��� ����
	public static ObscuredFloat LvUpGoldGR = 1.2f;
	//���� ��ȭ ���� ���
	public static ObscuredDouble StartEnchantCost = 10f;
	//���� ��ȭ ��� ����
	public static ObscuredFloat EnchantGR = 1.2f;
	//���� �������� ���ݷ� ����
	public static ObscuredFloat HeroAtkGR = 1.15f;
	//�� ü�� ����
	public static ObscuredFloat EnemyHpGR = 1.2f;
	//�� óġ ��� ����
	public static ObscuredFloat EnemyGoldGR = 1.1f;
	//ȯ�� ���� ��������
	public static ObscuredInt PossibleAscensionStage = 30;
	//ȯ�� ���� ������
	public static ObscuredDouble AscensionBasicReward = 50f;
	//ȯ�� ������ ����
	public static ObscuredFloat AscensionRewardFactor = 1.1f;
	//����Ʈ Ŭ���� ī��Ʈ�� ���� ����Ʈ ���� ����
	public static ObscuredInt[] QuestLvPerClearCount = { 5, 20, 40, 60, 80, 100, 150,  };
	//�ð��� �������� ���� �� ȹ�� ��� (�� óġ ��� ������ �����)
	public static ObscuredDouble TimeGold = 2.5f;
	//�������� Ȥ�� ����Ʈ �Ϸ�� ���� 1�д� ����� ��� ������ ĥ���ΰ�
	public static ObscuredInt KillEnemiesCount1Min = 10;
	//�÷��̾� �� ���� HitFx
	public static ObscuredString PlayerTouchAtkHitFx = "TestHitFx";
	//�������ν� ȹ�� ��� (�� óġ ��� �� �д� óġ���� �����)
	public static ObscuredDouble OfflineTimeGold = 2.5f;
	//����Ʈ���� ���� ȯ�� ī��Ʈ
	public static ObscuredInt OpenQuestAscensionCount = 0;
	//���� �Ŀ��� ���� ����
	public static ObscuredInt[] SlotPowerUpPossibleLv = { 5, 10, 25, 50, 100, 200, 300, 400, 500, 600, };	

	public static double GetLvUpCost(int lv)
	{		
		return CalcValue(StartLvUpGold, LvUpGoldGR, lv);		
	}

	public static double GetHeroAtkEnchant(double basicAtk, int enchantLv)
	{		
		return CalcValue(basicAtk, HeroAtkGR, enchantLv + 1);
	}

	public static double GetHeroAtk(double basicAtk, int lv, int enchantLv)
	{
		return CalcValue(GetHeroAtkEnchant(basicAtk, enchantLv), HeroAtkGR, lv);
	}

	public static double GetEnemyHp(double basicHp, int stageNo, bool isBoss)
	{		
		return CalcValue(basicHp, EnemyHpGR, stageNo);
	}

	public static double GetEnemyGold(double basicGold, int stageNo, bool isBoss)
	{		
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

	public static double GetGoldFromOfflineTime(double min, int stageNo)
	{
		return CalcValue(OfflineTimeGold, EnemyGoldGR, stageNo) * KillEnemiesCount1Min * min;
	}

	static double CalcValue(double beginValue, float growthRate, int lv)
	{
		return Math.Round(beginValue * (Mathf.Pow(growthRate, lv) - 1) / (growthRate - 1));
	}

}
