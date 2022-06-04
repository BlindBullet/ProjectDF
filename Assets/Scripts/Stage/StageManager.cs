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
	public List<PlayerLine> PlayerLines = new List<PlayerLine>();
	[HideInInspector] public int Hp;
	public LoseStagePanel LoseStagePanel;
	public AscensionSequence AscensionSequence;
	public GameObject Moat;
	public GameObject OfflineRewardPanel;

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
	Coroutine cLoseSeq = null;

	private void Start()
	{
		TutorialManager.Ins.InitTutorial();
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
		SoundManager.Ins.DissolveBGMVolume(1f, 1f);
		TopBar.Setup();
		
		SetSlots();
		SetHeroes(PlayerData.IsFirstPlay);

		StartCoroutine(OpenOfflineReward(PlayerData.IsFirstPlay));

		if (PlayerData.IsFirstPlay)
			PlayerData.RunFirstPlay();

		SEManager.Ins.Apply();
		PlayerBuffManager.Ins.RunAllBuffs();
		StartCoroutine(SetStage(PlayerData.Stage));		
	}

	void RestartStage()
	{
		TopBar.Setup();

		SetSlots();
		SetHeroes(PlayerData.IsFirstPlay);		

		SEManager.Ins.Apply();
		PlayerBuffManager.Ins.RunAllBuffs();
		StartCoroutine(SetStage(PlayerData.Stage));
	}

	IEnumerator OpenOfflineReward(bool isFirstPlay)
	{
		yield return StartCoroutine(TimeManager.Ins.GetTime());
		
		if (!isFirstPlay)
		{
			OfflineRewardPanel.SetActive(true);
			yield return new WaitForSeconds(3f);
			DialogManager.Ins.OpenOfflineReward();
			OfflineRewardPanel.SetActive(false);
		}
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
				LoseStagePanel.FadeIn();

				yield return new WaitForSeconds(2f);

				if (Bg != null)
					Destroy(Bg);

				Bg = Instantiate(Resources.Load("Prefabs/Bgs/" + chart.Bg) as GameObject);
				if (chart.Bgm != null && SoundManager.Ins.currentBgm != chart.Bgm)
					SoundManager.Ins.ChangeBGM(chart.Bgm);
			}
			else
			{
				if (Bg != null)
					Destroy(Bg);

				Bg = Instantiate(Resources.Load("Prefabs/Bgs/" + chart.Bg) as GameObject);
				if (chart.Bgm != null && SoundManager.Ins.currentBgm != chart.Bgm)
					SoundManager.Ins.ChangeBGM(chart.Bgm);
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
					Bg = Instantiate(Resources.Load("Prefabs/Bgs/" + _chart.Bg) as GameObject);					
					if (_chart.Bgm != null && SoundManager.Ins.currentBgm != _chart.Bgm)
						SoundManager.Ins.ChangeBGM(_chart.Bgm);
					break;
				}
			}
		}
	}

	IEnumerator SetStage(int stageNo)
	{
		yield return StartCoroutine(SetBg(stageNo));
		
		TutorialManager.Ins.SetTutorial();

		TopBar.SetStageText(stageNo);

		if (CheckBossStage(stageNo))
		{
			yield return StartCoroutine(BossSequence());
		}
		else
		{
			yield return new WaitForSeconds(1f);

			if (CheckAppearSupplies())
			{
				StartCoroutine(AppearSupplies());
			}
		}
		
		cStageSequence = StartCoroutine(ProgressStage(stageNo));
	}

	void RefreshLine()
	{
		for (int i = 0; i < PlayerLines.Count; i++)
		{
			PlayerLines[i].gameObject.SetActive(true);
			PlayerLines[i].Refresh();
		}

		Hp = PlayerLines.Count;
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
		yield return new WaitForSeconds(5f);

		List<SuppliesChart> charts = new List<SuppliesChart>();
		List<int> probs = new List<int>();
		
		foreach(KeyValuePair<string, SuppliesChart> elem in CsvData.Ins.SuppliesChart)
		{
			charts.Add(elem.Value);
			probs.Add(elem.Value.Prob);
		}

		int randNo = LotteryCalculator.LotteryCalc(probs);
		SuppliesChart result = charts[randNo];
		var supplies = ObjectManager.Ins.Pop<SuppliesBase>(Resources.Load("Prefabs/Supplies/Supplies") as GameObject);
		supplies.Setup(result);
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

		RefreshLine();
		EnemySpawner.Ins.Spawn(stageNo);

		while (!LastEnemiesSpawned)
		{   
			yield return null;
		}
		
		while (true)
		{
			yield return new WaitForSeconds(2f);

			if (EnemyBase.Enemies.Count <= 0)
			{
				break;
			}
		}

		yield return new WaitForSeconds(2f);

		while (true)
		{
			yield return new WaitForSeconds(2f);

			if (EnemyBase.Enemies.Count <= 0)
			{
				break;
			}
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

		if(cLoseSeq == null)
			cLoseSeq = StartCoroutine(LoseStageSequence());
	}

	public IEnumerator LoseStageSequence()
	{
		BattleInputManager.Ins.isPause = true;

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

		for (int i = 0; i < EnemyBase.Enemies.Count; i++)
		{
			EnemyBase.Enemies[i].Stop();
		}

		LoseStagePanel.gameObject.SetActive(true);
		LoseStagePanel.FadeIn();

		yield return new WaitForSeconds(2f);

		for (int i = 0; i < EnemyBase.Enemies.Count; i++)
		{
			EnemyBase.Enemies[i].Stop();
		}

		for (int i = EnemyBase.Enemies.Count - 1; i >= 0; i--)
		{
			EnemyBase.Enemies[i].Destroy();
		}

		if (!CheckBossStage(PlayerData.Stage - 1 < 1 ? 1 : PlayerData.Stage - 1))
			ChangeStage(-1);

		RestartStage();
		cLoseSeq = null;
		BattleInputManager.Ins.isPause = false;
	}	

	public void StartAscension(bool isAdAscension = false)
	{
		if(cStageSequence != null)
			StopCoroutine(cStageSequence);

		//보상 주기
		double rewardAmount = ConstantData.GetAscensionMagicite(PlayerData.Stage);
		rewardAmount = rewardAmount + (rewardAmount * (PlayerStat.AscensionReward / 100f));

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

		AscensionSequence.FadeIn();

		yield return new WaitForSeconds(2f);

		for (int i = EnemyBase.Enemies.Count - 1; i >= 0; i--)
		{
			EnemyBase.Enemies[i].Destroy();
		}

		for(int i = HeroBase.Heroes.Count - 1; i >= 0; i--)
		{
			HeroBase.Heroes[i].Destroy();
		}

		EnemySpawner.Ins.StopSpawn();
		RestartStage();
		PlayerUi.SetQuestBtn();		
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

	public void MoatOn()
	{		
		Moat.SetActive(true);
	}

}