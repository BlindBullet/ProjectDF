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
	public bool isEquip;

	public void Init(string id, EquipmentType type)
	{
		Id = id;
		EnchantLv = 1;
		Count = 0;
		isOpen = false;
		isEquip = false;
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

	public void Equip()
	{
		isEquip = true;
	}

	public void UnEquip()
	{
		isEquip = false;
	}

}
