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

    public void Setup()
    {
        StageManager.Ins.GoldChanged += SetGoldText;
        StageManager.Ins.SoulStoneChanged += SetSoulStoneText;
        StageManager.Ins.MagiciteChanged += SetMagiciteText;

        SetGoldText(0);
        SetSoulStoneText(0);
        SetMagiciteText(0);
        //GoldText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Gold);
    }

    public void SetStageText(int stageNo)
    {
        StageText.text = LanguageManager.Ins.SetString("Stage") + "\n" + stageNo.ToString();
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

    
}
