using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogHero : DialogController
{
    public TextMeshProUGUI TitleText;
    public Transform HeroIconsTrf;    

    public void OpenDialog()
    {
        TitleText.text = LanguageManager.Ins.SetString("title_dialog_hero");
        SetHeroes();        
        Show(true);
    }

    void SetHeroes()
    {
        List<HeroData> heroes = StageManager.Ins.PlayerData.Heroes;
        for (int i = 0; i < heroes.Count; i++)
        {
            GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/HeroIcon") as GameObject, HeroIconsTrf);
            obj.GetComponent<HeroIcon>().SetIcon(heroes[i], OpenHeroInfo);
        }
    }

    void OpenHeroInfo(HeroData data)
    {
        Debug.Log(data.Id + " 정보 팝업 열기");
    }
}
