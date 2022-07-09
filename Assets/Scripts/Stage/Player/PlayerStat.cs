using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class PlayerStat
{
	public ObscuredInt StartStage;
	public ObscuredFloat AscensionReward;
	public ObscuredDouble AscensionGold;
	public ObscuredFloat GetSoulStoneRate;
	public Dictionary<Attr, ObscuredFloat> NormalEnemyGoldInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> NormalEnemyGoldDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemyGoldInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemyGoldDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> NormalEnemyHpInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> NormalEnemyHpDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemyHpInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemyHpDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> NormalEnemySpdInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> NormalEnemySpdDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemySpdInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemySpdDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> NormalEnemyDefInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> NormalEnemyDefDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemyDefInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> BossEnemyDefDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> MinionAtkInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> MinionAtkDec = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> MinionSpdInc = new Dictionary<Attr, ObscuredFloat>();
	public Dictionary<Attr, ObscuredFloat> MinionSpdDec = new Dictionary<Attr, ObscuredFloat>();
	public ObscuredInt OfflineRewardLimitMin;
	public ObscuredFloat OfflineRewardAdd;
	public ObscuredFloat QuestTime;
	public ObscuredFloat QuestReward;
	public ObscuredFloat UseAutoSkillRate = 0f;
	public ObscuredFloat MoatSlowRate = 0f;
	public ObscuredFloat LvUpGold = 0f;
	public ObscuredFloat TouchSkillCoolDecProb = 0f;
	public ObscuredFloat GainGold = 1f;
	public ObscuredBool UseAutoSkill = false;
	public ObscuredFloat GameSpd = 1.25f;
	public ObscuredBool RemoveAd = false;

	public void Init()
	{
		StartStage = 1;
		AscensionReward = 0f;
		AscensionGold = 0f;
		GetSoulStoneRate = 0;

		foreach (Attr attr in Enum.GetValues(typeof(Attr)))
		{
			NormalEnemyGoldInc[attr] = 0f;
			NormalEnemyGoldDec[attr] = 0f;
			BossEnemyGoldInc[attr] = 0f;
			BossEnemyGoldDec[attr] = 0f;
			NormalEnemyHpInc[attr] = 0f;
			NormalEnemyHpDec[attr] = 0f;
			BossEnemyHpInc[attr] = 0f;
			BossEnemyHpDec[attr] = 0f;
			NormalEnemySpdInc[attr] = 0f;
			NormalEnemySpdDec[attr] = 0f;
			BossEnemySpdInc[attr] = 0f;
			BossEnemySpdDec[attr] = 0f;
			NormalEnemyDefInc[attr] = 0f;
			NormalEnemyDefDec[attr] = 0f;
			BossEnemyDefInc[attr] = 0f;
			BossEnemyDefDec[attr] = 0f;
			MinionAtkInc[attr] = 0f;
			MinionAtkDec[attr] = 0f;
			MinionSpdInc[attr] = 0f;
			MinionSpdDec[attr] = 0f;
		}
				
		OfflineRewardLimitMin = 120;
		OfflineRewardAdd = 0f;
		QuestTime = 0f;		
		QuestReward = 0f;
		UseAutoSkillRate = 0f;
		MoatSlowRate = 0f;
		LvUpGold = 0f;
		TouchSkillCoolDecProb = 0f;
		GameSpd = 1.25f;
		UseAutoSkill = false;
		GainGold = 1f;
		RemoveAd = true;
		Time.timeScale = GameSpd;
	}

	public void CheckRemoveAd()
	{
		if (IAPManager.Ins.HadPurchased("remove_ad"))
			RemoveAd = true;
		else
			RemoveAd = false;
	}

}
