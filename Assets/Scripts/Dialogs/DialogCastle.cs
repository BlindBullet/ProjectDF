using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogCastle : DialogController
{
	public TextMeshProUGUI TitleText;
	public Transform ListTrf;
	List<GameObject> Relics = new List<GameObject>();

	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("Castle");
		SetCastle();	
		Show(true, true);
	}

	public void SetCastle()
	{
		for (int i = 0; i < Relics.Count; i++)
		{
			Destroy(Relics[i].gameObject);
		}

		Relics.Clear();

		List<RelicData> castleData = StageManager.Ins.PlayerData.Castles;

		for (int i = 0; i < castleData.Count; i++)
		{
			GameObject bar = Instantiate(Resources.Load("Prefabs/Bars/RelicBar") as GameObject, ListTrf);
			bar.GetComponent<RelicBar>().SetBar(castleData[i]);
			Relics.Add(bar);
		}
	}

}