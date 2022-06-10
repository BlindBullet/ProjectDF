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
	public TextMeshProUGUI CEDesc2;
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
		Show(false);
	}

	void SetHero(HeroChart chart)
	{
		HeroName.text = LanguageManager.Ins.SetString(chart.StrName);
	}

	void SetHeroInfo(HeroData data)
	{
		List<HeroChart> chartList = CsvData.Ins.HeroChart[data.Id];
		chart = null;

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
		Atk.text = ConstantData.GetHeroAtkEnchant(chart.Atk, data.EnchantLv).ToCurrencyString();
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

		SEChart seChart = CsvData.Ins.SEChart[id];
		
		if (seData == null)		
			seData = new SEData(seChart, chart.Grade);

		double value1 = seData.SetValue();
		double value2 = seData.NextSetValue();

		string _desc = "";

		if(data.Grade == 5)
		{
			switch (seChart.EParam2)
			{
				case "Gold":
					if (seChart.EffectType == SEEffectType.StatChange)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_max_hero"), value1.ToCurrencyString(), data.Grade);
					else if (seChart.EffectType == SEEffectType.Ascension)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_max_hero"), value1.ToCurrencyString(), data.Grade);
					break;
				case "Time":
					if (seChart.EffectType == SEEffectType.OfflineReward)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_t_max_hero"), value1, data.Grade);
					else
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_max_hero"), Math.Round(value1, 1), data.Grade);
					break;
				default:
					_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_max_hero"), Math.Round(value1, 1), data.Grade);
					break;
			}
		}
		else
		{
			switch (seChart.EParam2)
			{
				case "Gold":
					if (seChart.EffectType == SEEffectType.StatChange)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_hero"), value1.ToCurrencyString(), value2.ToCurrencyString(), data.Grade, data.Grade + 1);
					else if (seChart.EffectType == SEEffectType.Ascension)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_hero"), value1.ToCurrencyString(), value2.ToCurrencyString(), data.Grade, data.Grade + 1);
					break;
				case "Time":
					if (seChart.EffectType == SEEffectType.OfflineReward)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_t_hero"), value1, value2, data.Grade, data.Grade + 1);
					else
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_hero"), Math.Round(value1, 1), Math.Round(value2, 1), data.Grade, data.Grade + 1);
					break;
				default:
					_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_hero"), Math.Round(value1, 1), Math.Round(value2, 1), data.Grade, data.Grade + 1);
					break;
			}
		}

		CEName.text = LanguageManager.Ins.SetString("CollectionEffect");
		CEDesc.text = LanguageManager.Ins.SetString(chart.CEDesc);
		CEDesc2.text = _desc;
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
				DeployBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); SetDeploy(); });
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
					SoundManager.Ins.PlaySFX("se_button_2");

					if (StageManager.Ins.PlayerData.EnchantHero(data))
					{
						StageManager.Ins.ChangeMagicite(-enchantCost);
						HeroIcon.Setup(data);
						DialogHero._DialogHero.SetHeroes();
						DialogHero._DialogHero.SetDeploySlots();

						if (data.SlotNo > 0)
						{
							StageManager.Ins.Slots[data.SlotNo - 1].SetEnchantLabel(data);

							for (int i = 0; i < HeroBase.Heroes.Count; i++)
							{
								if (HeroBase.Heroes[i].Data == data)
									HeroBase.Heroes[i].Stat.ChangeEnchantLv(data.EnchantLv);
							}
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
						SoundManager.Ins.PlaySFX("se_button_2");

						if (StageManager.Ins.PlayerData.UpgradeHero(data))
						{
							SEManager.Ins.Apply();
							StageManager.Ins.ChangeSoulStone(-cost);
							HeroIcon.Setup(data);
							DialogHero._DialogHero.SetHeroes();
							DialogHero._DialogHero.SetDeploySlots();

							if(data.SlotNo > 0)
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
					SoundManager.Ins.PlaySFX("se_button_2");

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
