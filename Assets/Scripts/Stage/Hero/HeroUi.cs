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
    public Button LvUpBtn;
    public TextMeshProUGUI LvUpBtnText;
    public TextMeshProUGUI LvUpCostText;
    public Image SkillCoolTimeFrame;
    public GameObject[] Stars;
    Hero data;
    Material lvUpBtnMat;
    HeroBase me;

    private void Start()
    {
        StageManager.Ins.GoldChanged += SetLvUpBtn;

        Image uiImage = LvUpBtn.GetComponent<Image>();
        uiImage.material = new Material(uiImage.material);

        lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;        
    }

    public void SetUp(Hero data)
    {
        me = GetComponent<HeroBase>();
        this.data = data;
        HeroChart chart = CsvData.Ins.HeroChart[data.Id][data.Grade - 1];        
        LvUpBtnText.text = LanguageManager.Ins.SetString("level_up");
        
        LvUpBtn.onClick.RemoveAllListeners();
        LvUpBtn.onClick.AddListener(() => 
        {
            if (data.LevelUp())
            {
                LvUp(data);
            }
        });

        SetLvText(data.Lv);
        SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
        SetIcon(chart);
        SetStars(chart.Grade);
        SetLvUpBtn(0);
        SetIconBtn();
    }

    void SetIcon(HeroChart chart)
    {
        IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Heroes/Heroes").GetSprite(chart.Model);
        IconFrame.sprite = Resources.Load<Sprite>("Sprites/Heroes/Frames/" + chart.Attr.ToString());
        IconBg.sprite = Resources.Load<Sprite>("Sprites/Heroes/Bgs/" + chart.Attr.ToString());
    }

    public void SetCoolTimeFrame(float value)
    {
        SkillCoolTimeFrame.fillAmount = value;
    }

    public void LvUp(Hero data)
    {
        SetLvText(data.Lv);
        SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
    }

    void SetLvText(int lv)
    {
        LvText.text = lv.ToString();
    }

    void SetLvUpCost(double cost)
    {
        LvUpCostText.text = ExtensionMethods.ToCurrencyString(cost);
    }

    void SetLvUpBtn(double value)
    {
        if(StageManager.Ins.PlayerData.Gold >= ConstantData.GetLvUpCost(data.Lv))
        {
            LvUpBtnEnable();
        }
        else
        {
            LvUpBtnDisable();
        }
    }

    void SetIconBtn()
    {
        IconBtn.onClick.RemoveAllListeners();
        IconBtn.onClick.AddListener(() => me.SkillCon.UseSkill());
    }

    public void LvUpBtnEnable()
    {
        LvUpBtn.enabled = true;
        lvUpBtnMat.DisableKeyword("GREYSCALE_ON");        
    }

    public void LvUpBtnDisable()
    {
        LvUpBtn.enabled = false;
        lvUpBtnMat.EnableKeyword("GREYSCALE_ON");        
    }

    void SetStars(int grade)
    {
        for (int i = 0; i < grade; i++)
        {
            Stars[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        StageManager.Ins.GoldChanged -= SetLvUpBtn;
    }
}
