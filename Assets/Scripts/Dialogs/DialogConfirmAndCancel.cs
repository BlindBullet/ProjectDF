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
	public Button CancelBtn;
	public TextMeshProUGUI CancelBtnText;
	public Button CloseBtn;

	public void SetDialog(string title, string desc, Action action)
	{
		SetBasic(title, desc);
		ConfirmBtn.onClick.RemoveAllListeners();
		ConfirmBtn.onClick.AddListener(() => { action(); CloseDialog(); });

		Show(true);
	}

	void SetBasic(string title, string desc)
	{
		Title.text = LanguageManager.Ins.SetString(title);
		Desc.text = LanguageManager.Ins.SetString(desc);
		ConfirmBtnText.text = LanguageManager.Ins.SetString("Confirm");
		CancelBtnText.text = LanguageManager.Ins.SetString("Cancel");

		CancelBtn.onClick.RemoveAllListeners();
		CancelBtn.onClick.AddListener(() => CloseDialog());

		CloseBtn.onClick.RemoveAllListeners();
		CloseBtn.onClick.AddListener(() => CloseDialog());

	}
}
