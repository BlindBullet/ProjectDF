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
			case RewardType.Gold:
				Amount.text = chart.RewardValue + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.GameSpeed:
				Amount.text = chart.RewardValue + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.SoulStone:								
				Amount.text = chart.RewardValue.ToString();
				break;
		}
	}

	public void SetIcon(RewardType type, double value, RewardValueShowType valueShowType = RewardValueShowType.Time)
	{
		IconImg.sprite = Resources.Load<Sprite>("Sprites/Cost/" + type.ToString());

		switch (type)
		{
			case RewardType.Gold:
				if(valueShowType == RewardValueShowType.Time)
				{
					Amount.text = value + LanguageManager.Ins.SetString("Minute");
				}
				else if(valueShowType == RewardValueShowType.CalcValue)
				{
					Amount.text = value.ToCurrencyString();
				}
				break;
			case RewardType.GameSpeed:
				Amount.text = value + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.SoulStone:
				Amount.text = value.ToString();
				break;
		}
	}


}

public enum RewardValueShowType
{
	Time,
	CalcValue,
}
