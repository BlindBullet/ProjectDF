using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;

public class HeroIcon : MonoBehaviour
{
    public Button Btn;
    public Image IconBg;
    public Image IconImg;
    public Image IconFrame;
    public GameObject LockPanel;
    public GameObject LockIcon;
    public GameObject[] Stars;

    public void SetIcon(HeroData data, Action<HeroData> action = null)
    {
        List<HeroChart> chartList = CsvData.Ins.HeroChart[data.Id];
        HeroChart chart = null;

        for(int i = 0; i < chartList.Count; i++)
        {
            if (chartList[i].Grade == data.Grade)
                chart = chartList[i];
        }

        IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Heroes/Heroes").GetSprite(chart.Model);
        IconFrame.sprite = Resources.Load<Sprite>("Sprites/Heroes/Frames/" + chart.Attr.ToString());
        IconBg.sprite = Resources.Load<Sprite>("Sprites/Heroes/Bgs/" + chart.Attr.ToString());

        Btn.onClick.RemoveAllListeners();
        Btn.onClick.AddListener(() => { if (action != null) action(data); });

        SetStars(chart.Grade);
        SetLock(data.IsOwn);
    }

    void SetLock(bool isOpen)
    {
        if (!isOpen)
        {
            LockPanel.SetActive(true);
            LockIcon.SetActive(true);
        }
    }
    
    void SetStars(int grade)
    {
        for(int i = 0; i < grade; i++)
        {
            Stars[i].SetActive(true);
        }
    }

}
