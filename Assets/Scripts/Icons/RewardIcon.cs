using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardIcon : MonoBehaviour
{
	public Image FrameImg;
	public Image IconImg;
	public TextMeshProUGUI Amount;

	public void SetIcon(QuestChart chart)
	{
		IconImg.sprite = Resources.Load<Sprite>("Sprites/Cost/" + chart.RewardType.ToString());

		switch (chart.RewardType)
		{
			case QuestReward.Gold:
				Amount.text = chart.RewardValue + LanguageManager.Ins.SetString("Minute");
				break;
			case QuestReward.GameSpeed:
				Amount.text = chart.RewardValue + LanguageManager.Ins.SetString("Minute");
				break;
			case QuestReward.SoulStone:								
				Amount.text = chart.RewardValue.ToString();
				break;
		}
	}

}
