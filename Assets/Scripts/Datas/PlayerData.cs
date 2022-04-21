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
	public double SoulStone;
	public int Stage;
	public int AscensionCount;
	public List<SlotData> Slots = new List<SlotData>();
	public List<HeroData> Heroes = new List<HeroData>();
	public List<RelicData> Relics = new List<RelicData>();


	public void Init()
	{
		IsFirstPlay = true;
		PlayAppCount = 1;
		Gold = 0;
		Magicite = 0f;
		SoulStone = 500f;
		Stage = 1;
		AscensionCount = 0;
		ResisterHeroes();
		ResisterRelics();

		for (int i = 0; i < 5; i++)
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
		if(SoulStone >= cost)
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

	public bool UpgradeHero(HeroData data)
	{		
		double cost = ConstantData.GetHeroUpgradeCost(data.Grade);

		if (SoulStone >= cost)
		{
			for (int i = 0; i < Heroes.Count; i++)
			{
				if (Heroes[i] == data)
					Heroes[i].Upgrade();
			}

			Save();
			return true;
		}
		else
		{
			return false;
		}
	}

	void ResisterRelics()
	{
		foreach(KeyValuePair<string, RelicChart> elem in CsvData.Ins.RelicChart)
		{
			bool alreadyOwn = false;

			for(int i = 0; i < Relics.Count; i++)
			{
				if (elem.Key == Relics[i].Id)
					alreadyOwn = true;
			}

			if (!alreadyOwn)
			{
				RelicData data = new RelicData();
				data.Init(elem.Key);
				Relics.Add(data);
			}
		}
	}

	public void ChangeGold(double amount)
	{
		Gold += amount;
		Save();
	}

	public void ChangeSoulStone(double amount)
	{
		SoulStone += amount;
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

	public void Ascension()
	{		
		Stage = StageManager.Ins.PlayerStat.StartStage;

		Gold = 0;

		Slots.Clear();

		for (int i = 0; i < 5; i++)
		{
			SlotData data = new SlotData();
			data.Init(i + 1);
			Slots.Add(data);
		}

		AscensionCount++;
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
			SoulStone = data.SoulStone;
			Stage = data.Stage;
			Heroes = data.Heroes;
			Relics = data.Relics;
			Slots = data.Slots;

			ResisterHeroes();
			ResisterRelics();
		}

		Save();
	}

	
}
