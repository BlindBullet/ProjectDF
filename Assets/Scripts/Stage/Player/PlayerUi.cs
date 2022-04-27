using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
	public Button HeroBtn;
	public Button RelicBtn;
	public Button AscensionBtn;
	public GameObject AscensionNotify;
	public Button QuestBtn;
	public GameObject QuestNotify;

	private void Start()
	{
		HeroBtn.onClick.RemoveAllListeners();
		HeroBtn.onClick.AddListener(() => DialogManager.Ins.OpenHero());

		RelicBtn.onClick.RemoveAllListeners();
		RelicBtn.onClick.AddListener(() => DialogManager.Ins.OpenRelic());

		AscensionBtn.onClick.RemoveAllListeners();
		AscensionBtn.onClick.AddListener(() => DialogManager.Ins.OpenAscension());

		StageManager.Ins.StageChanged += SetAscensionNotify;
		StageManager.Ins.StageChanged += SetQuestNotify;

		SetAscensionNotify();

		QuestBtn.onClick.RemoveAllListeners();
		QuestBtn.onClick.AddListener(() => DialogManager.Ins.OpenQuest());
	}

	void SetAscensionNotify()
	{
		if(StageManager.Ins.PlayerData.Stage >= ConstantData.PossibleAscensionStage)
		{
			AscensionNotify.SetActive(true);
		}
		else
		{
			AscensionNotify.SetActive(false);
		}
	}

	public void SetQuestNotify()
	{
		StageManager.Ins.PlayerData.CheckAllQuestComplete();

		bool isOn = false;

		for(int i = 0; i < StageManager.Ins.PlayerData.Quests.Count; i++)
		{
			if (StageManager.Ins.PlayerData.Quests[i].IsComplete)
			{
				isOn = true;
				break;
			}	
		}

		if (isOn)
		{
			QuestNotify.SetActive(true);
		}
		else
		{
			QuestNotify.SetActive(false);
		}
	}

}
