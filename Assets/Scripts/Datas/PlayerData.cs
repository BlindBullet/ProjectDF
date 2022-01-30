using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int Gold;
    public int Stage;
    public int AtkLv;
    public int DefLv;
    public int GoldLv;   

    public void Init()
	{
        Gold = 0;
        Stage = 1;
        AtkLv = 1;
        DefLv = 1;
        GoldLv = 1;

        
        Save();
	}

    public void ChangeGold(int amount)
	{
        Gold += amount;
        Save();
	}

    public void AtkLvUp()
	{
        AtkLv++;
        Save();
	}

    public void DefLvUp()
	{
        DefLv++;
        Save();
	}

    public void GoldLvUp()
	{
        GoldLv++;
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
            Stage = data.Stage;
            AtkLv = data.AtkLv;
            DefLv = data.DefLv;
            GoldLv = data.GoldLv;        
        }
	}

}
