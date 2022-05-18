using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogAdReward : DialogController
{
	public TextMeshProUGUI Title;
	public RewardIcon RewardIcon;
	public TextMeshProUGUI RewardDesc;
	public Button GetBtn;
	public TextMeshProUGUI GetBtnText;
	public Button AdBtn;
	public TextMeshProUGUI AdBtnText;
	
	public void OpenDialog(SuppliesChart chart)
	{
		GetBtn.gameObject.SetActive(false);

		SetBasic();

		double rewardValue = chart.RewardValue;

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				rewardValue = ConstantData.GetGoldFromTime(chart.RewardValue, StageManager.Ins.PlayerData.Stage);
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.CalcValue);
				RewardDesc.text = LanguageManager.Ins.SetString("Bonus") + ": " + rewardValue.ToCurrencyString() + " " + LanguageManager.Ins.SetString("Gold");
				break;
			case RewardType.SoulStone:				
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.CalcValue);
				RewardDesc.text = LanguageManager.Ins.SetString("Bonus") + ": " + rewardValue + " " + LanguageManager.Ins.SetString("SoulStone");
				break;
			case RewardType.GameSpeed:				
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.Time);
				RewardDesc.text = LanguageManager.Ins.SetString("Bonus") + ": " + LanguageManager.Ins.SetString("GameSpeed2x") + " " + rewardValue + LanguageManager.Ins.SetString("Minute");
				break;
		}

		AdBtnText.text = LanguageManager.Ins.SetString("get_ad_reward");
		AdBtn.onClick.RemoveAllListeners();
		AdBtn.onClick.AddListener(() => 
		{
			SendReward(chart);			
			CloseDialog();
		});

		Show(false);
	}

	public void OpenDialog(QuestChart chart)
	{
		CloseBtn.gameObject.SetActive(false);

		SetBasic();

		double rewardValue = chart.RewardValue;

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				rewardValue = ConstantData.GetGoldFromTime(chart.RewardValue, StageManager.Ins.PlayerData.Stage);
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.CalcValue);
				RewardDesc.text = LanguageManager.Ins.SetString("Gold") + " " + rewardValue.ToCurrencyString();
				break;
			case RewardType.SoulStone:				
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.CalcValue);
				RewardDesc.text = LanguageManager.Ins.SetString("SoulStone") + " " + rewardValue;
				break;
			case RewardType.GameSpeed:				
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.Time);
				RewardDesc.text = LanguageManager.Ins.SetString("GameSpeed2x") + " " + rewardValue + LanguageManager.Ins.SetString("Minute");
				break;
		}

		GetBtnText.text = LanguageManager.Ins.SetString("Claim");
		GetBtn.onClick.RemoveAllListeners();
		GetBtn.onClick.AddListener(() => { SendReward(chart); CloseDialog(); });

		AdBtnText.text = LanguageManager.Ins.SetString("get_ad_reward_x2");
		AdBtn.onClick.RemoveAllListeners();
		AdBtn.onClick.AddListener(() =>
		{
			SendReward(chart, true);			
			CloseDialog();
		});

		Show(false);
	}

	public override void SetCloseBtn()
	{
		base.SetCloseBtn();		
	}

	void SetBasic()
	{
		Title.text = LanguageManager.Ins.SetString("Reward");
	}

	void SendReward(SuppliesChart chart)
	{
		double rewardValue = chart.RewardValue;

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				rewardValue = ConstantData.GetGoldFromTime(chart.RewardValue, StageManager.Ins.PlayerData.Stage);
				StageManager.Ins.ChangeGold(rewardValue);
				break;
			case RewardType.SoulStone:
				StageManager.Ins.ChangeMagicite(chart.RewardValue);
				break;
			case RewardType.GameSpeed:
				StageManager.Ins.AddPlayerBuff(PlayerBuffType.GameSpeed, chart.RewardValue);
				break;
		}

		DialogManager.Ins.OpenReceiveReward(chart.RewardType, rewardValue);
	}

	void SendReward(QuestChart chart, bool isAd = false)
	{
		double rewardValue = chart.RewardValue;

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				rewardValue = ConstantData.GetGoldFromTime(chart.RewardValue, StageManager.Ins.PlayerData.Stage);
				rewardValue = isAd ? rewardValue * 2f : rewardValue;
				StageManager.Ins.ChangeGold(rewardValue);
				break;
			case RewardType.SoulStone:
				StageManager.Ins.ChangeMagicite(chart.RewardValue);
				rewardValue = isAd ? rewardValue * 2f : rewardValue;
				break;
			case RewardType.GameSpeed:
				rewardValue = isAd ? rewardValue * 2f : rewardValue;
				StageManager.Ins.AddPlayerBuff(PlayerBuffType.GameSpeed, rewardValue);
				break;
		}

		DialogManager.Ins.OpenReceiveReward(chart.RewardType, rewardValue);
	}

}
