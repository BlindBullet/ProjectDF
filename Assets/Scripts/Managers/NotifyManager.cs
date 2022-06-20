using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.SimpleAndroidNotifications;

public class NotifyManager : MonoSingleton<NotifyManager>
{
	private void OnApplicationPause(bool isPause)
	{		
#if UNITY_ANDROID
		if (isPause)
		{
			CallNotify();
		}

#endif
	}

	public void CallNotify()
	{
#if UNITY_ANDROID

		// 등록된 알림 모두 제거
		NotificationManager.CancelAll();		
		RunQuestNotify();
		RunOfflineNotify();
#endif
	}


	void RunQuestNotify()
	{
		PlayerData data = StageManager.Ins.PlayerData;

		for(int i = 0; i < data.Quests.Count; i++)
		{
			if (!data.Quests[i].IsComplete && data.Quests[i].IsDiapatch)
			{
				QuestChart chart = CsvData.Ins.QuestChart[data.Quests[i].Id];
				DateTime completeTime = data.Quests[i].StartTime.AddMinutes(chart.Time);
				TimeSpan delay = completeTime - TimeManager.Ins.GetCurrentTime();
				string title = LanguageManager.Ins.SetString("notify_title_quest");
				string desc = LanguageManager.Ins.SetString("notify_desc_quest");
				NotificationManager.SendWithAppIcon(delay, title, desc, Color.yellow, NotificationIcon.Star);
			}
		}
	}

	void RunOfflineNotify()
	{
		PlayerData data = StageManager.Ins.PlayerData;

		DateTime completeTime = TimeManager.Ins.GetCurrentTime().AddMinutes(StageManager.Ins.PlayerStat.OfflineRewardLimitMin);
		TimeSpan delay = completeTime - TimeManager.Ins.GetCurrentTime();
		string title = LanguageManager.Ins.SetString("notify_title_offline");
		string desc = LanguageManager.Ins.SetString("notify_desc_offline");
		NotificationManager.SendWithAppIcon(delay, title, desc, Color.yellow, NotificationIcon.Star);		
	}




}