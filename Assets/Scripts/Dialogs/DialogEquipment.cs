using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogEquipment : DialogController
{
	public List<EquipmentIcon> Icons = new List<EquipmentIcon>();

	public TextMeshProUGUI TitleText;
	public Button RedBtn;
	public GameObject RedSelectedObj;
	public Button BlueBtn;
	public GameObject BlueSelectedObj;
	public Button GreenBtn;
	public GameObject GreenSelectedObj;
	public Button AccBtn;
	public GameObject AccSelectedObj;

	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("Equipment");
		RedBtn.onClick.RemoveAllListeners();
		RedBtn.onClick.AddListener(() => ClickBtn(EquipmentType.Red));
		BlueBtn.onClick.RemoveAllListeners();
		BlueBtn.onClick.AddListener(() => ClickBtn(EquipmentType.Blue));
		GreenBtn.onClick.RemoveAllListeners();
		GreenBtn.onClick.AddListener(() => ClickBtn(EquipmentType.Green));
		AccBtn.onClick.RemoveAllListeners();
		AccBtn.onClick.AddListener(() => ClickBtn(EquipmentType.Acc));

		ClickBtn(EquipmentType.Red);
		Show(true);
	}

	void SetEquipmentIcons(EquipmentType type)
	{
		List<EquipmentData> datas = new List<EquipmentData>();

		for(int i = 0; i < StageManager.Ins.EquipmentData.Datas.Count; i++)
		{
			if (StageManager.Ins.EquipmentData.Datas[i].Type == type)
				datas.Add(StageManager.Ins.EquipmentData.Datas[i]);
		}

		datas.Sort(delegate (EquipmentData A, EquipmentData B)
		{
			int a = int.Parse(A.Id);
			int b = int.Parse(B.Id);

			if (a >= b)
				return 1;
			else
				return -1;
		});
		
		for(int i = 0; i < Icons.Count; i++)
		{
			Icons[i].Setup(datas[i]);
		}
	}

	void ClickBtn(EquipmentType type)
	{
		SetEquipmentIcons(type);

		switch (type)
		{
			case EquipmentType.Red:
				RedSelectedObj.SetActive(true);
				BlueSelectedObj.SetActive(false);
				GreenSelectedObj.SetActive(false);
				AccSelectedObj.SetActive(false);
				break;
			case EquipmentType.Blue:
				RedSelectedObj.SetActive(false);
				BlueSelectedObj.SetActive(true);
				GreenSelectedObj.SetActive(false);
				AccSelectedObj.SetActive(false);
				break;
			case EquipmentType.Green:
				RedSelectedObj.SetActive(false);
				BlueSelectedObj.SetActive(false);
				GreenSelectedObj.SetActive(true);
				AccSelectedObj.SetActive(false);
				break;
			case EquipmentType.Acc:
				RedSelectedObj.SetActive(false);
				BlueSelectedObj.SetActive(false);
				GreenSelectedObj.SetActive(false);
				AccSelectedObj.SetActive(true);
				break;
		}
	}


}
