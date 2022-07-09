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
	public Button RemoveAdBtn;
	public TextMeshProUGUI RemoveAdText;
	public Button AttendanceBtn;
	public TextMeshProUGUI AttendanceBtnText;

	private void Start()
	{		
		SettingBtn.onClick.RemoveAllListeners();
		SettingBtn.onClick.AddListener(() => { DialogManager.Ins.OpenSetting(); });
		SettingBtnText.text = LanguageManager.Ins.SetString("Setting");

		RankBtn.onClick.RemoveAllListeners();
		RankBtn.onClick.AddListener(() => { GPGSBinder.Inst.ShowAllLeaderboardUI(); });
		RankBtnText.text = LanguageManager.Ins.SetString("Rank");

		RemoveAdBtn.onClick.RemoveAllListeners();
		RemoveAdBtn.onClick.AddListener(() => { IAPManager.Ins.Purchase("remove_ad"); });
		RemoveAdText.text = LanguageManager.Ins.SetString("Remove_Ad");

		AttendanceBtn.onClick.RemoveAllListeners();
		AttendanceBtn.onClick.AddListener(() => { DialogManager.Ins.OpenAttendance(); });
		AttendanceBtnText.text = LanguageManager.Ins.SetString("Attendance");
	}

	public void Open()
	{
		//870f
		//690f
		MenuTrf.DOSizeDelta(new Vector2(MenuTrf.sizeDelta.x, 870f), 0.5f).SetEase(Ease.InOutQuad);
	}

	public void Close()
	{
		MenuTrf.DOSizeDelta(new Vector2(MenuTrf.sizeDelta.x, 150f), 0.5f).SetEase(Ease.InOutQuad);
	}



}
