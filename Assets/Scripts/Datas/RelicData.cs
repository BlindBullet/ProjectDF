using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RelicData
{
	public string Id;
	public int Lv;
	public bool isOwn;

	public void Init(string id)
	{
		Id = id;
		Lv = 1;
		isOwn = false;
	}

	public bool Puechase()
	{
		RelicChart chart = CsvData.Ins.RelicChart[Id];
		double cost = chart.Price;

		switch (chart.PriceCostType)
		{
			case CostType.Gold:
				if (StageManager.Ins.PlayerData.Gold >= cost)
				{
					StageManager.Ins.ChangeGold(-cost);
					isOwn = true;
					return true;
				}
				break;
			case CostType.Magicite:
				if (StageManager.Ins.PlayerData.Magicite >= cost)
				{
					StageManager.Ins.ChangeMagicite(-cost);
					isOwn = true;
					return true;
				}
				break;
			case CostType.SoulStone:
				if (StageManager.Ins.PlayerData.SoulStone >= cost)
				{
					StageManager.Ins.ChangeSoulStone(-cost);
					isOwn = true;
					return true;
				}
				break;
		}

		return false;
	}

	public bool LevelUp()
	{
		RelicChart chart = CsvData.Ins.RelicChart[Id];

		if (Lv >= chart.MaxLv)
			return false;

		double cost = chart.LvUpCost * (1 + Mathf.Pow(chart.LvUpCostIncRate, Lv));

		if (StageManager.Ins.PlayerData.Magicite >= cost)
		{
			StageManager.Ins.ChangeMagicite(-cost);
			Lv++;
			return true;
		}

		return false;
	}

}
