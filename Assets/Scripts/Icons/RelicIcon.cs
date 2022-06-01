using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using System;

public class RelicIcon : MonoBehaviour
{	
	public Image IconImg;
	public TextMeshProUGUI LvText;
	public GameObject LockObj;

	public void SetIcon(RelicData data)
	{
		RelicChart chart = CsvData.Ins.RelicChart[data.Id];

		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.Icon);
		
		SetLock(data);
		SetLevel(data, chart);
	}

	void SetLevel(RelicData data, RelicChart chart)
	{
		if(chart.MaxLv <= 0)
		{
			LvText.text = "Lv." + data.Lv;
		}
		else
		{
			if (data.Lv >= chart.MaxLv)
				LvText.text = "Lv." + data.Lv + "(Max)";
			else
				LvText.text = "Lv." + data.Lv;
		}
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

}
