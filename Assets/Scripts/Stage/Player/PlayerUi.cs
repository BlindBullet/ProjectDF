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



}
