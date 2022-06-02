using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerBuffManager : SingletonObject<PlayerBuffManager>
{
	public UnityAction<double> GameSpeedBuffAdded;
	public UnityAction<double> AutoSkillBuffAdded;
	public UnityAction<double> GainGoldBuffAdded;

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
				StageManager.Ins.PlayerStat.GameSpd = ConstantData.BuffGameSpeedRate;
				break;
			case PlayerBuffType.UseAutoSkill:
				StageManager.Ins.PlayerStat.AutoUseSKill = true;
				break;
			case PlayerBuffType.GainGoldx2:
				StageManager.Ins.PlayerStat.GainGold = ConstantData.BuffGainGoldRate;
				break;
		}

		StartCoroutine(BuffSequence(data));
	}

	void EndBuff(PlayerBuffData data)
	{
		switch (data.Type)
		{
			case PlayerBuffType.GameSpeed:
				StageManager.Ins.PlayerStat.GameSpd = 1f;
				break;
			case PlayerBuffType.UseAutoSkill:
				StageManager.Ins.PlayerStat.AutoUseSKill = false;
				break;
			case PlayerBuffType.GainGoldx2:
				StageManager.Ins.PlayerStat.GainGold = 1f;
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
				case PlayerBuffType.UseAutoSkill:
					AutoSkillBuffAdded((data.DurationTime * 60f) - span.TotalSeconds);
					break;
				case PlayerBuffType.GainGoldx2:
					GainGoldBuffAdded((data.DurationTime * 60f) - span.TotalSeconds);
					break;
			}

			if (span.TotalSeconds >= data.DurationTime * 60f)
			{
				switch (data.Type)
				{
					case PlayerBuffType.GameSpeed:
						GameSpeedBuffAdded((data.DurationTime * 60f) - span.TotalSeconds);
						break;
					case PlayerBuffType.UseAutoSkill:
						AutoSkillBuffAdded((data.DurationTime * 60f) - span.TotalSeconds);
						break;
					case PlayerBuffType.GainGoldx2:
						GainGoldBuffAdded((data.DurationTime * 60f) - span.TotalSeconds);
						break;
				}

				EndBuff(data);
				break;
			}

			yield return new WaitForSeconds(1f);
		}
	}


}
