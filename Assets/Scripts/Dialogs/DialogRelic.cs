using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogRelic : DialogController
{
	public static DialogRelic _Dialog = null;

	public TextMeshProUGUI TitleText;
	public Transform ListTrf;
	List<GameObject> Relics = new List<GameObject>();

	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("Relic");
		SetRelics();
		_Dialog = this;
		Show(true, true);
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
			GameObject bar = Instantiate(Resources.Load("Prefabs/Bars/RelicBar") as GameObject, ListTrf);
			bar.GetComponent<RelicBar>().SetBar(relicDatas[i]);
			Relics.Add(bar);
		}
	}

	public void SetBars()
	{
		for(int i = 0; i < Relics.Count; i++)
		{
			Relics[i].GetComponent<RelicBar>().SetInfo();
		}
	}

	private void OnDisable()
	{
		_Dialog = null;
	}

}
