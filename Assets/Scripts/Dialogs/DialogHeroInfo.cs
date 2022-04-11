using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogHeroInfo : DialogController
{
	public TextMeshProUGUI HeroName;
	public HeroIcon HeroIcon;    
	public TextMeshProUGUI AtkTitle;
	public TextMeshProUGUI Atk;
	public TextMeshProUGUI SpdTitle;
	public TextMeshProUGUI Spd;
	public TextMeshProUGUI RangeTitle;
	public TextMeshProUGUI Range;
	public TextMeshProUGUI SkillName;
	public TextMeshProUGUI SkillDesc;
	public TextMeshProUGUI CEName;
	public TextMeshProUGUI CEDesc;
	public TextMeshProUGUI PurchaseBtnText;
	public TextMeshProUGUI PurchaseCost;
	public Button PurchaseBtn;
	public TextMeshProUGUI UpgradeBtnText;
	public TextMeshProUGUI UpgradeCost;
	public Button UpgradeBtn;
	public TextMeshProUGUI DeployBtnText;    
	public Button DeployBtn;
	
	public void OpenDialog(HeroData data)
	{
		List<HeroChart> chartList = CsvData.Ins.HeroChart[data.Id];
		HeroChart chart = null;

		for(int i = 0; i < chartList.Count; i++)
		{
			if (chartList[i].Grade == data.Grade)
				chart = chartList[i];
		}

		SetHero(chart);
		HeroIcon.Setup(data);
		SetStat(chart);
		SetSkill(chart.Skill);
		SetCE(chart.CollectionEffect);
		SetButtons(data, chart);
		Show(true);
	}

	void SetHero(HeroChart chart)
	{
		HeroName.text = LanguageManager.Ins.SetString(chart.StrName);
	}

	void SetStat(HeroChart chart)
	{
		AtkTitle.text = LanguageManager.Ins.SetString("Atk");
		Atk.text = ExtensionMethods.ToCurrencyString(chart.Atk);
		SpdTitle.text = LanguageManager.Ins.SetString("Spd");
		Spd.text = ExtensionMethods.ToCurrencyString(chart.Spd);
		RangeTitle.text = LanguageManager.Ins.SetString("Range");
		Range.text = ExtensionMethods.ToCurrencyString(chart.Range);
	}

	void SetSkill(string id)
	{
		SkillChart skill = CsvData.Ins.SkillChart[id];
		SkillName.text = LanguageManager.Ins.SetString(skill.Name);
		SkillDesc.text = LanguageManager.Ins.SetString(skill.Desc);
	}

	void SetCE(string id)
	{
		SEChart se = CsvData.Ins.SEChart[id];
		CEName.text = LanguageManager.Ins.SetString("CollectionEffect");
		CEDesc.text = LanguageManager.Ins.SetString(se.Desc);
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
				DeployBtn.onClick.AddListener(() => { });
			}
			
			PurchaseBtn.gameObject.SetActive(false);
			UpgradeBtn.gameObject.SetActive(true);
			UpgradeBtnText.text = LanguageManager.Ins.SetString("Upgrade");
			//UpgradeCost.text = ExtensionMethods.ToCurrencyString()

			UpgradeBtn.onClick.RemoveAllListeners();
			UpgradeBtn.onClick.AddListener(() => { });
		}
		else
		{
			PurchaseBtn.gameObject.SetActive(true);
			PurchaseBtnText.text = LanguageManager.Ins.SetString("Summon");
			PurchaseCost.text = ThousandCommaText.GetThousandComma((int)chart.Cost);

			PurchaseBtn.onClick.RemoveAllListeners();
			PurchaseBtn.onClick.AddListener(() =>
			{
				if (StageManager.Ins.PlayerData.SummonHero(data, chart.Cost))
				{
					StageManager.Ins.ChangeGem(-chart.Cost);
					SetButtons(data, chart);
					DialogHero._DialogHero.SetHeroes();
				}
				else
				{
					Debug.Log("보석이 모자랍니다.");
				}
			});
		}
	}

}
