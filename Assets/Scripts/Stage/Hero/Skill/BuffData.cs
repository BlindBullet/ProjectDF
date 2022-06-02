using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuffData
{
	public HitresultChart Data;	
	public float DurationTime;
	
	public void SetData(HitresultChart chart)
	{
		Data = chart;
		DurationTime = chart.DurationTime <= 0 ? 99999f : chart.DurationTime;
	}


}