using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
	public int No;
	public int Lv;	
	public int Power;
	public int PowerUpStack;
	public AttackData AtkData = new AttackData();
	public List<PowerUpStackData> PowerUpStackDatas = new List<PowerUpStackData>();
	public List<SlotPowerUpList> PowerUpList = new List<SlotPowerUpList>();

	public void Init(int no)
	{
		No = no;
		Lv = 1;
		Power = 0;
		PowerUpStack = 0;
		PowerUpStackDatas.Clear();
		PowerUpList.Clear();
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

	public void IncUpgradeStack(int lv)
	{
		for(int i = 0; i < PowerUpStackDatas.Count; i++)
		{
			if (PowerUpStackDatas[i].Lv == lv)
				return;
		}

		PowerUpStackData data = new PowerUpStackData(lv);
		PowerUpStackDatas.Add(data);
	}

	public void SetLotteriedUpgradeBars(int lv, List<int> lotteriedNos)
	{		
		for(int i = 0; i < PowerUpList.Count; i++)
		{
			if (PowerUpList[i].Lv == lv)
			{
				PowerUpList[i].ChangeValue(lv, lotteriedNos);
				return;
			}	
		}

		PowerUpList.Add(new SlotPowerUpList(lv, lotteriedNos));		
	}

	public void PowerUp(int lv, AtkUpgradeType selectType)
	{
		for(int i = 0; i < PowerUpList.Count; i++)
		{
			if (PowerUpList[i].Lv == lv)
				PowerUpList.Remove(PowerUpList[i]);
		}

		switch (selectType)
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
			case AtkUpgradeType.Bounce:
				AtkData.Bounce++;
				break;
			case AtkUpgradeType.AtkUp:
				AtkData.AtkUp++;
				break;
			case AtkUpgradeType.Size:
				AtkData.Size++;
				break;
			case AtkUpgradeType.Push:
				AtkData.Push++;
				break;
		}

		PowerUpStackDatas.RemoveAt(0);
		Power++;
	}

}

public class SlotPowerUpList
{
	public int Lv;
	public List<int> LotteriedPowerUpBars = new List<int>();

	public SlotPowerUpList(int lv, List<int> lotteriedNos)
	{
		Lv = lv;
		LotteriedPowerUpBars = lotteriedNos;
	}

	public void ChangeValue(int lv, List<int> lotteriedNos)
	{
		Lv = lv;
		LotteriedPowerUpBars = lotteriedNos;
	}
}

public class PowerUpStackData
{
	public int Lv;	

	public PowerUpStackData(int lv)
	{
		Lv = lv;
	}
}