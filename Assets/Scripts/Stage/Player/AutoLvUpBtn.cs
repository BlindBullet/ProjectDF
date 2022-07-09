using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AutoLvUpBtn : MonoBehaviour
{	
	public Image Image;
	public Image Frame;
	public Button Btn;
	public GameObject OnObj;

	bool isOn = false;

	public void SetBtn(bool isPurchase)
	{
		if (isPurchase)
		{
			isOn = true;
			StageManager.Ins.PlayerData.SetAutoLvUp();
			StageManager.Ins.OnAutoLvUp();
		}
		else
		{			
			isOn = StageManager.Ins.PlayerData.AutoLvUp;

			if (isOn)
			{
				StageManager.Ins.OnAutoLvUp();
			}
			else
			{
				StageManager.Ins.OffAutoLvUp();
			}
		}
		
		SetState(isOn);


		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => SetAutoLvUp());
	}

	public void SetAutoLvUp()
	{
		StageManager.Ins.PlayerData.SetAutoLvUp();
		isOn = StageManager.Ins.PlayerData.AutoLvUp;

		if (isOn)
		{
			StageManager.Ins.OnAutoLvUp();
		}
		else
		{
			StageManager.Ins.OffAutoLvUp();
		}

		SetState(isOn);		
	}

	public void SetState(bool isOn)
	{
		if (isOn)
		{
			OnObj.SetActive(true);
		}
		else
		{
			OnObj.SetActive(false);
		}
	}


}
