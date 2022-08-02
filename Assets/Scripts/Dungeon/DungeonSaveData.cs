using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonSaveData
{
	public int DungeonEnterCount;
	public DateTime DungeonEnterTime;
	public bool IsDungeonOpen;

	public void Init()
	{
		DungeonEnterCount = 5;
		DungeonEnterTime = DateTime.UtcNow;

		if(StageManager.Ins.PlayerData.TopStage >= 200)
		{
			IsDungeonOpen = true;
		}
		else
		{
			IsDungeonOpen = false;
		}

		Save();
	}

	public void Save()
	{
		ES3.Save<DungeonSaveData>("DungeonData", this);
	}

	public void Load()
	{
		DungeonSaveData data = ES3.Load<DungeonSaveData>("DungeonData", defaultValue: null);

		if (data == null)
		{
			Init();
		}
		else
		{
			DungeonEnterCount = data.DungeonEnterCount;
			DungeonEnterTime = data.DungeonEnterTime;

			if (StageManager.Ins.PlayerData.TopStage >= 200)
			{
				IsDungeonOpen = true;
			}
			else
			{
				IsDungeonOpen = false;
			}
		}
				
		Save();
	}
}
