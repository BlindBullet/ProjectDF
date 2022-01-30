using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
	public Button BackPanelBtn;
	[HideInInspector]
	public bool EnabledBackkey;

	public void Show(bool enabledBackkey)
	{
		EnabledBackkey = enabledBackkey;
		BackkeyManager.Ins.AddDialog(this);
		gameObject.transform.SetAsLastSibling();
		
		if (EnabledBackkey)
		{
			BackPanelBtn.onClick.RemoveAllListeners();
			BackPanelBtn.onClick.AddListener(() => { BackkeyManager.Ins.UseBackkey(); });
		}
	}

	public virtual void CloseDialog()
	{
		BackkeyManager.Ins.RemoveDialog(this);
		Destroy(this.gameObject);
	}


}
