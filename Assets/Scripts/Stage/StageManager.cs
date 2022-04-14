using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoSingleton<StageManager>
{
	public PlayerData PlayerData = new PlayerData();
	public PlayerStat PlayerStat = new PlayerStat();
	public TopBar TopBar;
		
	public List<Slot> Slots = new List<Slot>();

	public event UnityAction<double> GoldChanged;
	public event UnityAction<double> GemChanged;
	public event UnityAction<double> MagiciteChanged;

	[HideInInspector]
	public bool LastEnemiesSpawned;
	Coroutine cStageSequence = null;

	private void Start()
	{
		PlayerData = new PlayerData();
		PlayerData.Load();

		PlayerStat = new PlayerStat();
		PlayerStat.Init();
		
		Load();

		PlayerData.IncAppCount();
	}

	void Load()
	{
		TopBar.Setup();
		SetSlots();
		SetHeroes();
		SEManager.Ins.Apply();
		StartCoroutine(SetStage(PlayerData.Stage));
	}

	void SetHeroes()
	{
		if (PlayerData.PlayAppCount == 1)
		{
			SetStartHeroes();
		}
		else
		{
			for (int i = 0; i < PlayerData.Heroes.Count; i++)
			{
				if (PlayerData.Heroes[i].SlotNo > 0)
				{
					DeployHero(PlayerData.Heroes[i], PlayerData.Heroes[i].SlotNo);
				}
			}
		}
	}

	void SetStartHeroes()
	{
		for (int i = 0; i < ConstantData.StartHeroes.Length; i++)
		{
			HeroData heroData = null;

			for (int k = 0; k < PlayerData.Heroes.Count; k++)
			{
				if(PlayerData.Heroes[k].Id == ConstantData.StartHeroes[i])
				{
					heroData = PlayerData.Heroes[k];
					break;
				}
			}

			HeroChart chart = CsvData.Ins.HeroChart[heroData.Id][heroData.Grade];
			GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/DeployHeroIcon") as GameObject, Slots[i].transform);
			obj.transform.SetAsFirstSibling();
			obj.GetComponent<HeroBase>().Init(heroData, PlayerData.Slots[i]);
			heroData.DeployHero(i + 1);
		}
	}

	public void DeployHero(HeroData heroData, int slotNo)
	{
		for(int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			if (HeroBase.Heroes[i].Data.SlotNo == slotNo)
			{
				HeroBase.Heroes[i].Data.ReleaseHero();
				HeroBase.Heroes[i].Destroy();
			}	
		}

		HeroChart chart = CsvData.Ins.HeroChart[heroData.Id][heroData.Grade];
		GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/DeployHeroIcon") as GameObject, Slots[slotNo - 1].transform);
		obj.transform.SetAsFirstSibling();
		obj.GetComponent<HeroBase>().Init(heroData, PlayerData.Slots[slotNo - 1]);
		heroData.DeployHero(slotNo);
		PlayerData.Save();
	}

	void SetSlots()
	{
		for(int i = 0; i < Slots.Count; i++)
		{
			Slots[i].Init(PlayerData.Slots[i]);
		}
	}

	IEnumerator SetStage(int stageNo)
	{
		TopBar.SetStageText(stageNo);

		yield return new WaitForSeconds(1f);

		bool isBossStage = false;

		List<StageChart> stageCharts = CsvData.Ins.StageChart[stageNo];

		for(int i = 0; i < stageCharts.Count; i++)
		{
			if (stageCharts[i].Boss != null)
				isBossStage = true;
		}

		if (isBossStage)
			yield return StartCoroutine(BossSequence());

		yield return new WaitForSeconds(2f);

		cStageSequence = StartCoroutine(ProgressStage(stageNo));
	}

	IEnumerator BossSequence()
	{		
		DialogManager.Ins.OpenBossWarning();

		yield return new WaitForSeconds(4f);
	}

	IEnumerator ProgressStage(int stageNo)
	{
		LastEnemiesSpawned = false;

		EnemySpawner.Ins.Spawn(stageNo);

		while (!LastEnemiesSpawned)
		{   
			yield return null;
		}
		
		while (EnemyBase.Enemies.Count > 0)
		{
			yield return null;
		}

		WinStage();
	}


	void WinStage()
	{
		PlayerData.NextStage();
		StartCoroutine(SetStage(PlayerData.Stage));   
	}

	public IEnumerator LoseStage()
	{
		for(int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			StartCoroutine(HeroBase.Heroes[i].Lose());
		}

		yield return new WaitForSeconds(1f);


	}

	
	public void GetGold(double value)
	{

		ChangeGold(value);
	}

	public void ChangeGold(double value)
	{
		PlayerData.ChangeGold(value);
		GoldChanged(value);
	}

	public void ChangeGem(double value)
	{
		PlayerData.ChangeGem(value);
		GemChanged(value);
	}

	public void ChangeMagicite(double value)
	{
		PlayerData.ChangeMagicite(value);
		MagiciteChanged(value);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			ChangeGold(100f);
		}
	}

}