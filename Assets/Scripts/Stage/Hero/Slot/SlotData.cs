using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
	public int No;
	public int Lv;
	public int UpgradeLv;
	public int UpgradeLimit;
	public AttackData AtkData;

	public void Init(int no)
	{
		No = no;
		Lv = 1;
		UpgradeLv = 0;
		UpgradeLimit = 2;
		AtkData = new AttackData();
	}

	public bool LevelUp()
	{
		double cost = ConstantData.GetLvUpCost(Lv);

		if (StageManager.Ins.PlayerData.Gold >= cost)
		{
			Lv++;
			StageManager.Ins.ChangeGold(-cost);
			return true;
		}

		return false;
	}

	public bool IncUpgradeLimit()
	{
		if (StageManager.Ins.PlayerData.SoulStone >= 200)
		{
			UpgradeLimit++;
			StageManager.Ins.ChangeSoulStone(-200);
			return true;
		}

		return false;
	}

	public void Upgrade(AtkUpgradeType type)
	{
		switch (type)
		{
			case AtkUpgradeType.Front:
				AtkData.Front++;
				break;
			case AtkUpgradeType.Diagonal:
				AtkData.Diagonal++;
				break;
			case AtkUpgradeType.Piercing:
				AtkData.Piercing++;
				break;
			case AtkUpgradeType.Multi:
				AtkData.Multi++;
				break;
			case AtkUpgradeType.Boom:
				AtkData.Boom++;
				break;
			case AtkUpgradeType.AtkUp:
				AtkData.AtkUp++;
				break;
		}

		UpgradeLv++;
	}



}
