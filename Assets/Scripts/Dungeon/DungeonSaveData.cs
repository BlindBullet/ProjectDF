using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DungeonSaveData
{
	public int TicketCount;
	public DateTime TicketChargeStartTime;
	public float TicketChargeLeftTime;	
	public int CurDungeonLv;
	public int TopDungeonLv;

	public void Init()
	{
		TicketCount = 5;
		TicketChargeStartTime = DateTime.UtcNow;
		TicketChargeLeftTime = 0;
		CurDungeonLv = 1;
		TopDungeonLv = 1;

		Save();
	}

	public void UseTicket()
	{
		if (TicketCount == 5)
			TicketChargeStartTime = TimeManager.Ins.GetCurrentTime();

		TicketCount--;
		Save();
	}

	public void CheckDungeonTicketAdd(double sec)
	{
		int addCount = (int)(sec / (ConstantData.DungeonEnterTicketAddTime * 60));
		double leftSec = Math.Round(sec % (ConstantData.DungeonEnterTicketAddTime * 60));		
		TicketCount += addCount;

		if (TicketCount > ConstantData.DungeonEnterMaxTicketCount)
		{
			TicketCount = ConstantData.DungeonEnterMaxTicketCount;
		}
		else
		{
			TicketChargeStartTime = TimeManager.Ins.GetCurrentTime().AddSeconds(-leftSec);			
		}
		
		Save();		
	}

	public void SetCurDungeonLv(int no)
	{
		CurDungeonLv = no;
		Save();
	}

	public void SetTopDungeonLv(int no)
	{
		if(no > TopDungeonLv)
			TopDungeonLv = no;

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
			TicketCount = data.TicketCount;
			TicketChargeStartTime = data.TicketChargeStartTime;
			TicketChargeLeftTime = data.TicketChargeLeftTime;
			CurDungeonLv = data.CurDungeonLv;
			TopDungeonLv = data.TopDungeonLv;
		}
				
		Save();
	}
}
