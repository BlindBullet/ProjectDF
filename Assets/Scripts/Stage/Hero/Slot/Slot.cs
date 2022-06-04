using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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
	public Button PowerUpBtn;
	public Image PowerUpBtnFrame;
	Material lvUpBtnMat;
	Material powerUpBtnMat;
	SlotData data;	

	public void Init(SlotData data)
	{
		Image uiImage = LvUpBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.materialForRendering);
		lvUpBtnMat = LvUpBtn.GetComponent<Image>().material;
		LvUpBtn.GetComponent<AllIn1SpriteShader.AllIn1Shader>().ApplyMaterialToHierarchy();

		Image uiImage2 = PowerUpBtnFrame;
		uiImage2.material = new Material(PowerUpBtnFrame.materialForRendering);
		powerUpBtnMat = PowerUpBtnFrame.material;

		StageManager.Ins.GoldChanged += SetLvUpBtnState;

		this.data = data;

		LvUpBtnText.text = LanguageManager.Ins.SetString("LevelUp");

		LvUpBtn.onClick.RemoveAllListeners();
		LvUpBtn.onClick.AddListener(() =>
		{
			LevelUp();
			StageManager.Ins.PlayerData.Save();
		});

		PowerUpBtn.onClick.RemoveAllListeners();
		PowerUpBtn.onClick.AddListener(() =>
		{
			DialogManager.Ins.OpenSlotPowerUp(data);
		});

		LvObj.SetActive(true);
				
		SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
		SetLvUpBtnState(0);
		SetLvText();
		SetPowerUpBtn();
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

		foreach(KeyValuePair<int, List<SlotPowerUpChart>> elem in CsvData.Ins.SlotPowerUpChart)
		{
			if(elem.Key == data.Lv)
			{
				data.IncUpgradeStack();
				SetPowerUpBtn();
			}
		}

		SetLvUpBtnState(0);
		SetLvUpCost(ConstantData.GetLvUpCost(data.Lv));
		SetLvText();
	}

	void SetPowerUpBtn()
	{
		DOTween.Kill("SF" + No);

		if (data.PowerUpStack > 0)
		{
			PowerUpBtn.enabled = true;
			PowerUpBtn.gameObject.SetActive(true);
			powerUpBtnMat.DOFloat(6.28f, "_ShineRotate", 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("SF" + No);
		}
	}

	public void AfterPowerUp()
	{
		if (data.PowerUpStack <= 0)
		{
			DOTween.Kill("SF" + No);
			PowerUpBtn.gameObject.SetActive(false);
		}	
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
