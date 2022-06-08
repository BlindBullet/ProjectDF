using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerMenu : MonoBehaviour
{
	public RectTransform MenuTrf;	
	public Button SettingBtn;
	public TextMeshProUGUI SettingBtnText;

	private void Start()
	{		
		SettingBtn.onClick.RemoveAllListeners();
		SettingBtn.onClick.AddListener(() => { DialogManager.Ins.OpenSetting(); });
		SettingBtnText.text = LanguageManager.Ins.SetString("Setting");
	}

	public void Open()
	{
		MenuTrf.DOSizeDelta(new Vector2(MenuTrf.sizeDelta.x, 330f), 0.5f).SetEase(Ease.InOutQuad);
	}

	public void Close()
	{
		MenuTrf.DOSizeDelta(new Vector2(MenuTrf.sizeDelta.x, 150f), 0.5f).SetEase(Ease.InOutQuad);
	}



}
