using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogRelic : DialogController
{
	public static DialogRelic _DialogRelic = null;

	public TextMeshProUGUI TitleText;
	public Transform ListTrf;
	List<GameObject> Relics = new List<GameObject>();

	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("Relic");
		SetRelics();
		_DialogRelic = this;
		Show(true);
	}

	public void SetRelics()
	{
		for(int i = 0; i < Relics.Count; i++)
		{
			Destroy(Relics[i].gameObject);
		}

		Relics.Clear();

		List<RelicData> relicDatas = StageManager.Ins.PlayerData.Relics;

		for(int i = 0; i < relicDatas.Count; i++)
		{
			GameObject icon = Instantiate(Resources.Load("Prefabs/Icons/RelicIcon") as GameObject, ListTrf);
			icon.GetComponent<RelicIcon>().SetIcon(relicDatas[i], OpenRelicInfo);
			Relics.Add(icon);
		}
	}

	void OpenRelicInfo(RelicData data)
	{
		DialogManager.Ins.OpenRelicInfo(data);
	}

	private void OnDisable()
	{
		_DialogRelic = null;
	}
}
