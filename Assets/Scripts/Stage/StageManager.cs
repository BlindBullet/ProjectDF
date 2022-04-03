using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoSingleton<StageManager>
{
    public PlayerData PlayerData = new PlayerData();
    public TopBar TopBar;

    public List<HeroBase> DeployedHeroes = new List<HeroBase>();

    public event UnityAction<double> GoldChanged;
    public event UnityAction<double> GemChanged;

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
        SetStartHeroes();
        SetStage(PlayerData.Stage);        
    }

    public void Load()
    {

    }

    void SetStartHeroes()
    {
        for (int i = 0; i < ConstantData.StartHeroes.Length; i++)
        {
            HeroChart chart = CsvData.Ins.HeroChart[ConstantData.StartHeroes[i]];
            Hero data = new Hero();
            data.InitData(chart);
            DeployedHeroes[i].Init(data);
        }
    }

    void SetStage(int stageNo)
    {
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
        SetStage(PlayerData.Stage);   
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeGold(100f);
        }
    }

}