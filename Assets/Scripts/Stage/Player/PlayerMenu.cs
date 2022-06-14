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
	public Button RankBtn;
	public TextMeshProUGUI RankBtnText;

	private void Start()
	{		
		SettingBtn.onClick.RemoveAllListeners();
		SettingBtn.onClick.AddListener(() => { DialogManager.Ins.OpenSetting(); });
		SettingBtnText.text = LanguageManager.Ins.SetString("Setting");

		RankBtn.onClick.RemoveAllListeners();
		RankBtn.onClick.AddListener(() => { GPGSBinder.Inst.ShowAllLeaderboardUI(); });
		RankBtnText.text = LanguageManager.Ins.SetString("Rank");
	}

	public void Open()
	{
		MenuTrf.DOSizeDelta(new Vector2(MenuTrf.sizeDelta.x, 510f), 0.5f).SetEase(Ease.InOutQuad);
	}

	public void Close()
	{
		MenuTrf.DOSizeDelta(new Vector2(MenuTrf.sizeDelta.x, 150f), 0.5f).SetEase(Ease.InOutQuad);
	}



}
