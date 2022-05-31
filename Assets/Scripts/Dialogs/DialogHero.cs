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
	public List<HeroIcon> HeroIcons = new List<HeroIcon>();
	public List<DeploySlot> DeploySlots = new List<DeploySlot>();
	
	public void OpenDialog()
	{
		TitleText.text = LanguageManager.Ins.SetString("title_dialog_hero");
		SetHeroes();
		SetDeploySlots();
		_DialogHero = this;		
		Show(true, true);
	}

	public void SetHeroes()
	{
		for(int i = 0; i < HeroIcons.Count; i++)
		{
			Destroy(HeroIcons[i].gameObject);
		}

		HeroIcons.Clear();

		List<HeroData> heroes = StageManager.Ins.PlayerData.Heroes;

		for (int i = 0; i < heroes.Count; i++)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/HeroIcon") as GameObject, HeroIconsTrf);
			HeroIcon icon = obj.GetComponent<HeroIcon>();
			icon.Setup(heroes[i], OpenHeroInfo);
			HeroIcons.Add(icon);
		}
	}

	public void SetDeploySlots()
	{
		for(int i = 0; i < DeploySlots.Count; i++)
		{
			HeroData heroData = null;

			for(int k = 0; k < StageManager.Ins.PlayerData.Heroes.Count; k++)
			{
				if (StageManager.Ins.PlayerData.Heroes[k].SlotNo == i + 1)
					heroData = StageManager.Ins.PlayerData.Heroes[k];
			}

			DeploySlots[i].SetSlot(heroData, i + 1);
		}
	}

	public void SetDeployState(HeroData data)
	{
		for(int i = 0; i < DeploySlots.Count; i++)
		{
			DeploySlots[i].SetDeployState(data);
		}

		for(int i = 0; i < HeroIcons.Count; i++)
		{
			if (HeroIcons[i].Data.Id == data.Id)
			{
				HeroIcons[i].ShowSelectedFrame();				
				HeroIcons[i].DiasbleBtns(true);
			}
			else
			{
				HeroIcons[i].DiasbleBtns();
			}
		}
	}

	public void EndDeployState()
	{
		for (int i = 0; i < DeploySlots.Count; i++)
		{
			DeploySlots[i].EndDeployState();
		}

		for (int i = 0; i < HeroIcons.Count; i++)
		{	
			HeroIcons[i].CloseSelectedFrame();
			HeroIcons[i].EnableBtns();
		}
	}

	void OpenHeroInfo(HeroData data)
	{   
		DialogManager.Ins.OpenHeroInfo(data);
	}

	private void OnDisable()
	{	
		_DialogHero = null;
	}
}
