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

	public void OpenHeroInfo(HeroData data)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogHeroInfo") as GameObject, DialogTrf);
		dialog.GetComponent<DialogHeroInfo>().OpenDialog(data);
	}

	public void OpenBossWarning()
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogBossWarning") as GameObject, DialogTrf);
		dialog.GetComponent<DialogBossWarning>().OpenDialog();
	}

	public void OpenRelic()
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogRelic") as GameObject, DialogTrf);
		dialog.GetComponent<DialogRelic>().OpenDialog();
	}

	public void OpenRelicInfo(RelicData data)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogRelicInfo") as GameObject, DialogTrf);
		dialog.GetComponent<DialogRelicInfo>().OpenDialog(data);
	}

	public void OpenAscension()
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogAscension") as GameObject, DialogTrf);
		dialog.GetComponent<DialogAscension>().OpenDialog();
	}



}
