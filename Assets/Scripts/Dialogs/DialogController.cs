using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class DialogController : MonoBehaviour
{
	public Button BackPanelBtn;	
	public DialogTween OpenTween;
	public DialogTween CloseTween;
	public Button CloseBtn;
	public TextMeshProUGUI CloseBtnText;
	[HideInInspector]
	public bool EnabledBackkey;	
	float _time = 0f;

	public enum DialogTween
	{
		None,

	}

	public void Show(bool enabledBackkey, bool stopTime = false)
	{		
		BattleInputManager.Ins.isPause = true;

		if (stopTime)					
			Time.timeScale = 0f;		

		Open();
		EnabledBackkey = enabledBackkey;
		BackkeyManager.Ins.AddDialog(this);
		gameObject.transform.SetAsLastSibling();
		
		if (EnabledBackkey)
		{
			BackPanelBtn.onClick.RemoveAllListeners();
			BackPanelBtn.onClick.AddListener(() => 
			{
				SoundManager.Ins.PlaySFX("se_button_2");

				if (stopTime)
					Time.timeScale = _time;

				BackkeyManager.Ins.UseBackkey(); 
			});
		}

		if(CloseBtn != null)
		{
			CloseBtn.onClick.RemoveAllListeners();
			CloseBtn.onClick.AddListener(() => 
			{
				SoundManager.Ins.PlaySFX("se_button_2");
				SetCloseBtn();
				CloseDialog(stopTime);
			});
		}

		if (CloseBtnText != null)
			CloseBtnText.text = LanguageManager.Ins.SetString("Close");
	}

	void Open()
	{
		switch (OpenTween)
		{
			case DialogTween.None:
				break;
		}
	}

	void Close()
	{
		switch (CloseTween)
		{
			case DialogTween.None:
				break;
		}
	}

	public virtual void SetCloseBtn()
	{

	}

	public virtual void CloseDialog(bool stopTime = false)
	{
		if (stopTime)
		{
			Time.timeScale = StageManager.Ins.PlayerStat.GameSpd;
		}
		
		Close();
		BackkeyManager.Ins.RemoveDialog(this);	
		
		if(BackkeyManager.Ins.OpenedDialogList.Count <= 0)
		{			
			BattleInputManager.Ins.isPause = false;
		}	

		Destroy(this.gameObject);
	}

}
