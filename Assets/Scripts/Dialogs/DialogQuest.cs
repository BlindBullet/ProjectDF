using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
	public TextMeshProUGUI ResetBtnText;
	public TextMeshProUGUI ResetBtnTimeText;
	bool canReset = false;

	public void OpenDialog()
	{		
		StageManager.Ins.PlayerData.CheckAllQuestComplete();
		Title.text = LanguageManager.Ins.SetString("popup_quest_title");
		Desc.text = LanguageManager.Ins.SetString("popup_quest_desc");
		ResetBtnText.text = LanguageManager.Ins.SetString("Refresh");

		SetQuests();
		SetPlayerLv();

		ResetBtn.onClick.RemoveAllListeners();
		ResetBtn.onClick.AddListener(() =>
		{
			AdmobManager.Ins.ShowQuestRefreshAd();
		});

		SetResetBtn();		

		Show(false, true);
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

	void SetResetBtn()
	{
		if (CheckCanReset())
		{
			ResetBtnTimeText.gameObject.SetActive(false);
			ResetBtn.enabled = true;
		}
		else
		{
			ResetBtnTimeText.gameObject.SetActive(true);
			ResetBtn.enabled = false;
			StartCoroutine(ResetBtnSequence());
		}
		
	}

	public IEnumerator ResetQuest()
	{
		yield return null;
		StageManager.Ins.PlayerData.SetQuestResetTime();
		SetResetBtn();
		QuestManager.Ins.ResetQuest();
		SetQuests();
		Time.timeScale = 0f;
	}

	bool CheckCanReset()
	{
		DateTime dateTime = StageManager.Ins.PlayerData.QuestResetStartTime;
		TimeSpan span = DateTime.UtcNow - dateTime;
		
		if(span.TotalSeconds >= ConstantData.QuestResetPossibleSec)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	IEnumerator ResetBtnSequence()
	{
		while (true)
		{
			if (!CheckCanReset())
			{
				DateTime dateTime = StageManager.Ins.PlayerData.QuestResetStartTime;
				TimeSpan timeSpan = DateTime.UtcNow - dateTime;
				int time = (ConstantData.QuestResetPossibleSec - (int)timeSpan.TotalSeconds);
				string timeStr = time < 10 ? "0" + time.ToString() : time.ToString();
				ResetBtnTimeText.text = "00:" + timeStr;
			}
			else
			{
				SetResetBtn();
				yield break;
			}

			yield return null;
		}
	}

	private void OnDisable()
	{	
		StageManager.Ins.PlayerUi.SetQuestNotify();
		_Dialog = null;
	}

}
