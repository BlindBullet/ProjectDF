using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogAdReward : DialogController
{
	public static DialogAdReward _Dialog = null;

	public TextMeshProUGUI Title;
	public RewardIcon RewardIcon;
	public TextMeshProUGUI RewardDesc;
	public Button GetBtn;
	public TextMeshProUGUI GetBtnText;
	public Button AdBtn;
	public TextMeshProUGUI AdBtnText;
	
	public void OpenDialog(SuppliesChart chart)
	{
		_Dialog = this;
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
			SoundManager.Ins.PlaySFX("se_button_2");
			AdmobManager.Ins.ShowSuppliesAdReward(chart);			
		});

		Show(false);
	}

	public void OpenDialog(QuestChart chart)
	{
		_Dialog = this;
		CloseBtn.gameObject.SetActive(false);

		SetBasic();

		double rewardValue = chart.RewardValue;

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				rewardValue = ConstantData.GetGoldFromTime(chart.RewardValue, StageManager.Ins.PlayerData.Stage);
				rewardValue = rewardValue + (rewardValue * (StageManager.Ins.PlayerStat.QuestReward / 100f));
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.CalcValue);
				RewardDesc.text = LanguageManager.Ins.SetString("Gold") + " " + rewardValue.ToCurrencyString();
				break;
			case RewardType.SoulStone:
				rewardValue = rewardValue + (rewardValue * (StageManager.Ins.PlayerStat.QuestReward / 100f));
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.CalcValue);
				RewardDesc.text = LanguageManager.Ins.SetString("SoulStone") + " " + rewardValue;
				break;
			case RewardType.GameSpeed:
				rewardValue = rewardValue + (rewardValue * (StageManager.Ins.PlayerStat.QuestReward / 100f));
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.Time);
				RewardDesc.text = LanguageManager.Ins.SetString("GameSpeedInc") + " " + rewardValue + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.UseAutoSkill:
				rewardValue = rewardValue + (rewardValue * (StageManager.Ins.PlayerStat.QuestReward / 100f));
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.Time);
				RewardDesc.text = LanguageManager.Ins.SetString("UseAutoSkill") + " " + rewardValue + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.GainGold:
				rewardValue = rewardValue + (rewardValue * (StageManager.Ins.PlayerStat.QuestReward / 100f));
				RewardIcon.SetIcon(chart.RewardType, -1, RewardValueShowType.Time);
				RewardDesc.text = LanguageManager.Ins.SetString("GainGold") + " " + rewardValue + LanguageManager.Ins.SetString("Minute");
				break;
		}

		GetBtnText.text = LanguageManager.Ins.SetString("Claim");
		GetBtn.onClick.RemoveAllListeners();
		GetBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); SendReward(chart); CloseDialog(); });

		AdBtnText.text = LanguageManager.Ins.SetString("get_ad_reward_x2");
		AdBtn.onClick.RemoveAllListeners();
		AdBtn.onClick.AddListener(() =>
		{
			SoundManager.Ins.PlaySFX("se_button_2");
			AdmobManager.Ins.ShowQuestRewardAd(chart);
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

	public IEnumerator GetReward(SuppliesChart chart)
	{
		yield return null;

		SendReward(chart);
	}

	public void SendReward(SuppliesChart chart)
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
			case RewardType.UseAutoSkill:
				StageManager.Ins.AddPlayerBuff(PlayerBuffType.UseAutoSkill, chart.RewardValue);
				break;
			case RewardType.GainGold:
				StageManager.Ins.AddPlayerBuff(PlayerBuffType.GainGold, chart.RewardValue);
				break;
		}

		DialogManager.Ins.OpenReceiveReward(chart.RewardType, rewardValue);
		CloseDialog();
	}

	public IEnumerator GetReward(QuestChart chart, bool isAd = false)
	{
		yield return null;

		SendReward(chart, isAd);
	}

	public void SendReward(QuestChart chart, bool isAd = false)
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
			case RewardType.UseAutoSkill:
				rewardValue = isAd ? rewardValue * 2f : rewardValue;
				StageManager.Ins.AddPlayerBuff(PlayerBuffType.UseAutoSkill, rewardValue);
				break;
			case RewardType.GainGold:
				rewardValue = isAd ? rewardValue * 2f : rewardValue;
				StageManager.Ins.AddPlayerBuff(PlayerBuffType.GainGold, rewardValue);
				break;
		}

		DialogManager.Ins.OpenReceiveReward(chart.RewardType, rewardValue);
		CloseDialog();
		SEManager.Ins.Apply();
	}

	private void OnDisable()
	{
		_Dialog = null;
	}
}
