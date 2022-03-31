using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TopBar : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI GemText;
    public TextMeshProUGUI StageText;

    private void Start()
    {
        StageManager.Ins.GoldChanged += SetGoldText;
        StageManager.Ins.GemChanged += SetGemText;
    }

    public void Init()
    {
        GoldText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Gold);
    }

    public void SetStageText(int stageNo)
    {
        StageText.text = LanguageManager.Ins.SetString("Stage") + " " + stageNo.ToString();
    }

    public void SetGoldText(double value)
    {
        GoldText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Gold);
    }

    public void SetGemText(double value)
    {
        GemText.text = ExtensionMethods.ToCurrencyString(StageManager.Ins.PlayerData.Gem);
    }

    
}
