using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class HeroData
{
	public string Id;
	public int Grade;
	public int EnchantLv;
	public bool IsOwn = false;
	public int SlotNo;
	public float CurCT;
	
	public void Init(List<HeroChart> chart)
	{
		HeroChart data = chart[0];

		Id = chart[0].Id;
		Grade = chart[0].Grade;
		EnchantLv = 0;		
		IsOwn = false;
		SlotNo = -1;
		CurCT = 0f;
	}

	public void Upgrade()
	{
		Grade++;
	}

	public void EnchantLvUp()
	{
		EnchantLv++;
	}

	public void DeployHero(int slotNo)
	{
		SlotNo = slotNo;
	}

	public void ReleaseHero()
	{
		SlotNo = -1;
	}

	public void SaveCurCoolTime(float value)
	{
		CurCT = value;
	}

}
