using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;

public class EquipmentIcon : MonoBehaviour
{
	public static List<EquipmentIcon> Icons = new List<EquipmentIcon>();

	public Button Btn;
	public Image Bg;
	public Image Image;
	public Image Frame;
	public Image CountFill;
	public TextMeshProUGUI CountText;
	public GameObject LockObj;
	EquipmentData data = null;

	public void Setup(EquipmentData data)
	{
		this.data = data;

		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => { });

		SetCount(data);
		SetLock(data);
		SetIcon();
		Icons.Add(this);
	}

	void SetIcon()
	{
		EquipmentChart chart = CsvData.Ins.EquipmentChart[data.Id];

		Bg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Equipment_Bg_" + chart.Grade.ToString());
		Frame.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Equipment_Frame_" + chart.Grade.ToString());
		Image.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.Icon);
	}

	public void SetCount(EquipmentData data)
	{
		CountFill.fillAmount = data.Count >= 5 ? 1f : (float)data.Count / 5f;
		CountText.text = data.Count + " / 5";
	}

	public void SetLock(EquipmentData data)
	{
		if (data.isOpen)
		{
			LockObj.SetActive(true);
		}
		else
		{
			LockObj.SetActive(false);
		}
	}
}
