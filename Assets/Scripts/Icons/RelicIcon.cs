using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using System;

public class RelicIcon : MonoBehaviour
{
	public Button Btn;
	public Image IconImg;
	public TextMeshProUGUI LvText;
	public GameObject LockObj;

	public void SetIcon(RelicData data, Action<RelicData> action = null)
	{
		RelicChart chart = CsvData.Ins.RelicChart[data.Id];

		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Relics/Relics").GetSprite(chart.Icon);
				
		SetLock(data);
		SetLevel(data, chart);
		SetBtn(data, action);
	}

	void SetLevel(RelicData data, RelicChart chart)
	{
		if (data.Lv >= chart.MaxLv)
			LvText.text = "Lv." + data.Lv + "(Max)";
		else
			LvText.text = "Lv." + data.Lv;
	}

	public void SetLock(RelicData data)
	{
		if(!data.isOwn)
		{
			LvText.gameObject.SetActive(false);
			LockObj.SetActive(true);
		}
		else
		{
			LvText.gameObject.SetActive(true);
			LockObj.SetActive(false);
		}
	}

	void SetBtn(RelicData data, Action<RelicData> action)
	{
		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => { if (action != null) action(data); });
	}

}
