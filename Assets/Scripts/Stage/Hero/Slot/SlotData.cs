using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
	public int No;
	public int Lv;
	
	public void Init(int no)
	{
		No = no;
		Lv = 1;
	}

	public bool LevelUp()
	{
		double cost = ConstantData.GetLvUpCost(Lv);

		if (StageManager.Ins.PlayerData.Gold >= cost)
		{
			StageManager.Ins.ChangeGold(-cost);
			Lv++;            
			return true;
		}

		return false;
	}

}
