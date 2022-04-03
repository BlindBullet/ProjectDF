using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class PlayerData
{    
    public double Gold;
    public double Gem;
    public int Stage;
    public List<HeroData> Heroes = new List<HeroData>();

    public void Init()
	{
        Gold = 0;
        Gem = 0;
        Stage = 1;
        ResisterHeroes();

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
        foreach(KeyValuePair<string, HeroChart> elem in CsvData.Ins.HeroChart)
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
                data.Init(elem.Key);                
                Heroes.Add(data);
            }   
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
