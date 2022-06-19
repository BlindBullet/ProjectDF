using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.U2D;
using AllIn1SpriteShader;

public class RelicBar : MonoBehaviour
{
	public RelicIcon RelicIcon;
	public TextMeshProUGUI Name;
	public TextMeshProUGUI Desc;
	public TextMeshProUGUI Desc2;
	public Button LvUpBtn;
	public Image LvUpBtnImg;
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
		lvUpBtnMat = LvUpBtn.GetComponent<Image>().materialForRendering;
		LvUpBtn.GetComponent<AllIn1Shader>().ApplyMaterialToHierarchy();

		this.data = data;
		chart = CsvData.Ins.RelicChart[data.Id];
		Name.text = LanguageManager.Ins.SetString(chart.Name);
		SetInfo();
		SetBtn();
	}

	public void SetInfo()
	{		
		SEData seData = null;

		for(int i = 0; i < SEManager.Ins.SeList.Count; i++)
		{
			if(SEManager.Ins.SeList[i].Chart.Id == chart.Effect)
			{
				seData = SEManager.Ins.SeList[i];
				break;
			}
		}

		SEChart seChart = CsvData.Ins.SEChart[chart.Effect];

		if (seData == null)		
			seData = new SEData(seChart, data.Lv);		
		
		double value1 = seData.SetValue();
		double value2 = seData.NextSetValue();

		RelicIcon.SetIcon(data);

		string _desc = "";

		if (data.Lv == chart.MaxLv)
		{
			switch (seChart.EParam2)
			{
				case "Gold":
					if (seChart.EffectType == SEEffectType.StatChange)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_max"), value1.ToCurrencyString());
					else if (seChart.EffectType == SEEffectType.Ascension)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_max"), value1.ToCurrencyString());
					break;
				case "Time":
					if (seChart.EffectType == SEEffectType.OfflineReward)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_t_max"), value1);
					else
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_max"), Math.Round(value1, 1));
					break;
				case "StartStage":
					_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_max"), Math.Round(value1, 1));
					break;
				default:
					_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p_max"), Math.Round(value1, 1));
					break;
			}
		}
		else
		{
			switch (seChart.EParam2)
			{
				case "Gold":
					if (seChart.EffectType == SEEffectType.StatChange)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p"), value1.ToCurrencyString(), value2.ToCurrencyString());
					else if (seChart.EffectType == SEEffectType.Ascension)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc"), value1.ToCurrencyString(), value2.ToCurrencyString());
					break;
				case "Time":
					if (seChart.EffectType == SEEffectType.OfflineReward)
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_t"), value1, value2);
					else
						_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p"), Math.Round(value1, 1), Math.Round(value2, 1));
					break;
				case "StartStage":
					_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc"), Math.Round(value1, 1), Math.Round(value2, 1));
					break;
				default:
					_desc = string.Format(LanguageManager.Ins.SetString("se_inc_desc_p"), Math.Round(value1, 1), Math.Round(value2, 1));
					break;
			}
		}

		Desc.text = LanguageManager.Ins.SetString(chart.Desc);
		Desc2.text = _desc;
		SetBtn();
	}

	void SetBtn()
	{
		RelicChart chart = CsvData.Ins.RelicChart[data.Id];

		switch (chart.Type)
		{
			case RelicType.Relic:
				LvUpBtnImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Btn_Purple");
				break;
			case RelicType.Castle:
				LvUpBtnImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Btn_Brown");
				break;
		}

		LvUpBtn.onClick.RemoveAllListeners();

		if (data.isOwn)
		{
			LvUpBtnText.text = LanguageManager.Ins.SetString("LevelUp");
			CostIcon.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.LvUpCostType.ToString());
			double cost = (chart.LvUpCost + (chart.LvUpCostIncValue * (data.Lv - 1))) * (chart.LvUpCostIncRate > 1f ? (1 + Mathf.Pow(chart.LvUpCostIncRate, data.Lv - 1)) : 1f);
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
					SoundManager.Ins.PlaySFX("se_button_2");

					if (data.LevelUp())
					{
						SEManager.Ins.Apply();
						
						if (DialogRelic._Dialog != null)
							DialogRelic._Dialog.SetBars();

						if (DialogCastle._Dialog != null)
							DialogCastle._Dialog.SetBars();

						StageManager.Ins.PlayerData.Save();
					}
				});
			}
			else
			{
				LvUpBtn.enabled = false;
				lvUpBtnMat.SetFloat("_GreyscaleBlend", 1f);
			}

			if (data.Lv >= chart.MaxLv)
			{
				LvUpBtn.gameObject.SetActive(false);
			}	
		}
		else
		{
			LvUpBtnText.text = LanguageManager.Ins.SetString("Purchase");
			CostIcon.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.PriceCostType.ToString());
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
					SoundManager.Ins.PlaySFX("se_button_2");

					if (data.Puechase())
					{						
						SEManager.Ins.Apply();
						
						if (DialogRelic._Dialog != null)
							DialogRelic._Dialog.SetBars();

						if (DialogCastle._Dialog != null)
							DialogCastle._Dialog.SetBars();

						StageManager.Ins.PlayerData.Save();
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
