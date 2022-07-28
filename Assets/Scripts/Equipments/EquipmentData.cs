using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentData
{
	public string Id;
	public int Lv;
	public int Count;

	public EquipmentData(string id)
	{
		Id = id;
		Lv = 0;
		Count = 0;
	}

	public void LvUp()
	{
		Lv++;
	}

	public void AddCount(int count)
	{
		Count += count;
	}

}
