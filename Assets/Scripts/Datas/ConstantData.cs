using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public static class ConstantData
{
	//시작 영웅 ID
	public static ObscuredString[] StartHeroes = { "10001", "10002", "10003", "10004", "10005", };
	//레벨업 골드 시작 비용
	public static ObscuredDouble StartLvUpGold = 10f;
	//레벨업 골드 비용 증가
	public static ObscuredFloat LvUpGoldGR = 1.09f;
	//영웅 강화 시작 비용
	public static ObscuredDouble StartEnchantCost = 10f;
	//영웅 강화 비용 증가
	public static ObscuredFloat EnchantGR = 1.2f;
	//영웅 레벨업시 공격력 증가
	public static ObscuredFloat HeroAtkGR = 1.08f;
	//적 체력 증가
	public static ObscuredFloat EnemyHpGR = 1.17f;
	//적 처치 골드 증가
	public static ObscuredFloat EnemyGoldGR = 1.04f;
	//환생 가능 스테이지
	public static ObscuredInt PossibleAscensionStage = 30;
	//환생 시작 리워드
	public static ObscuredDouble AscensionBasicReward = 200f;
	//환생 리워드 증가
	public static ObscuredFloat AscensionRewardFactor = 1.1f;
	//퀘스트 클리어 카운트에 따른 퀘스트 레벨 증가
	public static ObscuredInt[] QuestLvPerClearCount = { 5, 20, 40, 60, 80, 100, 150,  };
	//시간을 메인으로 했을 때 획득 골드 (퀘스트 혹은 보급품 등의 보상) (적 처치 골드 증가와 연계됨)
	public static ObscuredDouble TimeGold = 15f;
	//오프라인 혹은 퀘스트 완료시 적이 1분당 몇명을 잡는 것으로 칠것인가
	public static ObscuredInt KillEnemiesCount1Min = 20;	
	//오프라인시 획득 골드 (적 처치 골드 및 분당 처치수와 연계됨)
	public static ObscuredDouble OfflineTimeGold = 2.5f;
	//퀘스트탭이 열릴 환생 카운트
	public static ObscuredInt OpenQuestAscensionCount = 0;
	//슬롯 파워업 가능 레벨
	public static ObscuredInt[] SlotPowerUpPossibleLv = { 5, 25, 50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
	//게임스피드 버프의 증가량
	public static ObscuredFloat BuffGameSpeedRate = 1.5f;
	//얻는 골드량 버프의 증가량
	public static ObscuredFloat BuffGainGoldRate = 2f;
	//기본 전방공격이 활성화됐을 때 감소하는 데미지량(%)
	public static ObscuredFloat FrontDmgDecP = 25f;
	//기본 사선공격이 활성화됐을 때 감소하는 데미지량(%)
	public static ObscuredFloat DiagonalDmgDecP = 25f;
	//기본공격이 튕길 때마다 감소하는 데미지량(%)
	public static ObscuredFloat BounceDmgDecP = 35f;
	//기본공격이 관통할 때마다 감소하는 데미지량(%)
	public static ObscuredFloat PiercingDmgDecP = 20f;
	//기본공격의 총알 크기가 커짐(%)
	public static ObscuredFloat SizeIncP = 250f;
	//일반 몬스터를 처치했을 때 영혼석이 떨어질 확률
	public static ObscuredFloat NormalEnemyDropSoulStoneRate = 0.01f;
	//일반 몬스터를 처치했을 때 떨어질 최대 영혼석
	public static ObscuredDouble NormalEnemyDropSoulStoneMaxCount = 1;
	//보스 몬스터를 처치했을 때 영혼석이 떨어질 확률
	public static ObscuredFloat BossEnemyDropSoulStoneRate = 1f;
	//보스 몬스터를 처치했을 때 떨어질 최대 영혼석
	public static ObscuredDouble BossEnemyDropSoulStoneMaxCount = 3;
	//퀘스트 리셋이 가능한 시간(초)
	public static ObscuredInt QuestResetPossibleSec = 180;
	//파워업 리셋 가격
	public static ObscuredDouble PowerUpRefreshCost = 10;
	//보급품이 나오기 시작할 스테이지
	public static ObscuredInt SuppliesAppearPossibleStage = 7;
	
	public static double GetLvUpCost(int lv)
	{
		double value = CalcValue(StartLvUpGold, LvUpGoldGR, lv);
		value = value + (value * (StageManager.Ins.PlayerStat.LvUpGold / 100f));
			
		return value;		
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
		double value = CalcValue(basicGold, EnemyGoldGR, stageNo);
		value = value + (value * BuffGainGoldRate);
		return value;
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
