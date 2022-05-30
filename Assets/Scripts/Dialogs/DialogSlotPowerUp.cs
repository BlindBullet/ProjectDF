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
	public PowerUpBar[] Bars;
	public Button RefreshBtn;
	public TextMeshProUGUI RefreshBtnText;
	SlotData data;
	int powerUpNo = 5;
	public List<AtkUpgradeType> LotteriedBars = new List<AtkUpgradeType>();

	public void OpenDialog(SlotData data)
	{
		this.data = data;
		Title.text = LanguageManager.Ins.SetString("title_popup_slot_power_up");
		Desc.text = LanguageManager.Ins.SetString("desc_popup_slot_power_up");
		RefreshBtnText.text = LanguageManager.Ins.SetString("Refresh");

		powerUpNo = ConstantData.SlotPowerUpPossibleLv[data.PowerUpLv];

		SetBars();
		SetRefreshBtn();

		_Dialog = this;
		Show(false);
	}

	void SetBars()
	{
		bool canLoad = false;

		for(int i = 0; i < data.PowerUpList.Count; i++)
		{
			if (data.PowerUpLv == data.PowerUpList[i].Lv)
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
			probs.Add(charts[i].Prob);
		}

		List<int> results = LotteryCalculator.LotteryListNoVerbose(probs, 3);
		data.SetLotteriedUpgradeBars(data.Lv, results);

		for(int i = 0; i < Bars.Length; i++)
		{
			Bars[i].SetBar(data, charts[results[i]]);
			LotteriedBars.Add(charts[results[i]].Type);
		}

		StageManager.Ins.PlayerData.Save();
	}

	void RefreshPowerUpBars()
	{
		List<SlotPowerUpChart> charts = CsvData.Ins.SlotPowerUpChart[powerUpNo];

		List<int> probs = new List<int>();

		for (int i = 0; i < charts.Count; i++)
		{
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

		data.SetLotteriedUpgradeBars(data.Lv, results);

		for (int i = 0; i < Bars.Length; i++)
		{
			Bars[i].SetBar(data, charts[results[i]]);
			LotteriedBars.Add(charts[results[i]].Type);
		}

		StageManager.Ins.PlayerData.Save();
	}

	void SetRefreshBtn()
	{
		RefreshBtn.onClick.RemoveAllListeners();
		RefreshBtn.onClick.AddListener(() =>
		{
			//����
			RefreshPowerUpBars();
		});
	}

}
