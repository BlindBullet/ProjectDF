using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class StageManager : MonoSingleton<StageManager>
{
	public PlayerData PlayerData = new PlayerData();
	public PlayerStat PlayerStat = new PlayerStat();
	public PlayerUi PlayerUi;
	public TopBar TopBar;
	public Transform PlayerLine;
	public LoseStagePanel LoseStagePanel;
	public AscensionSequence AscensionSequence;

	public List<Slot> Slots = new List<Slot>();

	public event UnityAction<double> GoldChanged;
	public event UnityAction<double> SoulStoneChanged;
	public event UnityAction<double> MagiciteChanged;
	public event UnityAction StageChanged;	

	[HideInInspector]
	public bool LastEnemiesSpawned;
	GameObject Bg = null;
	Coroutine cStageSequence = null;
	float appearSuppliesProb = 0;

	private void Start()
	{
		appearSuppliesProb = 0f;

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
		SetHeroes(PlayerData.IsFirstPlay);
		StartCoroutine(OpenOfflineReward(PlayerData.IsFirstPlay));

		if(PlayerData.IsFirstPlay)
			PlayerData.RunFirstPlay();

		SEManager.Ins.Apply();
		PlayerBuffManager.Ins.RunAllBuffs();
		StartCoroutine(SetStage(PlayerData.Stage));
	}

	IEnumerator OpenOfflineReward(bool isFirstPlay)
	{
		yield return StartCoroutine(TimeManager.Ins.GetTime());

		if(!isFirstPlay)
			DialogManager.Ins.OpenOfflineReward();
	}

	void SetHeroes(bool isFirstPlay)
	{
		if (isFirstPlay)
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

			List<HeroChart> charts = CsvData.Ins.HeroChart[heroData.Id];
			HeroChart chart = null;

			for (int k = 0; k < charts.Count; k++)
			{
				if (heroData.Grade == charts[k].Grade)
					chart = charts[k];
			}

			GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/DeployHeroIcon") as GameObject, Slots[i].transform);
			obj.transform.SetAsFirstSibling();
			obj.GetComponent<HeroBase>().Init(heroData, PlayerData.Slots[i]);
			Slots[i].SetEnchantLabel(heroData);
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

		List<HeroChart> charts = CsvData.Ins.HeroChart[heroData.Id];
		HeroChart chart = null;

		for(int i = 0; i < charts.Count; i++)
		{
			if (heroData.Grade == charts[i].Grade)
				chart = charts[i];
		}

		GameObject obj = Instantiate(Resources.Load("Prefabs/Icons/DeployHeroIcon") as GameObject, Slots[slotNo - 1].transform);
		obj.transform.SetAsFirstSibling();
		obj.GetComponent<HeroBase>().Init(heroData, PlayerData.Slots[slotNo - 1]);
		heroData.DeployHero(slotNo);
		Slots[slotNo - 1].SetEnchantLabel(heroData);
				
		PlayerData.Save();
	}

	void SetSlots()
	{		
		for(int i = 0; i < Slots.Count; i++)
		{
			Slots[i].No = i + 1;
			Slots[i].Init(PlayerData.Slots[i]);
		}
	}

	IEnumerator SetBg(int stageNo)
	{
		int _stageNo = GetStageNo(stageNo);		
		StageChart chart = CsvData.Ins.StageChart[_stageNo.ToString()];
		
		if(chart.Bg != null)
		{
			if(stageNo != 1)
			{
				yield return new WaitForSeconds(2f);

				LoseStagePanel.gameObject.SetActive(true);
				StartCoroutine(LoseStagePanel.FadeIn());

				yield return new WaitForSeconds(2f);

				if (Bg != null)
					Destroy(Bg);

				Bg = Instantiate(Resources.Load("Prefabs/Bgs/" + chart.Bg) as GameObject);
				StartCoroutine(LoseStagePanel.FadeOut());
			}
			else
			{
				if (Bg != null)
					Destroy(Bg);

				Bg = Instantiate(Resources.Load("Prefabs/Bgs/" + chart.Bg) as GameObject);
			}
		}	
		else if(chart.Bg == null && Bg == null)
		{
			for(int i = _stageNo - 1; i >= 0; i--)
			{
				StageChart _chart = CsvData.Ins.StageChart[i.ToString()];

				if (_chart.Bg == null)
					continue;
				else
				{
					yield return new WaitForSeconds(2f);

					LoseStagePanel.gameObject.SetActive(true);
					StartCoroutine(LoseStagePanel.FadeIn());

					yield return new WaitForSeconds(2f);

					if (Bg != null)
						Destroy(Bg);

					Bg = Instantiate(Resources.Load("Prefabs/Bgs/" + chart.Bg) as GameObject);
					StartCoroutine(LoseStagePanel.FadeOut());
					break;
				}
			}
		}
	}

	IEnumerator SetStage(int stageNo)
	{
		yield return StartCoroutine(SetBg(stageNo));
		TopBar.SetStageText(stageNo);

		yield return new WaitForSeconds(0.5f);

		if (CheckBossStage(stageNo))
		{
			yield return StartCoroutine(BossSequence());
		}
		else
		{
			if (CheckAppearSupplies())
			{
				StartCoroutine(AppearSupplies());
			}
		}	

		yield return new WaitForSeconds(2f);

		cStageSequence = StartCoroutine(ProgressStage(stageNo));
	}

	bool CheckAppearSupplies()
	{
		if(PlayerData.Stage < 3)
		{
			return false;
		}
		else
		{
			float randNo = Random.Range(0, 100);

			if(randNo < appearSuppliesProb)
			{
				appearSuppliesProb = -50f;
				return true;
			}
			else
			{
				appearSuppliesProb += 15f;
				return false;
			}
		}
	}

	IEnumerator AppearSupplies()
	{
		yield return new WaitForSeconds(10f);
				
		int randNo = Random.Range(0, CsvData.Ins.SuppliesChart.Count);
		var supplies = ObjectManager.Ins.Pop<SuppliesBase>(Resources.Load("Prefabs/Supplies/Supplies") as GameObject);
		supplies.Setup(CsvData.Ins.SuppliesChart[(randNo + 1).ToString()]);
	}

	bool CheckBossStage(int stageNo)
	{
		bool isBossStage = false;
		int _stageNo = GetStageNo(stageNo);

		StageChart chart = CsvData.Ins.StageChart[_stageNo.ToString()];

		if (chart.Boss != null)
			isBossStage = true;

		return isBossStage;
	}

	public int GetStageNo(int stageNo)
	{
		int result = stageNo;		
		int lastResisteredNo = CsvData.Ins.StageChart[CsvData.Ins.StageChart.Count.ToString()].No;
		
		if (stageNo > lastResisteredNo)
		{
			result = (stageNo % lastResisteredNo) == 0 ? lastResisteredNo : (stageNo % lastResisteredNo);						
		}

		return result;
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
		ChangeStage(1);
		StartCoroutine(SetStage(PlayerData.Stage));   
	}

	void ChangeStage(int count)
	{
		PlayerData.ChangeStage(count);
		StageChanged();
	}

	public void LoseStage()
	{
		StopCoroutine(cStageSequence);		
		StartCoroutine(LoseStageSequence());
	}

	public IEnumerator LoseStageSequence()
	{
		for(int i = 0; i < EnemyBase.Enemies.Count; i++)
		{
			EnemyBase.Enemies[i].Stop();
		}
		
		for (int i = 0; i < MinionBase.Minions.Count; i++)
		{
			MinionBase.Minions[i].IsDie = true;
		}

		for (int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			HeroBase.Heroes[i].Lose();
			Slots[i].Lose();
		}

		yield return new WaitForSeconds(2f);

		LoseStagePanel.gameObject.SetActive(true);
		StartCoroutine(LoseStagePanel.FadeIn());

		yield return new WaitForSeconds(2f);

		for(int i = EnemyBase.Enemies.Count - 1; i >= 0 ; i--)
		{
			EnemyBase.Enemies[i].Destroy();
		}

		yield return new WaitForSeconds(1f);

		StartCoroutine(LoseStagePanel.FadeOut());

		if (!CheckBossStage(PlayerData.Stage - 1 < 1 ? 1 : PlayerData.Stage - 1))
			ChangeStage(-1);

		Load();
	}	

	public void StartAscension(bool isAdAscension = false)
	{
		StopCoroutine(cStageSequence);

		//보상 주기
		double rewardAmount = ConstantData.GetAscensionMagicite(PlayerData.Stage);

		if (isAdAscension)
			rewardAmount = rewardAmount * 2f;	
		
		PlayerData.ChangeMagicite(rewardAmount);
		PlayerData.Ascension();
		StageChanged();

		for (int i = 0; i < EnemyBase.Enemies.Count; i++)
		{
			EnemyBase.Enemies[i].Stop();
		}

		for (int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			HeroBase.Heroes[i].Stop();
		}

		StartCoroutine(Ascension());
	}

	public IEnumerator Ascension()
	{		
		//연출
		AscensionSequence.gameObject.SetActive(true);

		yield return StartCoroutine(AscensionSequence.FadeIn());

		for (int i = EnemyBase.Enemies.Count - 1; i >= 0; i--)
		{
			EnemyBase.Enemies[i].Destroy();
		}

		for(int i = HeroBase.Heroes.Count - 1; i >= 0; i--)
		{
			HeroBase.Heroes[i].Destroy();
		}

		Load();

		StartCoroutine(AscensionSequence.FadeOut());
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

	public void ChangeSoulStone(double value)
	{
		PlayerData.ChangeSoulStone(value);
		SoulStoneChanged(value);
	}

	public void ChangeMagicite(double value)
	{
		PlayerData.ChangeMagicite(value);
		MagiciteChanged(value);
	}

	public void AddPlayerBuff(PlayerBuffType type, double durationTime)
	{
		PlayerBuffManager.Ins.AddBuff(type, durationTime);		
	}

}