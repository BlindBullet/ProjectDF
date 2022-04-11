using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
	public int No;	
	public Button LvUpBtn;
	public TextMeshProUGUI LvText;
	public TextMeshProUGUI LvUpBtnText;
	public TextMeshProUGUI LvUpCostText;
	Material lvUpBtnMat;
	SlotData data;

	private void Start()
	{
		StageManager.Ins.GoldChanged += SetLvUpBtnState;

		Image uiImage = LvUpBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.material);

		lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;
	}

	public void Init(SlotData data)
	{
		this.data = data;
		SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
		SetLvUpBtnState(0);
		SetLvText();

		LvUpBtn.onClick.RemoveAllListeners();
		LvUpBtn.onClick.AddListener(() => 
		{
			LevelUp();
		});
	}

	void LevelUp()
	{
		if (data.LevelUp())
		{
			for (int i = 0; i < HeroBase.Heroes.Count; i++)
			{
				if (HeroBase.Heroes[i].Data.SlotNo == No)
				{
					HeroBase.Heroes[i].ChangeLv(data.Lv);
				}
			}
		}

		SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
		SetLvText();
	}

	void SetLvText()
	{
		LvText.text = data.Lv.ToString();
	}

	void SetLvUpCost(double cost)
	{
		LvUpCostText.text = ExtensionMethods.ToCurrencyString(cost);
	}

	void SetLvUpBtnState(double value)
	{
		if (StageManager.Ins.PlayerData.Gold >= ConstantData.GetLvUpCost(data.Lv))
		{
			LvUpBtnEnable();
		}
		else
		{
			LvUpBtnDisable();
		}
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

}
