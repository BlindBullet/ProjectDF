using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUi : MonoBehaviour
{
	public Button HeroBtn;
	public TextMeshProUGUI HeroBtnText;
	public Button RelicBtn;
	public TextMeshProUGUI RelicBtnText;
	public Button AscensionBtn;
	public TextMeshProUGUI AscensionBtnText;
	public GameObject AscensionNotify;
	public Button QuestBtn;
	public TextMeshProUGUI QuestBtnText;
	public GameObject QuestNotify;
	public Button SettingBtn;
	public TextMeshProUGUI SettingBtnText;

	private void Start()
	{
		HeroBtnText.text = LanguageManager.Ins.SetString("Hero");
		HeroBtn.onClick.RemoveAllListeners();
		HeroBtn.onClick.AddListener(() => DialogManager.Ins.OpenHero());

		RelicBtnText.text = LanguageManager.Ins.SetString("Relic");
		RelicBtn.onClick.RemoveAllListeners();
		RelicBtn.onClick.AddListener(() => DialogManager.Ins.OpenRelic());

		AscensionBtnText.text = LanguageManager.Ins.SetString("Ascension");
		AscensionBtn.onClick.RemoveAllListeners();
		AscensionBtn.onClick.AddListener(() => DialogManager.Ins.OpenAscension());

		StageManager.Ins.StageChanged += SetAscensionNotify;
		StageManager.Ins.StageChanged += SetQuestNotify;

		SetAscensionNotify();

		QuestBtnText.text = LanguageManager.Ins.SetString("Quest");
		QuestBtn.onClick.RemoveAllListeners();
		QuestBtn.onClick.AddListener(() => DialogManager.Ins.OpenQuest());

		SettingBtnText.text = LanguageManager.Ins.SetString("Setting");
		SettingBtn.onClick.RemoveAllListeners();
		SettingBtn.onClick.AddListener(() => DialogManager.Ins.OpenSetting());
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
