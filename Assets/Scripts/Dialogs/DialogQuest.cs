using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogQuest : DialogController
{
	public static DialogQuest Quest = null;

	public TextMeshProUGUI Title;
	public TextMeshProUGUI Desc;
	public QuestBar[] QuestBars;
	public Button ResetBtn;

	public void OpenDialog()
	{
		StageManager.Ins.PlayerData.CheckAllQuestComplete();
		Title.text = LanguageManager.Ins.SetString("popup_quest_title");
		Desc.text = LanguageManager.Ins.SetString("popup_quest_desc");

		SetQuests();

		ResetBtn.onClick.RemoveAllListeners();
		ResetBtn.onClick.AddListener(() => ResetQuest());

		Quest = this;

		Show(true, true);
	}

	public void SetQuests()
	{
		for(int i = 0; i < QuestBars.Length; i++)
		{
			QuestBars[i].SetBar(StageManager.Ins.PlayerData.Quests[i]);
		}
	}

	void ResetQuest()
	{
		QuestManager.Ins.ResetQuest();
		SetQuests();
	}

	private void OnDisable()
	{	
		StageManager.Ins.PlayerUi.SetQuestNotify();
		Quest = null;
	}

}
