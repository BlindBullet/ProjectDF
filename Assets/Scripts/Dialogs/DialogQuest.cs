using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogQuest : DialogController
{
	public static DialogQuest _Dialog = null;

	public TextMeshProUGUI Title;
	public TextMeshProUGUI Desc;
	public TextMeshProUGUI QuestPlayerLv;
	public Image QuestPlayerLvFill;
	public TextMeshProUGUI QuestPlayerLvCount;
	public QuestBar[] QuestBars;
	public Button ResetBtn;

	public void OpenDialog()
	{
		StageManager.Ins.PlayerData.CheckAllQuestComplete();
		Title.text = LanguageManager.Ins.SetString("popup_quest_title");
		Desc.text = LanguageManager.Ins.SetString("popup_quest_desc");

		SetQuests();
		SetPlayerLv();

		Show(false, true);

		ResetBtn.onClick.RemoveAllListeners();
		ResetBtn.onClick.AddListener(() =>
		{
			AdmobManager.Ins.ShowQuestRefreshAd();
		});

		_Dialog = this;
	}

	public void SetQuests()
	{
		for(int i = 0; i < QuestBars.Length; i++)
		{
			QuestBars[i].SetBar(StageManager.Ins.PlayerData.Quests[i]);
		}
	}

	public void SetPlayerLv()
	{
		int lv = QuestManager.Ins.GetQuestLevel();
		QuestPlayerLv.text = lv.ToString();
		QuestPlayerLvFill.fillAmount = (float)StageManager.Ins.PlayerData.ClearQuestCount / (float)ConstantData.QuestLvPerClearCount[lv - 1];
		QuestPlayerLvCount.text = StageManager.Ins.PlayerData.ClearQuestCount.ToString() + "/" + ConstantData.QuestLvPerClearCount[lv - 1];
	}

	public IEnumerator ResetQuest()
	{
		yield return null;
		QuestManager.Ins.ResetQuest();
		SetQuests();
	}

	private void OnDisable()
	{	
		StageManager.Ins.PlayerUi.SetQuestNotify();
		_Dialog = null;
	}

}
