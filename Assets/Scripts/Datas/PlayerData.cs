using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System;

[System.Serializable]
public class PlayerData
{
	public bool IsFirstPlay;
	public int PlayAppCount;
	public double Gold;
	public double Magicite;
	public double Gem;
	public int Stage;
	public List<SlotData> Slots = new List<SlotData>();
	public List<HeroData> Heroes = new List<HeroData>();	

	public void Init()
	{
		IsFirstPlay = true;
		PlayAppCount = 1;
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

		Save();
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

	public void ChangeStage(int count)
	{
		Stage += count;
		Save();
	}

	public void IncAppCount()
	{
		PlayAppCount++;
		Save();
	}

	public void RunFirstPlay()
	{
		IsFirstPlay = false;
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
			PlayAppCount = data.PlayAppCount;
			Gold = data.Gold;
			Magicite = data.Magicite;
			Gem = data.Gem;
			Stage = data.Stage;
			Heroes = data.Heroes;
			Slots = data.Slots;
		}
	}

	
}
