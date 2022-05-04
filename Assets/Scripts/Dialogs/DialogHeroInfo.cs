using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogHeroInfo : DialogController
{
	public TextMeshProUGUI HeroName;
	public HeroIcon HeroIcon;    
	public TextMeshProUGUI AtkTitle;
	public TextMeshProUGUI Atk;
	public TextMeshProUGUI SpdTitle;
	public TextMeshProUGUI Spd;	
	public TextMeshProUGUI SkillName;
	public TextMeshProUGUI SkillDesc;
	public TextMeshProUGUI CEName;
	public TextMeshProUGUI CEDesc;
	public TextMeshProUGUI PurchaseBtnText;
	public TextMeshProUGUI PurchaseCost;
	public Image PurchaseCostIcon;
	public Button PurchaseBtn;
	public TextMeshProUGUI UpgradeBtnText;
	public TextMeshProUGUI UpgradeCost;
	public Button UpgradeBtn;
	public TextMeshProUGUI DeployBtnText;    
	public Button DeployBtn;
	HeroData data;
	Material purchaseBtnMat;
	Material upgradeBtnMat;

	public void OpenDialog(HeroData data)
	{
		//Image uiImage = PurchaseBtn.GetComponent<Image>();
		//uiImage.material = new Material(uiImage.material);
		//Image uiImage2 = UpgradeBtn.GetComponent<Image>();
		//uiImage2.material = new Material(uiImage.material);

		purchaseBtnMat = PurchaseBtn.GetComponent<Image>().material;
		PurchaseBtn.GetComponent<AllIn1SpriteShader.AllIn1Shader>().ApplyMaterialToHierarchy();
		upgradeBtnMat = PurchaseBtn.GetComponent<Image>().material;
		UpgradeBtn.GetComponent<AllIn1SpriteShader.AllIn1Shader>().ApplyMaterialToHierarchy();

		this.data = data;		
		HeroIcon.Setup(data);
		SetHeroInfo(data);
		Show(true);
	}

	void SetHero(HeroChart chart)
	{
		HeroName.text = LanguageManager.Ins.SetString(chart.StrName);
	}

	void SetHeroInfo(HeroData data)
	{
		List<HeroChart> chartList = CsvData.Ins.HeroChart[data.Id];
		HeroChart chart = null;

		for (int i = 0; i < chartList.Count; i++)
		{
			if (chartList[i].Grade == data.Grade)
				chart = chartList[i];
		}

		SetHero(chart);
		SetStat(chart);
		SetSkill(chart.Skill);
		SetCE(chart.CollectionEffect, chart);
		SetButtons(data, chart);
	}

	void SetStat(HeroChart chart)
	{
		AtkTitle.text = LanguageManager.Ins.SetString("Atk");
		Atk.text = ExtensionMethods.ToCurrencyString(chart.Atk);
		SpdTitle.text = LanguageManager.Ins.SetString("Spd");
		Spd.text = ExtensionMethods.ToCurrencyString(chart.Spd);		
	}

	void SetSkill(string id)
	{
		SkillChart skill = CsvData.Ins.SkillChart[id];
		SkillName.text = LanguageManager.Ins.SetString(skill.Name);
		SkillDesc.text = LanguageManager.Ins.SetString(skill.Desc);
	}

	void SetCE(string id, HeroChart chart)
	{
		SEChart se = CsvData.Ins.SEChart[id][chart.Grade];
		CEName.text = LanguageManager.Ins.SetString("CollectionEffect");
		
		switch (se.EffectType)
		{
			case SEEffectType.StatChange:
				CEDesc.text = string.Format(LanguageManager.Ins.SetString(chart.CEDesc), se.EParam3);
				break;
			case SEEffectType.AddCurrency:
				CEDesc.text = string.Format(LanguageManager.Ins.SetString(chart.CEDesc), se.EParam3);
				break;
			case SEEffectType.StartStage:
				CEDesc.text = string.Format(LanguageManager.Ins.SetString(chart.CEDesc), se.EParam2);
				break;
		}
		
	}

	void SetButtons(HeroData data, HeroChart chart)
	{		
		if (data.IsOwn)
		{
			if (data.SlotNo < 0)
			{
				DeployBtn.gameObject.SetActive(true);
				DeployBtnText.text = LanguageManager.Ins.SetString("Deploy");

				DeployBtn.onClick.RemoveAllListeners();
				DeployBtn.onClick.AddListener(() => { SetDeploy(); });
			}
			
			PurchaseBtn.gameObject.SetActive(false);
			UpgradeBtn.gameObject.SetActive(true);
			UpgradeBtnText.text = LanguageManager.Ins.SetString("Upgrade");

			double cost = ConstantData.GetHeroUpgradeCost(data.Grade);

			UpgradeCost.text = cost.ToCurrencyString();

			if (StageManager.Ins.PlayerData.SoulStone >= cost)
			{
				upgradeBtnMat.DisableKeyword("GREYSCALE_ON");
				UpgradeBtn.onClick.RemoveAllListeners();
				UpgradeBtn.onClick.AddListener(() => 
				{
					if (StageManager.Ins.PlayerData.UpgradeHero(data))
					{
						StageManager.Ins.ChangeSoulStone(-cost);
						HeroIcon.Setup(data);						
						DialogHero._DialogHero.SetHeroes();
						DialogHero._DialogHero.SetDeploySlots();
						StageManager.Ins.DeployHero(data, data.SlotNo);
						SetHeroInfo(data);						
					}
				});
			}
			else
			{
				upgradeBtnMat.EnableKeyword("GREYSCALE_ON");
			}
		}
		else
		{
			PurchaseBtn.gameObject.SetActive(true);
			PurchaseBtnText.text = LanguageManager.Ins.SetString("Summon");
			PurchaseCostIcon.sprite = Resources.Load<Sprite>("Sprites/Cost/" + chart.CostType.ToString());
			PurchaseCost.text = chart.Cost.ToCurrencyString();

			bool haveCost = false;

			switch (chart.CostType)
			{				
				case CostType.Gold:
					if (StageManager.Ins.PlayerData.Gold >= chart.Cost)
						haveCost = true;
					break;
				case CostType.SoulStone:
					if (StageManager.Ins.PlayerData.SoulStone >= chart.Cost)
						haveCost = true;
					break;
				case CostType.Magicite:
					if (StageManager.Ins.PlayerData.Magicite >= chart.Cost)
						haveCost = true;
					break;
			}

			if (haveCost)
			{
				purchaseBtnMat.DisableKeyword("GREYSCALE_ON");
				PurchaseBtn.onClick.RemoveAllListeners();
				PurchaseBtn.onClick.AddListener(() =>
				{
					if (StageManager.Ins.PlayerData.SummonHero(data, chart))
					{						
						HeroIcon.Setup(data);
						SetButtons(data, chart);
						DialogHero._DialogHero.SetHeroes();
					}
				});
			}
			else
			{
				purchaseBtnMat.EnableKeyword("GREYSCALE_ON");
			}
		}
	}

	void SetDeploy()
	{
		DialogHero._DialogHero.SetDeployState(data);
		CloseDialog();
	}


}
