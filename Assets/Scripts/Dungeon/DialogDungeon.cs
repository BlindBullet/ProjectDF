using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogDungeon : DialogController
{
	public TextMeshProUGUI TitleText;
	public Transform DungeonBarTrf;
	public TextMeshProUGUI DungeonTicketText;
	
	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("Dungeon");
		SetDungeonBars();
		SetDungeonTicketInfo();
		Show(true);
	}

	void SetDungeonBars()
	{
		foreach(KeyValuePair<string, DungeonChart> elem in CsvData.Ins.DungeonChart)
		{
			var bar = Instantiate(Resources.Load("Prefabs/Bars/DungeonBar") as GameObject, DungeonBarTrf);
			bar.GetComponent<DungeonBar>().SetBar(elem.Value);
		}
	}

	public void SetDungeonTicketInfo()
	{
		if(StageManager.Ins.DungeonData.DungeonEnterCount >= ConstantData.DungeonEnterMaxTicketCount)
		{
			DungeonTicketText.text = StageManager.Ins.DungeonData.DungeonEnterCount + " / " + ConstantData.DungeonEnterMaxTicketCount;
		}
		else
		{
			StartCoroutine(TicketChargeSeq());
		}
	}

	IEnumerator TicketChargeSeq()
	{	
		while (true)
		{
			TimeSpan timeSpan = TimeManager.Ins.GetCurrentTime() - StageManager.Ins.DungeonData.TicketChargeStartTime;

			if(timeSpan.TotalMinutes >= ConstantData.DungeonEnterTicketAddTime)
			{
				StageManager.Ins.DungeonData.CheckDungeonTicketAdd(timeSpan.TotalSeconds);
				timeSpan = TimeManager.Ins.GetCurrentTime() - StageManager.Ins.DungeonData.TicketChargeStartTime;
				DungeonTicketText.text = StageManager.Ins.DungeonData.DungeonEnterCount + " / " + ConstantData.DungeonEnterMaxTicketCount;

				if (StageManager.Ins.DungeonData.DungeonEnterCount >= ConstantData.DungeonEnterMaxTicketCount)
					yield break;
			}
			
			int time = (int)((ConstantData.DungeonEnterTicketAddTime * 60) - timeSpan.TotalSeconds);			
			int hour = time / 3600;
			int min = time % 3600 / 60;
			int sec = time % 3600 % 60;

			string hourStr = "0" + hour;
			string minStr = min < 10 ? "0" + min : min.ToString();
			string secStr = sec < 10 ? "0" + sec : sec.ToString();

			DungeonTicketText.text = StageManager.Ins.DungeonData.DungeonEnterCount + " / " + ConstantData.DungeonEnterMaxTicketCount
				+ " (" + hourStr + ":" + minStr + ":" + secStr + ")";

			yield return new WaitForSeconds(1f);			
		}

		
	}

}
