using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotInfo : MonoBehaviour
{
	public int No;
	public int Lv;	
	public Button LvUpBtn;
	public TextMeshProUGUI LvUpBtnText;
	public TextMeshProUGUI LvUpCostText;
	Material lvUpBtnMat;
	SlotData data;

	private void Start()
	{
		StageManager.Ins.GoldChanged += SetLvUpBtn;

		Image uiImage = LvUpBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.material);

		lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;
	}

	public void Init(SlotData data)
	{
		this.data = data;		
		SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
		SetLvUpBtn(0);
	}

	void SetLvUpCost(double cost)
	{
		LvUpCostText.text = ExtensionMethods.ToCurrencyString(cost);
	}

	void SetLvUpBtn(double value)
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
