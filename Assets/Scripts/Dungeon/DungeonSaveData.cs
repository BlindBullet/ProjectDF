using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonSaveData
{
	public int DungeonEnterCount;
	public DateTime TicketChargeStartTime;
	public float TicketChargeLeftTime;
	public bool IsDungeonOpen;

	public void Init()
	{
		DungeonEnterCount = 5;
		TicketChargeStartTime = DateTime.UtcNow;
		TicketChargeLeftTime = 0;

		if(StageManager.Ins.PlayerData.TopStage > ConstantData.DungeonOpenStage)
		{
			IsDungeonOpen = true;
		}
		else
		{
			IsDungeonOpen = false;
		}

		Save();
	}

	public void UseTicket()
	{
		if (DungeonEnterCount == 5)
			TicketChargeStartTime = TimeManager.Ins.GetCurrentTime();

		DungeonEnterCount--;
		Save();
	}

	public void CheckDungeonTicketAdd(double sec)
	{
		int addCount = (int)(sec / (ConstantData.DungeonEnterTicketAddTime * 60));
		double leftSec = Math.Round(sec % (ConstantData.DungeonEnterTicketAddTime * 60));		
		DungeonEnterCount += addCount;

		if (DungeonEnterCount > ConstantData.DungeonEnterMaxTicketCount)
		{
			DungeonEnterCount = ConstantData.DungeonEnterMaxTicketCount;
		}
		else
		{
			TicketChargeStartTime = TimeManager.Ins.GetCurrentTime().AddSeconds(-leftSec);			
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
			TicketChargeStartTime = data.TicketChargeStartTime;
			TicketChargeLeftTime = data.TicketChargeLeftTime;

			if (StageManager.Ins.PlayerData.TopStage > ConstantData.DungeonOpenStage)
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
