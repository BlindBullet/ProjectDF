using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogAscension : DialogController
{
	public static DialogAscension _Dialog = null;

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
		_Dialog = this;
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
		Show(true, true);
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
				SoundManager.Ins.PlaySFX("se_button_2");
				StageManager.Ins.StartAscension();
				CloseDialog(true);
			});

			AdAscensionBtnText.text = LanguageManager.Ins.SetString("btn_ad_ascension");
			AdAscensionBtnRewardText.text = LanguageManager.Ins.SetString("btn_ad_ascension_reward");

			AdAscensionBtn.onClick.RemoveAllListeners();
			AdAscensionBtn.onClick.AddListener(() =>
			{
				SoundManager.Ins.PlaySFX("se_button_2");

				if (!AdmobManager.Ins.isReal)
				{
					return;
				}

				StageManager.Ins.PlayerStat.CheckRemoveAd();

				if (StageManager.Ins.PlayerStat.RemoveAd)
				{
					StartCoroutine(GetAdReward());
				}
				else
				{
					if (AdmobManager.Ins.isAscensionRewardAdLoaded)
					{
						AdmobManager.Ins.ShowAscensionRewardAd();
					}
					else
					{
						AdmobManager.Ins.LoadAd(AdType.AscensionReward);

						if (AdmobManager.Ins.isAscensionRewardAdLoaded)
						{
							AdmobManager.Ins.ShowAscensionRewardAd();
						}
						else
						{
							UnityAdsManager.Ins.ShowAd(AdType.AscensionReward);
						}
					}
				}
			});
		}
		else
		{
			AdAscensionBtn.gameObject.SetActive(false);
			AscensionBtn.gameObject.SetActive(false);
		}
	}

	public IEnumerator GetAdReward()
	{
		yield return null;
		StageManager.Ins.StartAscension(true);
		CloseDialog(true);
	}

	private void OnDisable()
	{
		_Dialog = null;
	}

}
