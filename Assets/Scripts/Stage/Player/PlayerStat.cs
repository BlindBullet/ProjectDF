using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

public class PlayerStat
{
	public ObscuredInt StartStage;
	public ObscuredFloat AddGold;
	public ObscuredFloat AddMagicite;
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


	public void Init()
	{
		StartStage = 1;
		AddGold = 0;
		AddMagicite = 0;
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

	}

}
