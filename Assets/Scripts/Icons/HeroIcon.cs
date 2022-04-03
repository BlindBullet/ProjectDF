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

    public void SetIcon(HeroData data, Action<HeroData> action = null)
    {
        HeroChart chart = CsvData.Ins.HeroChart[data.Id];

        IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Heroes/Heroes").GetSprite(chart.Model);
        IconFrame.sprite = Resources.Load<Sprite>("Sprites/Heroes/Frames/" + chart.Tier.ToString());
        IconBg.sprite = Resources.Load<Sprite>("Sprites/Heroes/Bgs/" + chart.Tier.ToString());

        Btn.onClick.RemoveAllListeners();
        Btn.onClick.AddListener(() => { if (action != null) action(data); });

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
    
}
