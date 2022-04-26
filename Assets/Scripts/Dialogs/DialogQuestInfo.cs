using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogQuestInfo : DialogController
{
	public static DialogQuestInfo QuestInfo = null;

	public TextMeshProUGUI Title;
	public RewardIcon RewardIcon;
	public DispatchEmptyIcon[] EmptyIcons;
	public QuestInfoGradeIcon GradeIcon;	
	public QuestInfoAttrIcon[] AttrIcons;
	public Button DispatchBtn;
	public Transform HeroListTrf;
	public List<HeroData> DispatchHeroes = new List<HeroData>();
	public List<HeroIcon> HeroIcons = new List<HeroIcon>();

	public void OpenDialog(QuestData data)
	{
		QuestChart chart = CsvData.Ins.QuestChart[data.Id];
		Title.text = LanguageManager.Ins.SetString(chart.Name);
		RewardIcon.SetIcon(chart);

		SetEmptyIcons(chart);
		SetConditions(chart);
		SetHeroes();

		QuestInfo = this;
		Show(false);
	}

	void SetEmptyIcons(QuestChart chart)
	{
		for(int i = 0; i < chart.NeedAttr.Length; i++)
		{
			EmptyIcons[i].gameObject.SetActive(true);
			EmptyIcons[i].SetIcon();
		}
	}

	void SetConditions(QuestChart chart)
	{
		if(chart.NeedGradeCount > 0)
		{
			GradeIcon.gameObject.SetActive(true);
			GradeIcon.Init(chart);
		}
			

		for(int i = 0; i < chart.NeedAttr.Length; i++)
		{
			AttrIcons[i].gameObject.SetActive(true);
			AttrIcons[i].SetIcon(chart.NeedAttr[i]);
		}
	}

	void SetHeroes()
	{
		List<QuestData> questDatas = StageManager.Ins.PlayerData.Quests;
		List<HeroData> datas = StageManager.Ins.PlayerData.Heroes;
		List<HeroData> result = new List<HeroData>();

		for (int i = 0; i < datas.Count; i++)
		{
			bool alreadyDispatching = false;

			if (!datas[i].IsOwn)
				continue;

			for(int k = 0; k < questDatas.Count; k++)
			{
				for(int j = 0; j < questDatas[k].DispatchHeroes.Count; j++)
				{
					if (questDatas[k].DispatchHeroes[j] == datas[i].Id)
					{
						alreadyDispatching = true;
					}
				}
			}

			if (!alreadyDispatching)
				result.Add(datas[i]);
		}

		for(int i = 0; i < result.Count; i++)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/HeroIcon") as GameObject, HeroListTrf);
			HeroIcon icon = obj.GetComponent<HeroIcon>();
			icon.Setup(result[i], icon, DispatchHero);
			HeroIcons.Add(icon);
		}

	}

	void DispatchHero(HeroIcon icon, HeroData data)
	{
		if (icon.Dispatched)
		{
			for(int i = 0; i < EmptyIcons.Length; i++)
			{
				if (EmptyIcons[i].isOpen && EmptyIcons[i].Data == data)
				{
					EmptyIcons[i].Undispatch(data);
					break;
				}	
			}

			icon.IconGreyScale(false);
			icon.Dispatched = false;
		}
		else
		{
			for (int i = 0; i < EmptyIcons.Length; i++)
			{
				if (EmptyIcons[i].isOpen && EmptyIcons[i].Data == null)
				{
					EmptyIcons[i].SetHeroIcon(data);
					break;
				}
			}

			icon.IconGreyScale(true);
			icon.Dispatched = true;			
		}
	}

	public void AddDispatchHero(HeroData data)
	{
		DispatchHeroes.Add(data);

	}

	public void RemoveDispatchHero(HeroData data)
	{
		DispatchHeroes.Remove(data);
	}


	void RunDispatch()
	{

	}

	private void OnDisable()
	{
		QuestInfo = null;
	}

}
