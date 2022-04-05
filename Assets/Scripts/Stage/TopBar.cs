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
    public TextMeshProUGUI GemText;
    public TextMeshProUGUI StageText;

    private void Start()
    {
        StageManager.Ins.GoldChanged += SetGoldText;
        StageManager.Ins.GemChanged += SetGemText;
    }

    public void Init()
    {
        SetGoldText(0);
        SetGemText(0);
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

    public void SetGemText(double value)
    {
        GemText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Gem);
    }

    
}
