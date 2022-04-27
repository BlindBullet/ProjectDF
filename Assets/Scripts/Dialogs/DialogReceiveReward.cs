using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogReceiveReward : DialogController
{
	public TextMeshProUGUI Title;
	public RewardIcon RewardIcon;
	public TextMeshProUGUI CloseText;

	public void OpenDialog(RewardType type, double value)
	{
		Title.text = LanguageManager.Ins.SetString("Reward");
		RewardIcon.SetIcon(type, value, RewardValueShowType.CalcValue);
		CloseText.text = LanguageManager.Ins.SetString("popup_receive_reward_close");
		CloseTextSequence();
		Show(true);
	}

	void CloseTextSequence()
	{
		CloseText.DOFade(0, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetId("CloseText");
	}

	private void OnDisable()
	{
		DOTween.Kill("CloseText");
	}

}
