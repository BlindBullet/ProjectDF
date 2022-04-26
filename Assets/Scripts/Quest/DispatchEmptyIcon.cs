using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DispatchEmptyIcon : MonoBehaviour
{
	public Button Btn;
	public HeroData Data;
	public bool isOpen = false;
	HeroIcon icon;

	public void SetIcon()
	{
		isOpen = true;
		Data = null;
		icon = null;
	}

	public void SetHeroIcon(HeroData data)
	{
		Data = data;
		GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/HeroIcon") as GameObject, this.transform);
		icon = obj.GetComponent<HeroIcon>();
		icon.Setup(data, icon, Undispatch);
		DialogQuestInfo.QuestInfo.AddDispatchHero(data);		
	}

	void Undispatch(HeroIcon icon, HeroData data)
	{
		for(int i = 0; i < DialogQuestInfo.QuestInfo.HeroIcons.Count; i++)
		{
			if(DialogQuestInfo.QuestInfo.HeroIcons[i].Data.Id == this.Data.Id)
			{
				DialogQuestInfo.QuestInfo.HeroIcons[i].IconGreyScale(false);
				DialogQuestInfo.QuestInfo.HeroIcons[i].Dispatched = false;
			}
		}

		Data = null;
		Destroy(icon.gameObject);
		DialogQuestInfo.QuestInfo.RemoveDispatchHero(data);
	}

	public void Undispatch(HeroData data)
	{
		Data = null;
		Destroy(icon.gameObject);
		DialogQuestInfo.QuestInfo.RemoveDispatchHero(data);
	}

}
