using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
	public int No;	
	public Button LvUpBtn;
	public Image GoldIconImg;
	public GameObject LvObj;
	public TextMeshProUGUI LvText;
	public TextMeshProUGUI LvUpBtnText;
	public TextMeshProUGUI LvUpCostText;
	public GameObject EnchantLabelObj;
	public TextMeshProUGUI EnchantLvText;
	public TextMeshProUGUI AtkText;
	Material lvUpBtnMat;	
	SlotData data;

	public void Init(SlotData data)
	{
		Image uiImage = LvUpBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.materialForRendering);
		lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;

		LvUpBtn.GetComponent<AllIn1SpriteShader.AllIn1Shader>().ApplyMaterialToHierarchy();

		StageManager.Ins.GoldChanged += SetLvUpBtnState;

		this.data = data;

		LvUpBtnText.text = LanguageManager.Ins.SetString("LevelUp");

		LvUpBtn.onClick.RemoveAllListeners();
		LvUpBtn.onClick.AddListener(() =>
		{
			LevelUp();
			StageManager.Ins.PlayerData.Save();
		});

		LvObj.SetActive(true);

		SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
		SetLvUpBtnState(0);
		SetLvText();
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

		SetLvUpBtnState(0);
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
		lvUpBtnMat.SetFloat("_GreyscaleBlend", 0f);
	}

	public void LvUpBtnDisable()
	{
		LvUpBtn.enabled = false;
		lvUpBtnMat.SetFloat("_GreyscaleBlend", 1f);
	}

	public void SetEnchantLabel(HeroData data)
	{
		if(data.EnchantLv > 0)
		{
			EnchantLabelObj.SetActive(true);
			EnchantLvText.text = "+" + data.EnchantLv;
		}
		else
		{
			EnchantLabelObj.SetActive(false);
		}
	}

	public void SetAtk(double atk)
	{
		AtkText.text = atk.ToCurrencyString();
	}

	public void Lose()
	{
		LvUpBtn.enabled = false;
		LvObj.SetActive(false);
	}

	private void OnDisable()
	{		
		StageManager.Ins.GoldChanged -= SetLvUpBtnState;
	}


}
