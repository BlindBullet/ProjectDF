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
	public GameObject QuestLockObj;
	public TextMeshProUGUI QuestBtnText;
	public GameObject QuestNotify;
	public Button CastleBtn;
	public TextMeshProUGUI CastleBtnText;
	public Button DungeonBtn;
	public TextMeshProUGUI DungeonBtnText;	
	public Button MenuBtn;
	public TextMeshProUGUI MenuBtnText;
	public PlayerMenu Menu;	

	bool isOpenMenu = false;

	private void Start()
	{
		HeroBtnText.text = LanguageManager.Ins.SetString("Hero");
		HeroBtn.onClick.RemoveAllListeners();
		HeroBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); DialogManager.Ins.OpenHero(); });

		RelicBtnText.text = LanguageManager.Ins.SetString("Relic");
		RelicBtn.onClick.RemoveAllListeners();
		RelicBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); DialogManager.Ins.OpenRelic(); });

		CastleBtnText.text = LanguageManager.Ins.SetString("Castle");
		CastleBtn.onClick.RemoveAllListeners();
		CastleBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); DialogManager.Ins.OpenCastle(); });

		AscensionBtnText.text = LanguageManager.Ins.SetString("Ascension");
		AscensionBtn.onClick.RemoveAllListeners();
		AscensionBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); DialogManager.Ins.OpenAscension(); });

		StageManager.Ins.StageChanged += SetAscensionNotify;
		StageManager.Ins.StageChanged += SetQuestNotify;

		SetAscensionNotify();
		SetQuestBtn();

		DungeonBtnText.text = LanguageManager.Ins.SetString("Dungeon");
		DungeonBtn.onClick.RemoveAllListeners();
		DungeonBtn.onClick.AddListener(() => { SoundManager.Ins.PlaySFX("se_button_2"); DialogManager.Ins.OpenCautionBar("notice_dungeon_update"); });

		MenuBtnText.text = LanguageManager.Ins.SetString("Menu");
		MenuBtn.onClick.RemoveAllListeners();
		MenuBtn.onClick.AddListener(() => 
		{ 
			SoundManager.Ins.PlaySFX("se_button_2");
			if (!isOpenMenu)
			{
				OpenMenu();
			}
			else
			{
				CloseMenu();
			}
		});
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

	public void SetQuestBtn()
	{
		QuestBtnText.text = LanguageManager.Ins.SetString("Quest");
		QuestBtn.onClick.RemoveAllListeners();

		if (StageManager.Ins.PlayerData.AscensionCount >= ConstantData.OpenQuestAscensionCount)
		{
			QuestLockObj.SetActive(false);			
			QuestBtn.onClick.AddListener(() =>
			{
				SoundManager.Ins.PlaySFX("se_button_2");
				DialogManager.Ins.OpenQuest();				
			});
		}
		else
		{
			QuestLockObj.SetActive(true);
			QuestBtn.onClick.AddListener(() =>
			{
				SoundManager.Ins.PlaySFX("se_button_2");
				DialogManager.Ins.OpenCautionBar("desc_locked_quest");
			});
		}			
	}

	void OpenMenu()
	{
		isOpenMenu = true;
		Menu.Open();
	}

	void CloseMenu()
	{
		isOpenMenu = false;		
		Menu.Close();
	}

}
