using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoSingleton<StageManager>
{
	public PlayerData PlayerData = new PlayerData();
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
		Init();
	}

	public void Init()
	{
		PlayerData = new PlayerData();
		PlayerData.Init();

		TopBar.Init();
		SetSlots();
		SetStartHeroes();
		StartCoroutine(SetStage(PlayerData.Stage));        
	}

	public void Load()
	{

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
			PlayerData.DeployHero(heroData, i + 1);
		}
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
		yield return new WaitForSeconds(2f);

		TopBar.SetStageText(stageNo);

		cStageSequence = StartCoroutine(ProgressStage(stageNo));
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

	public void LoseStage()
	{

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