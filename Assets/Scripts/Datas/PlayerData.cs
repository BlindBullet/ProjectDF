using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

[System.Serializable]
public class PlayerData
{    
	public double Gold;
	public double Magicite;
	public double Gem;
	public int Stage;
	public List<SlotData> Slots = new List<SlotData>();
	public List<HeroData> Heroes = new List<HeroData>();	

	public void Init()
	{
		Gold = 0;
		Magicite = 0;
		Gem = 500;
		Stage = 1;
		ResisterHeroes();

		for(int i = 0; i < 5; i++)
		{
			SlotData data = new SlotData();
			data.Init(i + 1);
			Slots.Add(data);
		}

		for (int i = 0; i < ConstantData.StartHeroes.Length; i++)
		{
			AddHero(ConstantData.StartHeroes[i]);
		}

		Save();
	}

	public void AddHero(string id)
	{
		for(int i = 0; i < Heroes.Count; i++)
		{
			if (Heroes[i].Id == id)
				Heroes[i].IsOwn = true;
		}

		Save();
	}

	void ResisterHeroes()
	{
		foreach(KeyValuePair<string, List<HeroChart>> elem in CsvData.Ins.HeroChart)
		{
			bool alreadyOwn = false;

			for(int i = 0; i < Heroes.Count; i++)
			{
				if (elem.Key == Heroes[i].Id)
					alreadyOwn = true;
			}

			if (!alreadyOwn)
			{
				HeroData data = new HeroData();
				data.Init(elem.Value);
				Heroes.Add(data);
			}   
		}
	}

	public bool SummonHero(HeroData data, double cost)
	{
		if(Gem >= cost)
		{			
			for (int i = 0; i < Heroes.Count; i++)
			{
				if (Heroes[i] == data)
					Heroes[i].IsOwn = true;
			}

			Save();
			return true;
		}
		else
		{
			return false;
		}
	}

	public void DeployHero(HeroData data, int slotNo)
	{
		for(int i = 0; i < Heroes.Count; i++)
		{
			if (Heroes[i].Id == data.Id)
			{
				Heroes[i].DeploySlotNo = slotNo;
			}
			else if(Heroes[i].DeploySlotNo == slotNo)
			{
				Heroes[i].DeploySlotNo = -1;
			}
		}

		//¿µ¿õ ¹èÄ¡...
	}

	public void ChangeGold(double amount)
	{
		Gold += amount;
		Save();
	}

	public void ChangeGem(double amount)
	{
		Gem += amount;
		Save();
	}

	public void ChangeMagicite(double amount)
	{
		Magicite += amount;
		Save();
	}

	public void NextStage()
	{
		Stage++;
		Save();
	}

	public void Save()
	{   
		ES3.Save<PlayerData>("PlayerData", this);
	}

	public void Load()
	{
		PlayerData data = ES3.Load<PlayerData>("PlayerData", defaultValue: null);

		if(data == null)
		{
			Init();           
		}
		else
		{            
			Gold = data.Gold;
			Gem = data.Gem;
			Stage = data.Stage;
		}
	}

}
