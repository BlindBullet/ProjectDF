using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogOfflineReward : DialogController
{
	public TextMeshProUGUI TitleText;
	public TextMeshProUGUI OfflineTimeText;
	public TextMeshProUGUI OfflineRewardDesc;
	public RewardIcon RewardIcon;
	public TextMeshProUGUI RewardDesc;
	public Button GetBtn;
	public TextMeshProUGUI GetBtnText;
	public Button AdGetBtn;
	public TextMeshProUGUI AdGetBtnText;	

	public void OpenDialog()
	{		
		TitleText.text = LanguageManager.Ins.SetString("title_popup_offline_reward");
		OfflineRewardDesc.text = LanguageManager.Ins.SetString("desc_popup_offline_reward");
		RewardIcon.SetIcon(RewardType.Gold, -1, RewardValueShowType.CalcValue);
		GetBtnText.text = LanguageManager.Ins.SetString("Get");
		AdGetBtnText.text = LanguageManager.Ins.SetString("Get_2x");
		SetOfflineTime();

		Show(false);
	}

	void SetOfflineTime()
	{
		TimeSpan timeSpan = TimeManager.Ins.GetCurrentTime() - StageManager.Ins.PlayerData.OfflineStartTime;

		if (timeSpan.TotalMinutes <= 1f)
			CloseDialog();

		string hour = timeSpan.Hours < 10 ? "0" + timeSpan.Hours.ToString() : timeSpan.Hours.ToString();
		string min = timeSpan.Minutes < 10 ? "0" + timeSpan.Minutes.ToString() : timeSpan.Minutes.ToString();

		OfflineTimeText.text = string.Format(LanguageManager.Ins.SetString("time_popup_offline_reward"), hour, min);

		double rewardValue = ConstantData.GetGoldFromOfflineTime(timeSpan.TotalMinutes, StageManager.Ins.PlayerData.Stage);
		RewardDesc.text = string.Format(LanguageManager.Ins.SetString("offline_reward"), rewardValue.ToCurrencyString());

		GetBtn.onClick.RemoveAllListeners();
		GetBtn.onClick.AddListener(() => {
			StageManager.Ins.ChangeGold(rewardValue);
			DialogManager.Ins.OpenReceiveReward(RewardType.Gold, rewardValue);
			CloseDialog();
		});

		AdGetBtn.onClick.RemoveAllListeners();
		AdGetBtn.onClick.AddListener(() => 
		{
			rewardValue = rewardValue * 2f;
			StageManager.Ins.ChangeGold(rewardValue);
			DialogManager.Ins.OpenReceiveReward(RewardType.Gold, rewardValue);
			CloseDialog();
		});
	}

}
