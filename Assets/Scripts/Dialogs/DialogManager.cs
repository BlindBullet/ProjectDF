using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogManager : SingletonObject<DialogManager> {

	Transform DialogTrf;
	public const string Path = "Dialogs/";

	public void SetDialogTransform()
	{
		DialogTrf = GameObject.Find("Ui Canvas").transform.Find("Dialog").transform;
	}

	public void OpenCautionBar(string cautionText)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/Dialog Caution Bar") as GameObject, DialogTrf);
		dialog.GetComponent<DialogCautionBar>().SetDialog(cautionText);
	}

	public void OpenConfirmAndCancel(string title, string desc, Action action)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/Dialog ConfirmAndCancel") as GameObject, DialogTrf);
		dialog.GetComponent<DialogConfirmAndCancel>().SetDialog(title, desc, action);
	}

	public void OpenHero()
    {
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogHero") as GameObject, DialogTrf);
		dialog.GetComponent<DialogHero>().OpenDialog();
	}

}
