using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using AllIn1SpriteShader;
using System;

public class HeroUi : MonoBehaviour
{
    public Button IconBtn;
    public Image IconFrame;
    public Image IconImg;
    public Image IconBg;
    public TextMeshProUGUI LvText;
    public Image SkillCoolTimeFrame;
    public GameObject[] Stars;    
    HeroBase me;

    public void SetUp(HeroData data)
    {
        me = GetComponent<HeroBase>();        
        HeroChart chart = CsvData.Ins.HeroChart[data.Id][data.Grade - 1];        
        
        SetIcon(chart);
        SetStars(chart.Grade);        
        SetIconBtn();
    }

    void SetIcon(HeroChart chart)
    {
        IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Heroes/Heroes").GetSprite(chart.Model);
        IconFrame.sprite = Resources.Load<Sprite>("Sprites/Heroes/Frames/" + chart.Attr.ToString());
        IconBg.sprite = Resources.Load<Sprite>("Sprites/Heroes/Bgs/" + chart.Attr.ToString());
    }

    public void SetLvText(int lv)
    {
        LvText.text = lv.ToString();
    }

    public void SetCoolTimeFrame(float value)
    {
        SkillCoolTimeFrame.fillAmount = value;
    }
        
    void SetIconBtn()
    {
        IconBtn.onClick.RemoveAllListeners();
        IconBtn.onClick.AddListener(() => me.SkillCon.UseSkill());
    }

    void SetStars(int grade)
    {
        for (int i = 0; i < grade; i++)
        {
            Stars[i].SetActive(true);
        }
    }
}
