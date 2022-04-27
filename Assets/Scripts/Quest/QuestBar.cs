using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestBar : MonoBehaviour
{
	public TextMeshProUGUI Name;
	public RewardIcon RewardIcon;
	public Image[] Stars;
	public TextMeshProUGUI QuestTimeText;
	public GameObject QuestProgressBar;
	public Image QuestProgressBarFill;
	public TextMeshProUGUI QuestProgressText;
	public Button AchieveBtn;
	public Button DisPatchBtn;
	QuestData data;

	public void SetBar(QuestData data)
	{
		this.data = data;
		QuestChart chart = CsvData.Ins.QuestChart[data.Id];
		Name.text = LanguageManager.Ins.SetString(chart.Name);
		RewardIcon.SetIcon(chart);
		SetStars(chart);
		SetDispatchInfo(data, chart);		
	}

	void SetStars(QuestChart chart)
	{
		for(int i = 0; i < chart.Lv; i++)
		{
			Stars[i].sprite = Resources.Load<Sprite>("Sprites/Icons/Star_On");
		}
	}

	void SetDispatchInfo(QuestData data, QuestChart chart)
	{
		if (data.IsDiapatch)
		{
			if (data.IsComplete)
			{				
				QuestTimeText.gameObject.SetActive(false);
				QuestProgressBar.SetActive(false);
				DisPatchBtn.gameObject.SetActive(false);

				AchieveBtn.gameObject.SetActive(true);
				AchieveBtn.onClick.RemoveAllListeners();
				AchieveBtn.onClick.AddListener(() => 
				{
					QuestManager.Ins.ClearQuest(data);
					DialogQuest.Quest.SetQuests();
				});
			}
			else
			{
				QuestProgressBar.SetActive(true);
				QuestTimeText.gameObject.SetActive(false);
				DateTime startTime = data.StartTime;				
				TimeSpan timeSpan = DateTime.UtcNow - startTime;				
				StartCoroutine(TimeProgress(chart.Time, timeSpan));

				AchieveBtn.gameObject.SetActive(false);
				DisPatchBtn.gameObject.SetActive(false);
			}
		}
		else
		{
			QuestProgressBar.SetActive(false);
			QuestTimeText.gameObject.SetActive(true);

			int hour = chart.Time / 60;
			int min = chart.Time % 60;

			QuestTimeText.text = LanguageManager.Ins.SetString("QuestTime") + ": " + hour + LanguageManager.Ins.SetString("Hour") + " " + min + LanguageManager.Ins.SetString("Minute");
			AchieveBtn.gameObject.SetActive(false);
			DisPatchBtn.gameObject.SetActive(true);
			DisPatchBtn.onClick.RemoveAllListeners();
			DisPatchBtn.onClick.AddListener(() => 
			{
				DialogManager.Ins.OpenQuestInfo(data);
			});
		}
	}

	IEnumerator TimeProgress(int totalMin, TimeSpan timeSpan)
	{
		double totalSec = totalMin * 60f;
		double progressSec = Math.Round(totalSec - timeSpan.TotalSeconds);
		
		while (true)
		{
			int hour = (int)(progressSec / 3600f);
			int min = (int)((progressSec % 3600f) / 60f);
			int sec = (int)((progressSec % 3600f) % 60f);

			string hourStr = hour < 10 ? "0" + hour : hour.ToString();
			string minStr = min < 10 ? "0" + min : min.ToString();
			string secStr = sec < 10 ? "0" + sec : sec.ToString();

			QuestProgressText.text = hourStr + ":" + minStr + ":" + secStr;
			QuestProgressBarFill.fillAmount = (float)((totalSec - progressSec) / totalSec);

			yield return new WaitForSeconds(1f);
						
			progressSec -= 1f;			

			if (progressSec <= 0f)
			{				
				QuestProgressText.text = hourStr + ":" + minStr + ":" + secStr;
				QuestProgressBarFill.fillAmount = (float)((totalSec - progressSec) / totalSec);

				StageManager.Ins.PlayerData.CheckAllQuestComplete();
				SetBar(data);
				yield break;
			}
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

}
