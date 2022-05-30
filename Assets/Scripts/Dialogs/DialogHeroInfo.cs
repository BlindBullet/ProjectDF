using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.U2D;

public class DialogHeroInfo : DialogController
{
	public TextMeshProUGUI HeroName;
	public HeroIcon HeroIcon;    
	public TextMeshProUGUI AtkTitle;
	public TextMeshProUGUI Atk;
	public TextMeshProUGUI SpdTitle;
	public TextMeshProUGUI Spd;
	public TextMeshProUGUI AttrTitle;
	public Image AttrIcon;
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
	HeroChart chart;
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

		List<HeroChart> chartList = CsvData.Ins.HeroChart[data.Id];
		chart = null;

		for (int i = 0; i < chartList.Count; i++)
		{
			if (chartList[i].Grade == data.Grade)
				chart = chartList[i];
		}

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
		Spd.text = chart.Spd.ToString();
		AttrTitle.text = LanguageManager.Ins.SetString("Attr");
		AttrIcon.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.Attr.ToString());
	}

	void SetSkill(string id)
	{
		List<SkillChart> skills = CsvData.Ins.SkillChart[id];
		SkillChart skill = null;

		for(int i = 0; i < skills.Count; i++)
		{
			if (skills[i].Grade == data.Grade)
				skill = skills[i];
		}

		SkillName.text = LanguageManager.Ins.SetString("Skill");
		SkillDesc.text = string.Format(LanguageManager.Ins.SetString(skill.Desc), skill.CoolTime.ToString());
	}

	void SetCE(string id, HeroChart chart)
	{
		SEData seData = null;

		for(int i = 0; i < SEManager.Ins.SeList.Count; i++)
		{
			if(SEManager.Ins.SeList[i].Chart.Id == id)
			{
				seData = SEManager.Ins.SeList[i];
			}
		}

		if (seData == null)
		{
			seData = new SEData(CsvData.Ins.SEChart[id], chart.Grade);
			seData.SetValue(double.Parse(seData.Chart.EParam5));
		}
			
		
		CEName.text = LanguageManager.Ins.SetString("CollectionEffect");
		CEDesc.text = string.Format(LanguageManager.Ins.SetString(chart.CEDesc), Math.Round(seData.Value, 1).ToCurrencyString());		
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
			
			PurchaseBtn.gameObject.SetActive(true);
			PurchaseBtnText.text = LanguageManager.Ins.SetString("Enchant");
			double enchantCost = ConstantData.GetHeroEnchantCost(data.EnchantLv);
			PurchaseCost.text = enchantCost.ToCurrencyString();
			PurchaseCostIcon.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Magicite");
			PurchaseBtn.onClick.RemoveAllListeners();

			if(StageManager.Ins.PlayerData.Magicite >= enchantCost)
			{
				purchaseBtnMat.SetFloat("_GreyscaleBlend", 0f);				
				PurchaseBtn.onClick.AddListener(() => 
				{
					if (StageManager.Ins.PlayerData.EnchantHero(data))
					{
						StageManager.Ins.ChangeMagicite(-enchantCost);
						HeroIcon.Setup(data);
						DialogHero._DialogHero.SetHeroes();
						DialogHero._DialogHero.SetDeploySlots();
						StageManager.Ins.Slots[data.SlotNo - 1].SetEnchantLabel(data);

						for(int i = 0; i < HeroBase.Heroes.Count; i++)
						{
							if (HeroBase.Heroes[i].Data == data)
								HeroBase.Heroes[i].Stat.ChangeEnchantLv(data.EnchantLv);
						}

						SetHeroInfo(data);
					}
				});
			}
			else
			{
				purchaseBtnMat.SetFloat("_GreyscaleBlend", 1f);				
			}

			if(data.Grade >= 5)
			{
				UpgradeBtn.gameObject.SetActive(false);
			}
			else
			{
				UpgradeBtn.gameObject.SetActive(true);
				UpgradeBtnText.text = LanguageManager.Ins.SetString("Upgrade");

				double cost = ConstantData.GetHeroUpgradeCost(data.Grade);

				UpgradeCost.text = cost.ToCurrencyString();

				if (StageManager.Ins.PlayerData.SoulStone >= cost)
				{
					upgradeBtnMat.SetFloat("_GreyscaleBlend", 0f);					
					UpgradeBtn.onClick.RemoveAllListeners();
					UpgradeBtn.onClick.AddListener(() =>
					{
						if (StageManager.Ins.PlayerData.UpgradeHero(data))
						{
							SEManager.Ins.Apply();
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
					upgradeBtnMat.SetFloat("_GreyscaleBlend", 1f);
				}
			}
		}
		else
		{
			PurchaseBtn.gameObject.SetActive(true);
			PurchaseBtnText.text = LanguageManager.Ins.SetString("Summon");
			PurchaseCostIcon.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.CostType.ToString());
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
				purchaseBtnMat.SetFloat("_GreyscaleBlend", 0f);				
				PurchaseBtn.onClick.RemoveAllListeners();
				PurchaseBtn.onClick.AddListener(() =>
				{
					if (StageManager.Ins.PlayerData.SummonHero(data, chart))
					{
						SEManager.Ins.Apply();
						HeroIcon.Setup(data);
						SetButtons(data, chart);
						DialogHero._DialogHero.SetHeroes();
					}
				});
			}
			else
			{
				purchaseBtnMat.SetFloat("_GreyscaleBlend", 1f);				
			}
		}
	}

	void SetDeploy()
	{
		DialogHero._DialogHero.SetDeployState(data);
		CloseDialog();
	}


}
