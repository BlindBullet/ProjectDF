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
	public Button EnchantBtn;
	public Image EnchantCostIconImg;
	public TextMeshProUGUI EnchantCostText;
	public Button FusionBtn;
	public TextMeshProUGUI FusionBtnText;
	public Button EquipBtn;
	public TextMeshProUGUI EquipBtnText;

	public void OpenDialog(EquipmentData data)
	{
		EquipmentChart chart = CsvData.Ins.EquipmentChart[data.Id];

		NameText.text = LanguageManager.Ins.SetString(chart.Name);
		EquipmentIcon.Setup(data);
		EeLabelText.text = LanguageManager.Ins.SetString("EquipEffect");
		CeLabelText.text = LanguageManager.Ins.SetString("CollectionEffect");

		SetEquipEffect(chart.EquipEffect, data, chart);
		SetCollectionEffect(chart.CollectionEffect, data, chart);

		Show(true);
	}

	void SetEquipEffect(string id, EquipmentData data, EquipmentChart chart)
	{
		SEData seData = null;

		for (int i = 0; i < SEManager.Ins.SeList.Count; i++)
		{
			if (SEManager.Ins.SeList[i].Chart.Id == id)
			{
				seData = SEManager.Ins.SeList[i];
			}
		}

		SEChart seChart = CsvData.Ins.SEChart[id];

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

	void SetCollectionEffect(string id, EquipmentData data, EquipmentChart chart)
	{
		SEData seData = null;

		for (int i = 0; i < SEManager.Ins.SeList.Count; i++)
		{
			if (SEManager.Ins.SeList[i].Chart.Id == id)
			{
				seData = SEManager.Ins.SeList[i];
			}
		}

		SEChart seChart = CsvData.Ins.SEChart[id];

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


}
