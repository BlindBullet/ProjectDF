using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackkeyManager : SingletonObject<BackkeyManager>
{
	public List<DialogController> OpenedDialogList = new List<DialogController>();
	
	public void ClearDialog()
	{
		OpenedDialogList.Clear();
	}

	public void UseBackkey()
	{
		if (OpenedDialogList.Count > 0)
		{
			if (OpenedDialogList[OpenedDialogList.Count - 1].EnabledBackkey)
			{				
				OpenedDialogList[OpenedDialogList.Count - 1].CloseDialog();
			}
			else
			{

			}
		}
		else
		{
			DialogManager.Ins.OpenConfirmAndCancel("QuitGame", "QuitGameDesc", () => 
			{
				NotifyManager.Ins.CallNotify();
				StageManager.Ins.PlayerData.Save();

				//if (!Application.isEditor)
				//	System.Diagnostics.Process.GetCurrentProcess().Kill();
				Application.Quit();
			});
		}
	}

	public void AddDialog(DialogController dialog)
	{		
		OpenedDialogList.Add(dialog);
	}

	public void RemoveDialog(DialogController dialog)
	{
		OpenedDialogList.Remove(dialog);
	}

	private void OnDisable()
	{
		ClearDialog();
	}

}
