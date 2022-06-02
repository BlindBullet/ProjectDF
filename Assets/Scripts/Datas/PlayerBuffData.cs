using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBuffData
{
	public PlayerBuffType Type;
	public DateTime StartTime;
	public double DurationTime;	

	public void Init(PlayerBuffType type, DateTime startTime, double durationTime)
	{
		Type = type;
		StartTime = startTime;
		DurationTime = durationTime;		
	}

	public void AddDurationTime(double addTime)
	{
		DurationTime += addTime;
	}
	
}

public enum PlayerBuffType
{
	None,
	GameSpeed,
	UseAutoSkill,
	GainGoldx2,


}