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
    
    public void Init()
	{
        Gold = 0;
        Gem = 0;
        Stage = 1;    
        
        Save();
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
