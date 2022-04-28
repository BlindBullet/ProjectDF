using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerBuffManager : SingletonObject<PlayerBuffManager>
{
	public UnityAction<double> GameSpeedBuffAdded;

	public void AddBuff(PlayerBuffType type, double durationTime)
	{
		StageManager.Ins.PlayerData.AddBuff(type, durationTime, TimeManager.Ins.GetCurrentTime());
		RunAllBuffs();
	}

	public void RunAllBuffs()
	{
		List<PlayerBuffData> buffs = StageManager.Ins.PlayerData.PlayerBuffs;

		for(int i = 0; i < buffs.Count; i++)
		{			
			StartBuff(buffs[i]);
		}
	}

	void StartBuff(PlayerBuffData data)
	{		
		switch (data.Type)
		{
			case PlayerBuffType.GameSpeed:
				Time.timeScale = 2f;
				break;
		}

		StartCoroutine(BuffSequence(data));
	}

	void EndBuff(PlayerBuffData data)
	{
		switch (data.Type)
		{
			case PlayerBuffType.GameSpeed:
				Time.timeScale = 1f;
				break;
		}

		StageManager.Ins.PlayerData.RemoveBuff(data);
	}

	IEnumerator BuffSequence(PlayerBuffData data)
	{
		while (true)
		{
			TimeSpan span = TimeManager.Ins.GetCurrentTime() - data.StartTime;
			
			switch (data.Type)
			{
				case PlayerBuffType.GameSpeed:
					GameSpeedBuffAdded((data.DurationTime * 60f) - span.TotalSeconds);
					break;
			}

			if (span.TotalSeconds >= data.DurationTime * 60f)
			{
				switch (data.Type)
				{
					case PlayerBuffType.GameSpeed:
						GameSpeedBuffAdded((data.DurationTime * 60f) - span.TotalSeconds);
						break;
				}

				EndBuff(data);
				break;
			}

			yield return new WaitForSeconds(1f);
		}
	}


}
