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
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogCautionBar") as GameObject, DialogTrf);
		dialog.GetComponent<DialogCautionBar>().SetDialog(cautionText);
	}

	public void OpenConfirmAndCancel(string title, string desc, Action action)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogConfirmAndCancel") as GameObject, DialogTrf);
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

	public void OpenCastle()
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogCastle") as GameObject, DialogTrf);
		dialog.GetComponent<DialogCastle>().OpenDialog();
	}

	public void OpenAscension()
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogAscension") as GameObject, DialogTrf);
		dialog.GetComponent<DialogAscension>().OpenDialog();
	}

	public void OpenQuest()
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogQuest") as GameObject, DialogTrf);
		dialog.GetComponent<DialogQuest>().OpenDialog();
	}

	public void OpenQuestInfo(QuestData data)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogQuestInfo") as GameObject, DialogTrf);
		dialog.GetComponent<DialogQuestInfo>().OpenDialog(data);
	}

	public void OpenReceiveReward(RewardType type, double value)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogReceiveReward") as GameObject, DialogTrf);
		dialog.GetComponent<DialogReceiveReward>().OpenDialog(type, value);
	}

	public void OpenAdReward(SuppliesChart chart)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogAdReward") as GameObject, DialogTrf);
		dialog.GetComponent<DialogAdReward>().OpenDialog(chart);
	}

	public void OpenAdReward(QuestChart chart)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogAdReward") as GameObject, DialogTrf);
		dialog.GetComponent<DialogAdReward>().OpenDialog(chart);
	}

	public void OpenSetting()
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogSetting") as GameObject, DialogTrf);
		dialog.GetComponent<DialogSetting>().OpenDialog();
	}

	public void OpenOfflineReward(bool isAdLodead = true)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogOfflineReward") as GameObject, DialogTrf);
		dialog.GetComponent<DialogOfflineReward>().OpenDialog(isAdLodead);
	}

	public void OpenSlotPowerUp(SlotData data)
	{
		GameObject dialog = Instantiate(Resources.Load("Prefabs/Dialogs/DialogSlotPowerUp") as GameObject, DialogTrf);
		dialog.GetComponent<DialogSlotPowerUp>().OpenDialog(data);
	}
}
