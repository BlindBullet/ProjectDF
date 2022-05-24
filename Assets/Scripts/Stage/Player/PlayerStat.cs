using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class PlayerStat
{
	public ObscuredInt StartStage;
	public ObscuredFloat AddEnemyGold;
	public ObscuredFloat AddAscensionReward;
	public ObscuredFloat GetSoulStoneRate;
	public ObscuredFloat EnemyHpInc;
	public ObscuredFloat EnemyHpDec;
	public ObscuredFloat EnemySpdInc;
	public ObscuredFloat EnemySpdDec;
	public ObscuredFloat MinionHpInc;
	public ObscuredFloat MinionHpDec;
	public ObscuredFloat MinionSpdInc;
	public ObscuredFloat MinionSpdDec;
	public ObscuredFloat BossHpInc;
	public ObscuredFloat BossHpDec;
	public ObscuredFloat BossSpdInc;
	public ObscuredFloat BossSpdDec;
	public ObscuredInt OfflineRewardLimitMin;
	public ObscuredFloat OfflineRewardAdd;

	public void Init()
	{
		StartStage = 1;
		AddEnemyGold = 0;
		AddAscensionReward = 0;
		GetSoulStoneRate = 0;
		EnemyHpInc = 0;
		EnemyHpDec = 0;
		EnemySpdInc = 0;
		EnemySpdDec = 0;
		MinionHpInc = 0;
		MinionHpDec = 0;
		MinionSpdInc = 0;
		MinionSpdDec = 0;
		BossHpInc = 0;
		BossHpDec = 0;
		BossSpdInc = 0;
		BossSpdDec = 0;
		OfflineRewardLimitMin = 120;
		OfflineRewardAdd = 0f;
	}

}
