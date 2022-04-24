using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class QuestData
{
	public string Id;
	public DateTime StartTime;
	public List<string> DispatchHeroes = new List<string>();
	public bool IsDiapatch;
	public bool IsComplete;

	public void Init(QuestChart chart)
	{
		Id = chart.Id;
		StartTime = DateTime.Now;
		IsDiapatch = false;
		IsComplete = false;
	}

	public void ChangeQuest(QuestChart chart)
	{
		Id = chart.Id;
		StartTime = DateTime.Now;
		IsDiapatch = false;
		IsComplete = false;
		DispatchHeroes.Clear();
	}
}
