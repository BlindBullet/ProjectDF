using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RelicBar : MonoBehaviour
{
	public RelicIcon RelicIcon;
	public TextMeshProUGUI Name;
	public TextMeshProUGUI Desc;
	public Button LvUpBtn;
	public TextMeshProUGUI LvUpBtnText;
	public Image CostIcon;
	public TextMeshProUGUI Cost;
	RelicData data;
	RelicChart chart;
	Material lvUpBtnMat;

	public void SetBar(RelicData data)
	{
		Image uiImage = LvUpBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.materialForRendering);
		lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;

		this.data = data;
		chart = CsvData.Ins.RelicChart[data.Id];		
		Name.text = LanguageManager.Ins.SetString(chart.Name);
		SetInfo();
		SetBtn();
	}

	public void SetInfo()
	{	
		SEChart seChart = CsvData.Ins.SEChart[chart.Effect][data.Lv - 1];
		RelicIcon.SetIcon(data);
		Desc.text = string.Format(LanguageManager.Ins.SetString(chart.Desc), seChart.EParam5);
		SetBtn();
	}

	void SetBtn()
	{
		LvUpBtn.onClick.RemoveAllListeners();

		if (data.isOwn)
		{
			LvUpBtnText.text = LanguageManager.Ins.SetString("LevelUp");
			CostIcon.sprite = Resources.Load<Sprite>("Sprites/Cost/" + chart.LvUpCostType.ToString());
			double cost = chart.LvUpCost * (1 + Mathf.Pow(chart.LvUpCostIncRate, data.Lv));
			Cost.text = cost.ToCurrencyString();

			bool canPurchase = false;

			switch (chart.PriceCostType)
			{
				case CostType.Gold:
					if (StageManager.Ins.PlayerData.Gold >= cost)
						canPurchase = true;
					break;
				case CostType.Magicite:
					if (StageManager.Ins.PlayerData.Magicite >= cost)
						canPurchase = true;
					break;
				case CostType.SoulStone:
					if (StageManager.Ins.PlayerData.SoulStone >= cost)
						canPurchase = true;
					break;
			}

			if (canPurchase)
			{
				LvUpBtn.enabled = true;
				lvUpBtnMat.SetFloat("_GreyscaleBlend", 0f);

				LvUpBtn.onClick.RemoveAllListeners();
				LvUpBtn.onClick.AddListener(() =>
				{
					if (data.LevelUp())
					{
						SEManager.Ins.Apply();
						SetInfo();
					}
				});
			}
			else
			{
				LvUpBtn.enabled = false;
				lvUpBtnMat.SetFloat("_GreyscaleBlend", 1f);
			}			
		}
		else
		{
			LvUpBtnText.text = LanguageManager.Ins.SetString("Purchase");
			CostIcon.sprite = Resources.Load<Sprite>("Sprites/Cost/" + chart.PriceCostType.ToString());
			double cost = chart.Price;
			Cost.text = cost.ToCurrencyString();
			
			bool canPurchase = false;

			switch (chart.PriceCostType)
			{
				case CostType.Gold:
					if (StageManager.Ins.PlayerData.Gold >= cost)
						canPurchase = true;
					break;
				case CostType.Magicite:
					if (StageManager.Ins.PlayerData.Magicite >= cost)
						canPurchase = true;
					break;
				case CostType.SoulStone:
					if (StageManager.Ins.PlayerData.SoulStone >= cost)
						canPurchase = true;
					break;
			}

			if (canPurchase)
			{
				LvUpBtn.enabled = true;
				lvUpBtnMat.SetFloat("_GreyscaleBlend", 0f);

				LvUpBtn.onClick.RemoveAllListeners();
				LvUpBtn.onClick.AddListener(() =>
				{
					if (data.Puechase())
					{						
						SEManager.Ins.Apply();
						SetInfo();
					}
				});
			}
			else
			{
				LvUpBtn.enabled = false;
				lvUpBtnMat.SetFloat("_GreyscaleBlend", 1f);
			}
		}
	}

}
