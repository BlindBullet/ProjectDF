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

	public void Dispatch(DateTime startTime, List<HeroData> heroes)
	{
		IsDiapatch = true;
		StartTime = startTime;
		Debug.Log(startTime);

		for(int i = 0; i < heroes.Count; i++)
		{
			DispatchHeroes.Add(heroes[i].Id);
		}
	}

	public void CheckCompelete()
	{
		if (!IsDiapatch)
			return;

		QuestChart chart = CsvData.Ins.QuestChart[Id];

		double totalTime = chart.Time * 60f;
		TimeSpan timeSpan = TimeManager.Ins.ReceivedTime - StartTime;
		double progressTime = timeSpan.TotalSeconds + TimeManager.Ins.SinceTime;

		if (progressTime >= totalTime)
			IsComplete = true;
	}

}
