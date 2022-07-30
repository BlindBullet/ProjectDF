using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponGachaBar : MonoBehaviour
{
	public static WeaponGachaBar _Bar = null;

	public TextMeshProUGUI TitleText;
	public Button InfoBtn;
	public Button Gacha15Btn;
	public TextMeshProUGUI Gacha15BtnText;
	public Button Gacha35Btn;
	public TextMeshProUGUI Gacha35BtnText;
	public TextMeshProUGUI LvText;
	public TextMeshProUGUI ExpText;
	public Image ExpFill;

	public void Setup()
	{
		TitleText.text = LanguageManager.Ins.SetString("Summon_Weapon");

		InfoBtn.onClick.RemoveAllListeners();
		InfoBtn.onClick.AddListener(() => { });

		Gacha15Btn.onClick.RemoveAllListeners();
		Gacha15Btn.onClick.AddListener(() => 
		{
			if(StageManager.Ins.PlayerData.SoulStone >= 500)
			{
				StageManager.Ins.ChangeSoulStone(-500);
				var result = StageManager.Ins.EquipmentData.GachaWeapon(15);
				DialogManager.Ins.OpenGachaEquipmentInfo(result);
				SetLvAndExp();
			}
			else
			{
				DialogManager.Ins.OpenCautionBar("not_enough_soulstone");
			}
		});

		Gacha35Btn.onClick.RemoveAllListeners();
		Gacha35Btn.onClick.AddListener(() => 
		{
			if (StageManager.Ins.PlayerData.SoulStone >= 1200)
			{
				StageManager.Ins.ChangeSoulStone(-1200);
				var result = StageManager.Ins.EquipmentData.GachaWeapon(35);
				DialogManager.Ins.OpenGachaEquipmentInfo(result);
				SetLvAndExp();
			}
			else
			{
				DialogManager.Ins.OpenCautionBar("not_enough_soulstone");
			}
		});

		SetLvAndExp();

		_Bar = this;
	}

	public void SetLvAndExp()
	{
		int lv = StageManager.Ins.EquipmentData.WeaponLv;
		int exp = StageManager.Ins.EquipmentData.WeaponExp;
		var chart = CsvData.Ins.EquipmentLvChart[lv.ToString()];

		LvText.text = lv.ToString();
		ExpText.text = exp + " / " + chart.NeedCount;
		ExpFill.fillAmount = (float)exp / (float)chart.NeedCount;
	}


}
