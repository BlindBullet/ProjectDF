using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfoAttrIcon : MonoBehaviour
{
	public Image AttrImg;
	public GameObject CheckObj;
	public Attr Attr;

	public void SetIcon(Attr attr)
	{
		AttrImg.sprite = Resources.Load<Sprite>("Sprites/Icons/" + attr.ToString());
		Attr = attr;
		CheckObj.SetActive(false);
	}

	public void Check(bool isCheck)
	{
		if (isCheck)
		{
			CheckObj.SetActive(true);
		}
		else
		{
			CheckObj.SetActive(false);
		}
	}
}
