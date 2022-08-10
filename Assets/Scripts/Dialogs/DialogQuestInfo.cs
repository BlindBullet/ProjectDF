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
	public TextMeshProUGUI DispatchBtnText;
	public Transform HeroListTrf;
	public List<HeroData> DispatchHeroes = new List<HeroData>();
	public List<HeroIcon> HeroIcons = new List<HeroIcon>();
	bool fillCondition = false;
	QuestChart chart = null;
	QuestData data = null;

	public void OpenDialog(QuestData data)
	{
		this.data = data;
		this.chart = CsvData.Ins.QuestChart[data.Id];
		Title.text = LanguageManager.Ins.SetString(chart.Name);
		RewardIcon.SetIcon(chart);
		DispatchBtnText.text = LanguageManager.Ins.SetString("Dispatch");

		SetEmptyIcons();
		SetConditions();
		SetHeroes();

		DispatchBtn.onClick.RemoveAllListeners();
		DispatchBtn.onClick.AddListener(() => 
		{
			SoundManager.Ins.PlaySFX("se_button_2");
			StartCoroutine(RunDispatch());
		});

		fillCondition = false;
		QuestInfo = this;
		Show(true);
	}

	void SetEmptyIcons()
	{
		for(int i = 0; i < chart.NeedAttr.Length; i++)
		{
			EmptyIcons[i].gameObject.SetActive(true);
			EmptyIcons[i].SetIcon();
		}
	}

	void SetConditions()
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
		CheckCondition();
	}

	public void RemoveDispatchHero(HeroData data)
	{
		DispatchHeroes.Remove(data);
		CheckCondition();
	}

	void CheckCondition()
	{		
		bool[] fillAttrCon = new bool[chart.NeedAttr.Length];
		int fillGradeCount = 0;
		
		for(int i = 0; i < chart.NeedAttr.Length; i++)
		{
			AttrIcons[i].Check(false);
			fillAttrCon[i] = false;
		}
		
		for (int i = 0; i < DispatchHeroes.Count; i++)
		{	
			if (DispatchHeroes[i].Grade >= chart.NeedGrade)
			{				
				fillGradeCount++;
			}

			List<HeroChart> heroCharts = CsvData.Ins.HeroChart[DispatchHeroes[i].Id];
			HeroChart heroChart = null;
			for(int k = 0; k < heroCharts.Count; k++)
			{
				if(heroCharts[k].Grade == DispatchHeroes[i].Grade)
				{
					heroChart = heroCharts[k];
				}
			}	
			
			for (int k = 0; k < chart.NeedAttr.Length; k++)
			{
				if(chart.NeedAttr[k] == heroChart.Attr && !fillAttrCon[k])
				{
					fillAttrCon[k] = true;
					AttrIcons[k].Check(true);
					break;
				}
			}
		}

		GradeIcon.SetCountText(fillGradeCount, chart.NeedGradeCount);

		fillCondition = true;

		if (fillGradeCount < chart.NeedGradeCount)
		{
			fillCondition = false;
		}

		for (int i = 0; i < fillAttrCon.Length; i++)
		{
			if (fillAttrCon[i] == false)
			{
				fillCondition = false;
				break;
			}
		}
	}

	IEnumerator RunDispatch()
	{
		if (fillCondition)
		{			
			yield return StartCoroutine(TimeManager.Ins.GetTime());

			data.Dispatch(TimeManager.Ins.ReceivedTime, DispatchHeroes);
			DialogQuest._Dialog.SetQuests();
			StageManager.Ins.PlayerData.Save();
			CloseDialog();
		}
		else
		{
			Debug.Log("조건을 충족하지 못함.");
		}
	}

	private void OnDisable()
	{
		QuestInfo = null;
	}

}
