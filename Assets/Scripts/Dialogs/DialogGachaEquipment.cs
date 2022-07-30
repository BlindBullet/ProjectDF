using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogGachaEquipment : DialogController
{
	public List<EquipmentRewardIcon> Icons = new List<EquipmentRewardIcon>();

	public Button Gacha15Btn;
	public TextMeshProUGUI Gacha15BtnText;
	public Button Gacha35Btn;
	public TextMeshProUGUI Gacha35BtnText;
	
	public void OpenDialog(List<string> ids)
	{
		Gacha15BtnText.text = LanguageManager.Ins.SetString("Gacha15");
		Gacha35BtnText.text = LanguageManager.Ins.SetString("Gacha35");

		Gacha15Btn.onClick.RemoveAllListeners();
		Gacha15Btn.onClick.AddListener(() => { });

		Gacha35Btn.onClick.RemoveAllListeners();
		Gacha35Btn.onClick.AddListener(() => { });

		StartCoroutine(SetIcons(ids));
		Show(false);
	}

	IEnumerator SetIcons(List<string> ids)
	{		
		CloseBtn.gameObject.SetActive(false);

		for(int i = 0; i < Icons.Count; i++)
		{
			Icons[i].gameObject.SetActive(false);
		}

		for(int i = 0; i < ids.Count; i++)
		{
			var chart = CsvData.Ins.EquipmentChart[ids[i]];
			Icons[i].gameObject.SetActive(true);
			Icons[i].SetIcon(chart);

			yield return new WaitForSeconds(0.1f);
		}
		
		CloseBtn.gameObject.SetActive(true);
	}






}
