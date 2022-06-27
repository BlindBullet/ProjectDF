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
	public AttackData AtkData = new AttackData();
	public List<int> PowerUpStackDatas = new List<int>();
	public List<int> PowerUpListLvs = new List<int>();
	public List<AtkUpgradeType> PowerUpLists = new List<AtkUpgradeType>();	

	public void Init(int no)
	{
		No = no;
		Lv = 1;
		Power = 0;		
		PowerUpStackDatas.Clear();
		PowerUpListLvs.Clear();
		PowerUpLists.Clear();		
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
			if (PowerUpStackDatas[i] == lv)
				return;
		}
				
		PowerUpStackDatas.Add(lv);		
	}

	public void SetLotteriedUpgradeBars(int lv, List<AtkUpgradeType> upgrades)
	{		
		for(int i = 0; i < PowerUpListLvs.Count; i++)
		{
			if (PowerUpListLvs[i] == lv)
			{
				PowerUpLists = upgrades;
				PowerUpListLvs[i] = lv;
				return;
			}	
		}

		PowerUpListLvs.Add(lv);
		PowerUpLists = upgrades;
	}

	public void PowerUp(int lv, AtkUpgradeType selectType)
	{
		for(int i = 0; i < PowerUpListLvs.Count; i++)
		{
			if (PowerUpListLvs[i] == lv)
				PowerUpListLvs.Remove(PowerUpListLvs[i]);
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

		if(PowerUpStackDatas.Count > 0)
			PowerUpStackDatas.RemoveAt(0);

		Power++;		
	}

}