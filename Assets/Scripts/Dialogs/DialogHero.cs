using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogHero : DialogController
{
	public static DialogHero _DialogHero = null;

	public TextMeshProUGUI TitleText;
	public Transform HeroIconsTrf;
	public List<GameObject> HeroListObjs = new List<GameObject>();

	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("title_dialog_hero");
		SetHeroes();
		_DialogHero = this;
		Time.timeScale = 0f;
		Show(true);
	}

	public void SetHeroes()
	{
		for(int i = 0; i < HeroListObjs.Count; i++)
		{
			Destroy(HeroListObjs[i]);
		}

		List<HeroData> heroes = StageManager.Ins.PlayerData.Heroes;

		for (int i = 0; i < heroes.Count; i++)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/HeroIcon") as GameObject, HeroIconsTrf);
			obj.GetComponent<HeroIcon>().Setup(heroes[i], OpenHeroInfo);
			HeroListObjs.Add(obj);
		}
	}

	void OpenHeroInfo(HeroData data)
	{   
		DialogManager.Ins.OpenHeroInfo(data);
	}

	private void OnDisable()
	{
		Time.timeScale = 1f;
		_DialogHero = null;
	}
}
