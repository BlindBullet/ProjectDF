using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogEquipment : DialogController
{
	public static DialogEquipment _Dialog = null;
	public List<EquipmentIcon> Icons = new List<EquipmentIcon>();

	public TextMeshProUGUI TitleText;
	public Button WeaponBtn;
	public GameObject WeaponSelectedObj;	
	public Button AccBtn;
	public GameObject AccSelectedObj;

	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("Equipment");		
		WeaponBtn.onClick.RemoveAllListeners();
		WeaponBtn.onClick.AddListener(() => ClickBtn(EquipmentType.Weapon));
		AccBtn.onClick.RemoveAllListeners();
		AccBtn.onClick.AddListener(() => ClickBtn(EquipmentType.Acc));

		ClickBtn(EquipmentType.Weapon);
		_Dialog = this;
		Show(true);
	}

	public void SetEquipmentIcons(EquipmentType type)
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
			Icons[i].Setup(datas[i], OpenEquipmentInfo);
		}
	}

	void OpenEquipmentInfo(EquipmentData data)
	{
		DialogManager.Ins.OpenEquipmentInfo(data);
	}

	void ClickBtn(EquipmentType type)
	{
		SetEquipmentIcons(type);

		switch (type)
		{
			case EquipmentType.Weapon:
				WeaponSelectedObj.SetActive(true);				
				AccSelectedObj.SetActive(false);
				break;			
			case EquipmentType.Acc:
				WeaponSelectedObj.SetActive(false);				
				AccSelectedObj.SetActive(true);
				break;
		}
	}

	private void OnDisable()
	{
		_Dialog = null;
	}
}
