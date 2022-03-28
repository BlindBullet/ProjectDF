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
    HeroData data;
    Material lvUpBtnMat;
    HeroBase me;

    private void Start()
    {
        StageManager.Ins.GoldChanged += SetLvUpBtn;

        Image uiImage = LvUpBtn.GetComponent<Image>();
        uiImage.material = new Material(uiImage.material);

        lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;        
    }

    public void SetUp(HeroData data)
    {
        me = GetComponent<HeroBase>();
        this.data = data;
        HeroChart chart = CsvData.Ins.HeroChart[data.Id];

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
        SetLvUpBtn(0);
        SetIconBtn();
    }

    void SetIcon(HeroChart chart)
    {
        IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Heroes").GetSprite(chart.Model);        
    }

    public void SetCoolTimeFrame(float value)
    {
        SkillCoolTimeFrame.fillAmount = value;
    }

    public void LvUp(HeroData data)
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

    private void OnDisable()
    {
        StageManager.Ins.GoldChanged -= SetLvUpBtn;
    }
}
