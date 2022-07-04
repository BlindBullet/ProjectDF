using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using System;

public class RewardIcon : MonoBehaviour
{
	public Image FrameImg;
	public Image IconImg;
	public TextMeshProUGUI Amount;
	public GameObject CheckObj;
	public Image SelectedFrame;
	public Button Btn;

	public void SetIcon(QuestChart chart)
	{
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.RewardType.ToString());

		float value = chart.RewardValue + (chart.RewardValue * (StageManager.Ins.PlayerStat.QuestReward / 100f));

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				Amount.text = Mathf.RoundToInt(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.GameSpeed:
				Amount.text = Mathf.RoundToInt(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.UseAutoSkill:
				Amount.text = Mathf.RoundToInt(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.GainGold:
				Amount.text = Mathf.RoundToInt(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.SoulStone:								
				Amount.text = Mathf.RoundToInt(value).ToString();
				break;
		}
	}

	public void SetIcon(RewardType type, double value, RewardValueShowType valueShowType = RewardValueShowType.Time)
	{
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(type.ToString());

		switch (type)
		{
			case RewardType.Gold:
				if(valueShowType == RewardValueShowType.Time)
				{
					Amount.text = value + LanguageManager.Ins.SetString("Minute");
				}
				else if(valueShowType == RewardValueShowType.CalcValue)
				{
					if(value <= 0)
						Amount.text = "";
					else					
						Amount.text = value.ToCurrencyString();
				}
				break;
			case RewardType.GameSpeed:
				if (value <= 0)
					Amount.text = "";
				else
					Amount.text = value + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.GainGold:
				if (value <= 0)
					Amount.text = "";
				else
					Amount.text = value + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.UseAutoSkill:
				if (value <= 0)
					Amount.text = "";
				else
					Amount.text = value + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.SoulStone:
				if (value <= 0)
					Amount.text = "";
				else
					Amount.text = value.ToString();
				break;
		}
	}

	public void SetIcon(AttendanceChart chart, int lv)
	{
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.RewardType.ToString());
		float incP = 0.1f * (lv - 1) > 3f ? 3f : 0.1f * (lv - 1);
		double value = chart.Amount + (chart.Amount * incP);

		switch (chart.RewardType)
		{
			case RewardType.Gold:
				Amount.text = Math.Round(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.GameSpeed:
				Amount.text = Math.Round(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.UseAutoSkill:
				Amount.text = Math.Round(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.GainGold:
				Amount.text = Math.Round(value) + LanguageManager.Ins.SetString("Minute");
				break;
			case RewardType.SoulStone:
				Amount.text = Math.Round(value).ToCurrencyString();
				break;
		}
	}

	public void CheckOn()
	{
		CheckObj.SetActive(true);
	}

	public void CheckOff()
	{
		CheckObj.SetActive(false);
	}

	public void SelectedFrameOn()
	{
		SelectedFrame.gameObject.SetActive(true);
	}

	public void SelectedFrameOff()
	{
		SelectedFrame.gameObject.SetActive(false);
	}


}

public enum RewardValueShowType
{
	Time,
	CalcValue,
}
