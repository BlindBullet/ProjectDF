using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DialogController : MonoBehaviour
{
	public Button BackPanelBtn;	
	public DialogTween OpenTween;
	public DialogTween CloseTween;
	public Button CloseBtn;
	[HideInInspector]
	public bool EnabledBackkey;

	public enum DialogTween
    {
		None,

    }

	public void Show(bool enabledBackkey)
	{
		Open();
		EnabledBackkey = enabledBackkey;
		BackkeyManager.Ins.AddDialog(this);
		gameObject.transform.SetAsLastSibling();
		
		if (EnabledBackkey)
		{
			BackPanelBtn.onClick.RemoveAllListeners();
			BackPanelBtn.onClick.AddListener(() => { BackkeyManager.Ins.UseBackkey(); });
		}

		if(CloseBtn != null)
        {
			CloseBtn.onClick.RemoveAllListeners();
			CloseBtn.onClick.AddListener(() => CloseDialog());
		}
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

	public virtual void CloseDialog()
	{
		Close();
		BackkeyManager.Ins.RemoveDialog(this);
		Destroy(this.gameObject);
	}

}
