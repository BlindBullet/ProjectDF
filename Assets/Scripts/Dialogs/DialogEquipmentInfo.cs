using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogEquipmentInfo : DialogController
{
	public TextMeshProUGUI NameText;	
	public EquipmentIcon EquipmentIcon;
	public TextMeshProUGUI EeLabelText;
	public TextMeshProUGUI EeDesc1;
	public TextMeshProUGUI EeDesc2;
	public TextMeshProUGUI CeLabelText;
	public TextMeshProUGUI CeDesc1;
	public TextMeshProUGUI CeDesc2;
	public TextMeshProUGUI FusionDescText;
	public Button EnchantBtn;
	public TextMeshProUGUI EnchantBtnText;
	public Image EnchantCostIconImg;
	public TextMeshProUGUI EnchantCostText;
	public Button FusionBtn;
	public TextMeshProUGUI FusionBtnText;
	public Button EquipBtn;
	public TextMeshProUGUI EquipBtnText;	
	public TextMeshProUGUI EnchantStoneText;
	EquipmentData data = null;
	EquipmentChart chart = null;

	public void OpenDialog(EquipmentData data)
	{
		this.data = data;
		chart = CsvData.Ins.EquipmentChart[data.Id];

		NameText.text = LanguageManager.Ins.SetString(chart.Name);
		EquipmentIcon.Setup(data);
		EeLabelText.text = LanguageManager.Ins.SetString("EquipEffect");
		CeLabelText.text = LanguageManager.Ins.SetString("CollectionEffect");
		EquipBtnText.text = LanguageManager.Ins.SetString("Equip");
		FusionBtnText.text = LanguageManager.Ins.SetString("Fusion");
		EnchantBtnText.text = LanguageManager.Ins.SetString("Enchant");
		FusionDescText.text = LanguageManager.Ins.SetString("FusionDesc");

		SetEquipEffect();
		SetCollectionEffect();

		SetFusionBtn();
		SetEquipBtn();
		SetEnchantBtn();

		Show(true);
	}

	void SetEquipEffect()
	{
		SEData seData = null;

		//for (int i = 0; i < SEManager.Ins.SeList.Count; i++)
		//{
		//	if (SEManager.Ins.SeList[i].Chart.Id == chart.EquipEffect)
		//	{
		//		seData = SEManager.Ins.SeList[i];
		//	}
		//}

		SEChart seChart = CsvData.Ins.SEChart[chart.EquipEffect];

		if (seData == null)
			seData = new SEData(seChart, data.EnchantLv);

		double value1 = seData.SetValue();
		double value2 = seData.NextSetValue();
		
		string _desc = "";

		switch (seChart.EParam2)
		{			
			default:
				_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p"), Math.Round(value1, 1), Math.Round(value2, 1));
				break;
		}
		
		EeDesc1.text = LanguageManager.Ins.SetString(chart.EquipEffectDesc);
		EeDesc2.text = _desc;
	}

	void SetCollectionEffect()
	{		
		SEData seData = null;

		//for (int i = 0; i < SEManager.Ins.SeList.Count; i++)
		//{
		//	if (SEManager.Ins.SeList[i].Chart.Id == chart.CollectionEffect)
		//	{
		//		seData = SEManager.Ins.SeList[i];
		//	}
		//}

		SEChart seChart = CsvData.Ins.SEChart[chart.CollectionEffect];

		if (seData == null)
			seData = new SEData(seChart, data.EnchantLv);

		double value1 = seData.SetValue();
		double value2 = seData.NextSetValue();

		string _desc = "";

		switch (seChart.EParam2)
		{
			default:
				_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p"), Math.Round(value1, 1), Math.Round(value2, 1));
				break;
		}

		CeDesc1.text = LanguageManager.Ins.SetString(chart.CollectionEffectDesc);
		CeDesc2.text = _desc;
	}


	void SetEquipBtn()
	{
		if (!data.isOpen)
		{
			EquipBtn.gameObject.SetActive(false);
			return;
		}

		if (data.isEquip)
		{
			EquipBtn.gameObject.SetActive(false);
		}
		else
		{
			EquipBtn.gameObject.SetActive(true);
			EquipBtn.onClick.RemoveAllListeners();
			EquipBtn.onClick.AddListener(() =>
			{
				StageManager.Ins.EquipmentData.Equip(data);
				EquipmentIcon.Setup(data);
				DialogEquipment._Dialog.SetEquipmentIcons(data.Type);
				SEManager.Ins.Apply();
				EquipBtn.gameObject.SetActive(false);
			});
		}
	}

	void SetFusionBtn()
	{
		if (!data.isOpen)
		{
			FusionBtn.gameObject.SetActive(false);
			return;
		}

		if(data.Count >= 5)
		{
			FusionBtn.gameObject.SetActive(true);
			FusionBtn.onClick.RemoveAllListeners();
			FusionBtn.onClick.AddListener(() => 
			{
				StageManager.Ins.EquipmentData.Fusion(data);
				EquipmentIcon.Setup(data);
				DialogEquipment._Dialog.SetEquipmentIcons(data.Type);
				SEManager.Ins.Apply();
				FusionBtn.gameObject.SetActive(false);
			});
		}
		else
		{
			FusionBtn.gameObject.SetActive(false);
		}
	}

	void SetEnchantBtn()
	{
		if (!data.isOpen)
		{
			EnchantBtn.gameObject.SetActive(false);
			return;
		}

		SetEnchantStoneText();

		EquipmentChart chart = CsvData.Ins.EquipmentChart[data.Id];
		double cost = ConstantData.CalcValue(chart.EnchantCost, chart.CostIncRate, data.EnchantLv);		
		EnchantCostText.text = cost.ToCurrencyString();

		EnchantBtn.onClick.RemoveAllListeners();

		if(StageManager.Ins.EquipmentData.EnchantStone >= cost)
		{
			EnchantBtn.onClick.AddListener(() => 
			{
				StageManager.Ins.EquipmentData.GetEnchantStone(-cost);
				StageManager.Ins.EquipmentData.EnchantLvUp(data);

				EquipmentIcon.Setup(data);
				SetEquipEffect();
				SetCollectionEffect();
				DialogEquipment._Dialog.SetEquipmentIcons(data.Type);
				SEManager.Ins.Apply();
				SetEnchantBtn();
			});
		}
		else
		{
			EnchantBtn.onClick.AddListener(() => 
			{
				DialogManager.Ins.OpenCautionBar("not_enough_enchantstone");
			});
		}


	}

	void SetEnchantStoneText()
	{
		EnchantStoneText.text = StageManager.Ins.EquipmentData.EnchantStone.ToCurrencyString();
	}
}
