using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogConfirmAndCancel : DialogController
{
	public TextMeshProUGUI Title;
	public TextMeshProUGUI Desc;
	public Button ConfirmBtn;
	public TextMeshProUGUI ConfirmBtnText;	

	public void SetDialog(string title, string desc, Action action)
	{
		SetBasic(title, desc);
		ConfirmBtn.onClick.RemoveAllListeners();
		ConfirmBtn.onClick.AddListener(() => { action(); CloseDialog(); });

		Show(false, true);
	}

	void SetBasic(string title, string desc)
	{
		Title.text = LanguageManager.Ins.SetString(title);
		Desc.text = LanguageManager.Ins.SetString(desc);
		ConfirmBtnText.text = LanguageManager.Ins.SetString("Confirm");
	}
}
