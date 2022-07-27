using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerMenu : MonoBehaviour
{	
	public Button SettingBtn;
	public TextMeshProUGUI SettingBtnText;
	public Button RankBtn;
	public TextMeshProUGUI RankBtnText;	
	public Button RemoveAdBtn;
	public TextMeshProUGUI RemoveAdText;	
	public Button AttendanceBtn;
	public TextMeshProUGUI AttendanceBtnText;
	public RectTransform[] MenusTrf;	
	Coroutine cOpenSeq = null;
	Coroutine cCloseSeq = null;

	private void Start()
	{
		SettingBtn.onClick.RemoveAllListeners();
		SettingBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); DialogManager.Ins.OpenSetting(); });
		SettingBtnText.text = LanguageManager.Ins.SetString("Setting");

		RankBtn.onClick.RemoveAllListeners();
		RankBtn.onClick.AddListener(() => { if (AdmobManager.Ins.isReal) GPGSBinder.Inst.ShowAllLeaderboardUI(); });
		RankBtnText.text = LanguageManager.Ins.SetString("Rank");

		RemoveAdBtn.onClick.RemoveAllListeners();
		RemoveAdBtn.onClick.AddListener(() => { IAPManager.Ins.Purchase("remove_ad"); });
		RemoveAdText.text = LanguageManager.Ins.SetString("Remove_Ad");

		AttendanceBtn.onClick.RemoveAllListeners();
		AttendanceBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); DialogManager.Ins.OpenAttendance(); });
		AttendanceBtnText.text = LanguageManager.Ins.SetString("Attendance");
	}

	public void Open()
	{
		if(cOpenSeq == null)
		{
			if (cCloseSeq != null)
				StopCoroutine(cCloseSeq);

			cOpenSeq = StartCoroutine(OpenSeq());
			cCloseSeq = null;
		}	
	}

	IEnumerator OpenSeq()
	{
		DOTween.Kill(this.gameObject);

		for (int i = 0; i < MenusTrf.Length; i++)
		{
			MenusTrf[i].anchoredPosition = new Vector2(0, 0);
		}

		for (int i = 0; i < MenusTrf.Length; i++)
		{
			MenusTrf[i].DOAnchorPosY(200 * (i + 1), 0.5f).SetEase(Ease.InOutQuad);
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void Close()
	{
		if (cCloseSeq == null)
		{
			if (cOpenSeq != null)
				StopCoroutine(cOpenSeq);

			cCloseSeq = StartCoroutine(CloseSeq());
			cOpenSeq = null;
		}
	}

	IEnumerator CloseSeq()
	{
		DOTween.Kill(this.gameObject);

		for (int i = 0; i < MenusTrf.Length; i++)
		{			
			MenusTrf[i].DOAnchorPosY(0f, 0.5f).SetEase(Ease.InOutQuad);
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(1f);

		for (int i = 0; i < MenusTrf.Length; i++)
		{
			MenusTrf[i].anchoredPosition = new Vector2(0, -250f);
		}
	}

}
