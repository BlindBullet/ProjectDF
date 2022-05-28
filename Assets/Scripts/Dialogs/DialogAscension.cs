using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogAscension : DialogController
{
	public TextMeshProUGUI Title;
	public TextMeshProUGUI Desc;
	public TextMeshProUGUI RewardTitle;
	public TextMeshProUGUI RewardAmount;
	public TextMeshProUGUI StartStageDesc;
	public Button AdAscensionBtn;
	public TextMeshProUGUI AdAscensionBtnText;
	public TextMeshProUGUI AdAscensionBtnRewardText;
	public Button AscensionBtn;
	public TextMeshProUGUI AscensionBtnText;

	public void OpenDialog()
	{
		Title.text = LanguageManager.Ins.SetString("Ascension");
		Desc.text = LanguageManager.Ins.SetString("popup_ascension_desc");
		RewardTitle.text = LanguageManager.Ins.SetString("popup_ascension_reward_title");		

		bool isPossibleAscension = false;

		if(StageManager.Ins.PlayerData.Stage >= ConstantData.PossibleAscensionStage)
		{
			isPossibleAscension = true;
		}

		SetReward(isPossibleAscension);
		SetAscensionBtn(isPossibleAscension);
		Show(true);
	}

	void SetReward(bool isPossibleAscension)
	{
		if (!isPossibleAscension)
		{
			RewardAmount.text = "???";
			StartStageDesc.text = string.Format(LanguageManager.Ins.SetString("popup_ascension_cant_ascension_desc"), ConstantData.PossibleAscensionStage);
		}
		else
		{
			StartStageDesc.text = string.Format(LanguageManager.Ins.SetString("popup_ascension_start_stage_desc"), StageManager.Ins.PlayerStat.StartStage);
			double rewardAmount = ConstantData.GetAscensionMagicite(StageManager.Ins.PlayerData.Stage);
			double rewardAdd = rewardAmount * (StageManager.Ins.PlayerStat.AscensionReward / 100f);
			string addMagicite = rewardAdd > 0 ? "(+" + rewardAdd.ToCurrencyString() + ")" : "";
			RewardAmount.text = rewardAmount.ToCurrencyString() + addMagicite;
		}
	}

	void SetAscensionBtn(bool isPossibleAscension)
	{
		if (isPossibleAscension)
		{
			AscensionBtnText.text = LanguageManager.Ins.SetString("Ascension");

			AscensionBtn.onClick.RemoveAllListeners();
			AscensionBtn.onClick.AddListener(() =>
			{
				StageManager.Ins.StartAscension();
				CloseDialog();
			});

			AdAscensionBtnText.text = LanguageManager.Ins.SetString("btn_ad_ascension");
			AdAscensionBtnRewardText.text = LanguageManager.Ins.SetString("btn_ad_ascension_reward");

			AdAscensionBtn.onClick.RemoveAllListeners();
			AdAscensionBtn.onClick.AddListener(() =>
			{
				StageManager.Ins.StartAscension(true);
				CloseDialog();
			});
		}
		else
		{
			AdAscensionBtn.gameObject.SetActive(false);
			AscensionBtn.gameObject.SetActive(false);
		}
	}


}
