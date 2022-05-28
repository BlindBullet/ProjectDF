using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TopBar : MonoBehaviour
{
	public TextMeshProUGUI GoldText;
	public TextMeshProUGUI MagiciteText;
	public TextMeshProUGUI SoulStoneText;
	public TextMeshProUGUI StageText;
	public GameObject GameSpeedBuffObj;
	public TextMeshProUGUI GameSpeedTimeText;
	public void Setup()
	{
		StageManager.Ins.GoldChanged += SetGoldText;
		StageManager.Ins.SoulStoneChanged += SetSoulStoneText;
		StageManager.Ins.MagiciteChanged += SetMagiciteText;
		PlayerBuffManager.Ins.GameSpeedBuffAdded += SetGameSpeed;

		SetGoldText(0);
		SetSoulStoneText(0);
		SetMagiciteText(0);
		//GoldText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Gold);
	}

	public void SetStageText(int stageNo)
	{
		StageText.text = LanguageManager.Ins.SetString("Stage") + " " + stageNo.ToString();
	}

	public void SetGoldText(double value)
	{
		GoldText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Gold);
	}

	public void SetMagiciteText(double value)
	{
		MagiciteText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Magicite);
	}

	public void SetSoulStoneText(double value)
	{
		SoulStoneText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.SoulStone);
	}

	public void SetGameSpeed(double leftTime)
	{		
		if(leftTime > 0f)
		{
			int hour = (int)(leftTime / 3600f);
			int min = (int)((leftTime % 3600f) / 60f);			
						
			string hourStr = hour < 10 ? "0" + hour : hour.ToString();
			string minStr = "";  

			if(hour <= 0)
			{
				minStr = (min + 1).ToString();
				GameSpeedTimeText.text = minStr + "M";
			}
			else
			{
				minStr = min < 10 ? "0" + (min + 1) : (min + 1).ToString();
				GameSpeedTimeText.text = hourStr + "H " + minStr + "M";
			}
			

			GameSpeedBuffObj.SetActive(true);
		}
		else
		{
			GameSpeedBuffObj.SetActive(false);
		}
	}

}
