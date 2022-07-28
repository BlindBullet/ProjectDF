using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentData
{
	public string Id;
	public int EnchantLv;
	public int Count;
	public bool isOpen;
	public EquipmentType Type;
	
	public EquipmentData(string id, EquipmentType type)
	{
		Id = id;
		EnchantLv = 0;
		Count = 0;
		isOpen = false;
		Type = type;
	}

	public void EnchantLvUp()
	{
		EnchantLv++;
	}

	public void AddCount(int count)
	{
		Count += count;
	}

	public void Open()
	{
		isOpen = true;
	}

}
