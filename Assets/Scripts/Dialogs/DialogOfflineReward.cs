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

		string hour = (StageManager.Ins.PlayerStat.OfflineRewardLimitMin / 60).ToString();
		string min = (StageManager.Ins.PlayerStat.OfflineRewardLimitMin % 60).ToString();

		OfflineRewardDesc.text = string.Format(LanguageManager.Ins.SetString("desc_popup_offline_reward"), hour, min);
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

		if (timeSpan.TotalMinutes > StageManager.Ins.PlayerStat.OfflineRewardLimitMin)
			timeSpan = new TimeSpan(0, StageManager.Ins.PlayerStat.OfflineRewardLimitMin, 0);

		string hour = timeSpan.Hours.ToString();
		string min = timeSpan.Minutes.ToString();

		OfflineTimeText.text = string.Format(LanguageManager.Ins.SetString("time_popup_offline_reward"), hour, min);

		double rewardValue = ConstantData.GetGoldFromOfflineTime(timeSpan.TotalMinutes, StageManager.Ins.PlayerData.Stage);
		double addRewardValue = rewardValue * (StageManager.Ins.PlayerStat.OfflineRewardAdd / 100f);

		if(addRewardValue > 0)		
			RewardDesc.text = string.Format(LanguageManager.Ins.SetString("offline_reward"), rewardValue.ToCurrencyString(), addRewardValue.ToCurrencyString());		
		else		
			RewardDesc.text = string.Format(LanguageManager.Ins.SetString("offline_reward_no_add"), rewardValue.ToCurrencyString());		

		GetBtn.onClick.RemoveAllListeners();
		GetBtn.onClick.AddListener(() => {
			StageManager.Ins.ChangeGold(rewardValue + addRewardValue);
			DialogManager.Ins.OpenReceiveReward(RewardType.Gold, rewardValue + addRewardValue);
			CloseDialog();
		});

		AdGetBtn.onClick.RemoveAllListeners();
		AdGetBtn.onClick.AddListener(() => 
		{
			rewardValue = rewardValue * 2f;
			StageManager.Ins.ChangeGold(rewardValue + addRewardValue);
			DialogManager.Ins.OpenReceiveReward(RewardType.Gold, rewardValue + addRewardValue);
			CloseDialog();
		});
	}

}
