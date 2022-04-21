using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogRelicInfo : DialogController
{
	public TextMeshProUGUI Name;
	public RelicIcon RelicIcon;
	public TextMeshProUGUI DescLabel;
	public TextMeshProUGUI Desc;
	public Button LvUpBtn;
	public TextMeshProUGUI LvUpBtnText;
	public Image LvUpCostIcon;
	public TextMeshProUGUI LvUpCostText;
	Material lvUpBtnMat;

	public void OpenDialog(RelicData data)
	{
		Image uiImage = LvUpBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.material);

		lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;
		LvUpBtn.GetComponent<AllIn1SpriteShader.AllIn1Shader>().ApplyMaterialToHierarchy();

		RelicChart chart = CsvData.Ins.RelicChart[data.Id];

		Name.text = LanguageManager.Ins.SetString(chart.Name);
		RelicIcon.SetIcon(data);
		SetDesc(data, chart);		
		SetLvUpBtn(data, chart);

		Show(true);
	}

	void SetDesc(RelicData data, RelicChart chart)
	{
		List<SEChart> seList = CsvData.Ins.SEChart[chart.Effect];
		SEChart se = null;

		for(int i = 0; i < seList.Count; i++)
		{
			if (seList[i].Lv == data.Lv)
				se = seList[i];
		}
		
		DescLabel.text = LanguageManager.Ins.SetString("RelicEffect");

		switch (se.EffectType)
		{
			case SEEffectType.StatChange:
				Desc.text = string.Format(LanguageManager.Ins.SetString(chart.Desc), se.EParam3);
				break;
			case SEEffectType.AddCurrency:
				Desc.text = string.Format(LanguageManager.Ins.SetString(chart.Desc), se.EParam3);
				break;
			case SEEffectType.StartStage:
				Desc.text = string.Format(LanguageManager.Ins.SetString(chart.Desc), se.EParam2);
				break;
		}
	}

	void SetLvUpBtn(RelicData data, RelicChart chart)
	{
		if(data.Lv >= chart.MaxLv)
		{
			LvUpBtn.gameObject.SetActive(false);
			return;
		}

		if (data.isOwn)
		{
			LvUpBtnText.text = LanguageManager.Ins.SetString("Upgrade");
			double cost = chart.LvUpCost * (1 + Mathf.Pow(chart.LvUpCostIncRate, data.Lv));

			LvUpCostText.text = cost.ToCurrencyString();

			if (StageManager.Ins.PlayerData.Magicite >= cost)
			{
				LvUpBtn.enabled = true;
				lvUpBtnMat.DisableKeyword("GREYSCALE_ON");

				LvUpBtn.onClick.RemoveAllListeners();
				LvUpBtn.onClick.AddListener(() =>
				{
					if (data.LevelUp())
					{						
						DialogRelic._DialogRelic.SetRelics();
						RelicIcon.SetIcon(data);
						SEManager.Ins.Apply();
						SetLvUpBtn(data, chart);
					}
				});
			}
			else
			{
				LvUpBtn.enabled = false;
				lvUpBtnMat.EnableKeyword("GREYSCALE_ON");
			}
		}
		else
		{
			LvUpBtnText.text = LanguageManager.Ins.SetString("Purchase");
			double cost = chart.Price;

			if (StageManager.Ins.PlayerData.Magicite >= cost)
			{
				LvUpBtn.enabled = true;
				lvUpBtnMat.DisableKeyword("GREYSCALE_ON");

				LvUpBtn.onClick.RemoveAllListeners();
				LvUpBtn.onClick.AddListener(() =>
				{
					if (data.Puechase())
					{
						DialogRelic._DialogRelic.SetRelics();
						RelicIcon.SetIcon(data);
						SEManager.Ins.Apply();
						SetLvUpBtn(data, chart);
					}
				});
			}
			else
			{
				LvUpBtn.enabled = false;
				lvUpBtnMat.EnableKeyword("GREYSCALE_ON");
			}
		}
	}

}
