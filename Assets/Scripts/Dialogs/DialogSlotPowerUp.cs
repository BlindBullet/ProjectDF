using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSlotPowerUp : DialogController
{
	public static DialogSlotPowerUp _Dialog = null;

	public TextMeshProUGUI Title;
	public TextMeshProUGUI Desc;
	public Button HelpBtn;
	public GameObject HelpBubble;
	public TextMeshProUGUI HelpText;
	public PowerUpBar[] Bars;
	public Button RefreshBtn;
	public TextMeshProUGUI RefreshBtnText;
	public TextMeshProUGUI RefreshCostText;
	SlotData data;
	int powerUpNo = 0;
	public List<AtkUpgradeType> LotteriedBars = new List<AtkUpgradeType>();
	bool onHelp;
	Material refreshBtnMat;

	public void OpenDialog(SlotData data)
	{
		this.data = data;

		Image uiImage = RefreshBtn.GetComponent<Image>();
		uiImage.material = new Material(uiImage.materialForRendering);
		refreshBtnMat = RefreshBtn.GetComponent<Image>().materialForRendering;
		RefreshBtn.GetComponent<AllIn1SpriteShader.AllIn1Shader>().ApplyMaterialToHierarchy();

		Title.text = LanguageManager.Ins.SetString("title_popup_slot_power_up");
		Desc.text = LanguageManager.Ins.SetString("desc_popup_slot_power_up");
		HelpText.text = LanguageManager.Ins.SetString("help_slot_power_up");
		RefreshBtnText.text = LanguageManager.Ins.SetString("Refresh");
		RefreshCostText.text = ConstantData.PowerUpRefreshCost.ToString();
		onHelp = true;

		HelpBtn.onClick.RemoveAllListeners();
		HelpBtn.onClick.AddListener(() => 
		{
			SoundManager.Ins.PlaySFX("se_button_2");
			OnHelp();
		});

		powerUpNo = ConstantData.SlotPowerUpPossibleLv[data.Power];

		SetBars();
		SetRefreshBtn();		

		_Dialog = this;
		Show(false, true);
	}

	void SetBars()
	{
		bool canLoad = false;
		
		for(int i = 0; i < data.PowerUpListLvs.Count; i++)
		{
			if (data.Power == data.PowerUpListLvs[i])
			{
				LoadBars(data.PowerUpLists);
				canLoad = true;				
				break;
			}	
		}

		if(!canLoad)
			LotteryPowerUpBars();
	}

	void LoadBars(List<AtkUpgradeType> lotteriedUpgrades)
	{
		List<SlotPowerUpChart> charts = CsvData.Ins.SlotPowerUpChart[powerUpNo];

		for (int i = 0; i < lotteriedUpgrades.Count; i++)
		{
			Bars[i].SetBar(data, lotteriedUpgrades[i], charts[0].CostType, charts[0].Cost);
			LotteriedBars.Add(lotteriedUpgrades[i]);
		}
	}

	void LotteryPowerUpBars(bool isFirst = true)
	{
		LotteriedBars.Clear();
				
		List<SlotPowerUpChart> charts = CsvData.Ins.SlotPowerUpChart[powerUpNo];				
		List<AtkUpgradeType> upgrades = new List<AtkUpgradeType>();
		
		for(int i = 0; i < charts.Count; i++)
		{
			switch (charts[i].Type)
			{
				case AtkUpgradeType.Front:
					if (data.AtkData.Front < 4)
						upgrades.Add(AtkUpgradeType.Front);
					break;
				case AtkUpgradeType.Diagonal:
					if (data.AtkData.Diagonal < 2)
						upgrades.Add(AtkUpgradeType.Diagonal);
					break;
				case AtkUpgradeType.Multi:
					if (data.AtkData.Multi < 2)
						upgrades.Add(AtkUpgradeType.Multi);
					break;
				case AtkUpgradeType.Boom:
					if (data.AtkData.Boom < 3)
						upgrades.Add(AtkUpgradeType.Boom);
					break;
				case AtkUpgradeType.Size:
					if (data.AtkData.Size < 3)
						upgrades.Add(AtkUpgradeType.Size);
					break;
				case AtkUpgradeType.Push:
					if (data.AtkData.Push < 5)
						upgrades.Add(AtkUpgradeType.Push);
					break;					
				default:
					upgrades.Add(charts[i].Type);
					break;
			}			
		}

		List<AtkUpgradeType> results = new List<AtkUpgradeType>();

		if (!isFirst)		
		{
			for (int i = 0; i < upgrades.Count; i++)
			{
				for (int k = 0; k < data.PowerUpListLvs.Count; k++)
				{
					if (data.PowerUpListLvs[k] == data.Power)
					{
						if (data.PowerUpLists == null)
						{
							results.Add(upgrades[i]);
						}
					}
				}
			}			
		}
		else
		{
			results = upgrades;
		}

		results = Shuffle.ShuffleList(results);
		
		for (int i = 0; i < Bars.Length; i++)
		{
			Bars[i].SetBar(data, results[i], charts[0].CostType, charts[0].Cost);
			LotteriedBars.Add(results[i]);
		}

		data.SetLotteriedUpgradeBars(data.Power, LotteriedBars);

		StageManager.Ins.PlayerData.Save();
	}

	void SetRefreshBtn()
	{
		if (StageManager.Ins.PlayerData.SoulStone >= ConstantData.PowerUpRefreshCost)
		{
			refreshBtnMat.SetFloat("_GreyccaleBlend", 0f);
			RefreshBtn.enabled = true;

			RefreshBtn.onClick.RemoveAllListeners();
			RefreshBtn.onClick.AddListener(() =>
			{				
				SoundManager.Ins.PlaySFX("se_button_2");
				StageManager.Ins.ChangeSoulStone(-ConstantData.PowerUpRefreshCost);
				LotteryPowerUpBars(false);
			});
		}
		else
		{
			refreshBtnMat.SetFloat("_GreyccaleBlend", 1f);
			RefreshBtn.enabled = false;
		}
	}

	void OnHelp()
	{
		if (!onHelp)
		{
			HelpBubble.SetActive(true);
			HelpText.gameObject.SetActive(true);
			onHelp = true;
		}
		else
		{
			HelpBubble.SetActive(false);
			HelpText.gameObject.SetActive(false);
			onHelp = false;
		}
			
	}

	private void OnDisable()
	{
		_Dialog = null;
	}

}
