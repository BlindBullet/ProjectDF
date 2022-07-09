using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
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
	public int TopStage;
	public int AscensionCount;
	public int TouchAttackLv;
	public List<SlotData> Slots = new List<SlotData>();
	public List<HeroData> Heroes = new List<HeroData>();
	public List<RelicData> Relics = new List<RelicData>();
	public List<RelicData> Castles = new List<RelicData>();
	public List<QuestData> Quests = new List<QuestData>();
	public List<PlayerBuffData> PlayerBuffs = new List<PlayerBuffData>();
	public int QuestLv;
	public int ClearQuestCount;
	public int TotalClearQuestCount;
	public bool OnBGM;
	public bool OnSFX;
	public bool OnDmgText;
	public DateTime OfflineStartTime;
	public int TutorialStep = 0;
	public DateTime QuestResetStartTime;
	public int CheckLv = 1;
	public int CheckCount = 0;
	public DateTime CheckStartTime;
	public bool AutoLvUp = false;

	public void Init()
	{
		IsFirstPlay = true;
		PlayAppCount = 1;
		Gold = 0f;
		Magicite = 0f;
		SoulStone = 0f;
		Stage = 1;
		TopStage = 1;
		AscensionCount = 0;
		QuestLv = 1;
		ClearQuestCount = 0;
		TotalClearQuestCount = 0;
		TouchAttackLv = 1;
		OnBGM = true;
		OnSFX = true;
		OnDmgText = true;
		OfflineStartTime = TimeManager.Ins.GetCurrentTime();
		TutorialStep = 0;
		QuestResetStartTime = TimeManager.Ins.GetCurrentTime();
		CheckLv = 1;
		CheckCount = 0;
		CheckStartTime = TimeManager.Ins.GetCurrentTime();
		AutoLvUp = false;

		ResisterHeroes();
		ResisterRelics();
		ResisterQuests();
		ResisterCastle();

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

	private void ResisterQuests()
	{
		int count = 0;

		foreach(KeyValuePair<string, QuestChart> elem in CsvData.Ins.QuestChart)
		{
			if(elem.Value.Lv == 1)
			{				
				QuestData data = new QuestData();
				data.Init(elem.Value);
				Quests.Add(data);
				count++;
			}

			if (count >= 3)
				break;
		}
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

	public bool SummonHero(HeroData data, HeroChart chart)
	{
		bool result = false;

		switch (chart.CostType)
		{
			case CostType.Gold:
				if (Gold >= chart.Cost)
				{
					result = true;
					StageManager.Ins.ChangeGold(-chart.Cost);
				}
				break;
			case CostType.Magicite:
				if (Magicite >= chart.Cost)
				{
					result = true;
					StageManager.Ins.ChangeMagicite(-chart.Cost);
				}
				break;
			case CostType.SoulStone:
				if (SoulStone >= chart.Cost)
				{
					result = true;
					StageManager.Ins.ChangeSoulStone(-chart.Cost);
				}				
				break;
		}

		if (result)
		{
			for (int i = 0; i < Heroes.Count; i++)
			{
				if (Heroes[i] == data)
					Heroes[i].IsOwn = true;
			}
		}

		Save();

		return result;
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

	public bool EnchantHero(HeroData data)
	{
		double cost = ConstantData.GetHeroEnchantCost(data.EnchantLv);

		if(Magicite >= cost)
		{
			for(int i = 0; i < Heroes.Count; i++)
			{
				if (Heroes[i] == data)
					Heroes[i].EnchantLvUp();
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
				if (elem.Key == Relics[i].Id && elem.Value.Type == RelicType.Relic)
					alreadyOwn = true;
			}

			if (!alreadyOwn && elem.Value.Type == RelicType.Relic)
			{
				RelicData data = new RelicData();
				data.Init(elem.Key);
				Relics.Add(data);
			}
		}
	}

	void ResisterCastle()
	{
		foreach (KeyValuePair<string, RelicChart> elem in CsvData.Ins.RelicChart)
		{
			bool alreadyOwn = false;

			for (int i = 0; i < Castles.Count; i++)
			{
				if (elem.Key == Castles[i].Id && elem.Value.Type == RelicType.Castle)
				{	
					alreadyOwn = true;
				}
			}

			if (!alreadyOwn && elem.Value.Type == RelicType.Castle)
			{
				RelicData data = new RelicData();
				data.Init(elem.Key);
				Castles.Add(data);
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

		if (Stage < 1)
			Stage = 1;

		if (TopStage < Stage)
		{
			TopStage = Stage;
			GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_top_stage, TopStage);
		}	

		Save();
	}

	public void ClearQuest(int lv)
	{
		if(QuestLv <= lv)
		{
			ClearQuestCount++;
			TotalClearQuestCount++;
		}

		Save();
	}

	public void QuestLvUp()
	{	
		QuestLv++;
		ClearQuestCount = 0;
		Save();
	}

	public void Ascension()
	{		
		Stage = StageManager.Ins.PlayerStat.StartStage;
		Gold = ConstantData.GetAscensionGold();
		
		Slots.Clear();

		for (int i = 0; i < 5; i++)
		{
			SlotData data = new SlotData();
			data.Init(i + 1);
			Slots.Add(data);
		}

		AscensionCount++;
		GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_ascension, AscensionCount);
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

	public void CheckAllQuestComplete()
	{
		for(int i = 0; i < Quests.Count; i++)
		{
			Quests[i].CheckCompelete();
		}
	}

	public void AddBuff(PlayerBuffType type, double durationTime, DateTime startTime)
	{
		bool alreadyHave = false;

		for(int i = 0; i < PlayerBuffs.Count; i++)
		{
			if (PlayerBuffs[i].Type == type)
			{
				alreadyHave = true;
				PlayerBuffs[i].AddDurationTime(durationTime);
			}	
		}

		if (!alreadyHave)
		{
			PlayerBuffData data = new PlayerBuffData();
			data.Init(type, startTime, durationTime);
			PlayerBuffs.Add(data);
		}

		Save();
	}

	public void RemoveBuff(PlayerBuffData data)
	{
		PlayerBuffs.Remove(data);
		Save();
	}

	public void SetBGM(bool isOn)
	{
		OnBGM = isOn;
		Save();
	}

	public void SetSFX(bool isOn)
	{
		OnSFX = isOn;
		Save();
	}

	public void SetDmgText(bool isOn)
	{
		OnDmgText = isOn;
		Save();
	}

	public void SetOfflineTime()
	{
		OfflineStartTime = TimeManager.Ins.GetCurrentTime();
		Save();
	}

	public void SetQuestResetTime()
	{
		QuestResetStartTime = TimeManager.Ins.GetCurrentTime();
		Save();
	}

	public void IncTutorialStep()
	{
		TutorialStep++;
		Save();
	}

	public bool IncCheckCount()
	{
		bool result = false;

		CheckCount++;
		CheckStartTime = TimeManager.Ins.GetCurrentTime();

		if (CheckCount >= 15)
		{			
			CheckLvUp();
			result = true;
		}	

		Save();
		return result;
	}

	void CheckLvUp()
	{
		CheckLv++;
		CheckCount = 0;
	}

	public void SetAutoLvUp()
	{
		if (AutoLvUp)
		{
			AutoLvUp = false;
		}
		else
		{
			AutoLvUp = true;
		}

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
			IsFirstPlay = data.IsFirstPlay;
			PlayAppCount = data.PlayAppCount;
			Gold = data.Gold;
			Magicite = data.Magicite;
			SoulStone = data.SoulStone;
			TouchAttackLv = data.TouchAttackLv;
			AscensionCount = data.AscensionCount;
			QuestLv = data.QuestLv;
			ClearQuestCount = data.ClearQuestCount;
			TotalClearQuestCount = data.TotalClearQuestCount;
			Stage = data.Stage;
			TopStage = data.Stage;
			Heroes = data.Heroes;

			for(int i = 0; i < Heroes.Count; i++)
			{
				Heroes[i].CurCT = 0f;
			}

			Relics = data.Relics;
			Slots = data.Slots;
			Quests = data.Quests;
			Castles = data.Castles;
			PlayerBuffs = data.PlayerBuffs;
			OnBGM = data.OnBGM;
			OnSFX = data.OnSFX;
			OnDmgText = data.OnDmgText;
			OfflineStartTime = data.OfflineStartTime;
			TutorialStep = data.TutorialStep;
			QuestResetStartTime = data.QuestResetStartTime;

			if (data.CheckLv == 0)
				CheckLv = 1;
			else
				CheckLv = data.CheckLv;
						
			CheckCount = data.CheckCount;
			CheckStartTime = data.CheckStartTime;
			AutoLvUp = data.AutoLvUp;

			ResisterHeroes();
			ResisterRelics();
			ResisterCastle();
		}

		Save();
	}

	
}
