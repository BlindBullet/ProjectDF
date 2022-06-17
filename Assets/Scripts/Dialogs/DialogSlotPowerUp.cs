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
		
		for(int i = 0; i < data.PowerUpList.Count; i++)
		{
			if (data.Power == data.PowerUpList[i].Lv)
			{
				LoadBars(data.PowerUpList[i].LotteriedPowerUpBars);
				canLoad = true;				
				break;
			}	
		}

		if(!canLoad)
			LotteryPowerUpBars();
	}

	void LoadBars(List<int> lotteriedNos)
	{
		List<SlotPowerUpChart> charts = CsvData.Ins.SlotPowerUpChart[powerUpNo];

		for (int i = 0; i < lotteriedNos.Count; i++)
		{
			Bars[i].SetBar(data, charts[lotteriedNos[i]]);
			LotteriedBars.Add(charts[lotteriedNos[i]].Type);
		}
	}

	void LotteryPowerUpBars()
	{
		LotteriedBars.Clear();

		List<SlotPowerUpChart> charts = CsvData.Ins.SlotPowerUpChart[powerUpNo];

		List<int> probs = new List<int>();
		
		for(int i = 0; i < charts.Count; i++)
		{
			bool _add = false;

			switch (charts[i].Type)
			{
				case AtkUpgradeType.Front:
					if (data.AtkData.Front < 5)
						_add = true;
					break;
				case AtkUpgradeType.Diagonal:
					if (data.AtkData.Diagonal < 3)
						_add = true;
					break;
				case AtkUpgradeType.Multi:
					if (data.AtkData.Multi < 3)
						_add = true;
					break;
				case AtkUpgradeType.Boom:
					if (data.AtkData.Boom < 3)
						_add = true;
					break;
				case AtkUpgradeType.Size:
					if (data.AtkData.Size < 3)
						_add = true;
					break;
				case AtkUpgradeType.Push:
					if (data.AtkData.Push < 5)
						_add = true;
					break;
				default:
					_add = true;
					break;
			}
			
			if(_add)
				probs.Add(charts[i].Prob);
		}

		List<int> results = LotteryCalculator.LotteryListNoVerbose(probs, 3);
		data.SetLotteriedUpgradeBars(data.Power, results);

		for(int i = 0; i < Bars.Length; i++)
		{
			Bars[i].SetBar(data, charts[results[i]]);
			LotteriedBars.Add(charts[results[i]].Type);
		}

		StageManager.Ins.PlayerData.Save();
	}

	public void RefreshPowerUpBars()
	{
		LotteriedBars.Clear();
		List<SlotPowerUpChart> charts = CsvData.Ins.SlotPowerUpChart[powerUpNo];

		List<int> probs = new List<int>();

		for (int i = 0; i < charts.Count; i++)
		{
			bool _add = false;

			switch (charts[i].Type)
			{
				case AtkUpgradeType.Front:
					if (data.AtkData.Front < 5)
						_add = true;
					break;
				case AtkUpgradeType.Diagonal:
					if (data.AtkData.Diagonal < 4)
						_add = true;
					break;
				case AtkUpgradeType.Multi:
					if (data.AtkData.Multi < 4)
						_add = true;
					break;
				case AtkUpgradeType.Boom:
					if (data.AtkData.Boom < 3)
						_add = true;
					break;
				case AtkUpgradeType.Size:
					if (data.AtkData.Size < 3)
						_add = true;
					break;
				case AtkUpgradeType.Push:
					if (data.AtkData.Push < 5)
						_add = true;
					break;
				default:
					_add = true;
					break;
			}

			if (_add)
				probs.Add(charts[i].Prob);
		}

		bool success = false;
		List<int> results = new List<int>();

		while (!success)
		{
			results.Clear();
			results = LotteryCalculator.LotteryListNoVerbose(probs, 3);

			int verboseCount = 0;

			for (int i = 0; i < results.Count; i++)
			{
				for (int k = 0; k < LotteriedBars.Count; k++)
				{
					if (LotteriedBars[k] == charts[results[i]].Type)
					{
						verboseCount++;
						break;
					}
				}
			}

			if(verboseCount < 3)
			{
				success = true;
			}
		}

		data.SetLotteriedUpgradeBars(data.Power, results);
		
		for (int i = 0; i < Bars.Length; i++)
		{
			Bars[i].SetBar(data, charts[results[i]]);
			LotteriedBars.Add(charts[results[i]].Type);
		}

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
				RefreshPowerUpBars();
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
