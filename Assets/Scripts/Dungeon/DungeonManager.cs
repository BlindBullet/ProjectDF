using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DungeonManager : MonoSingleton<DungeonManager>
{
	public List<DungeonHeroBase> Heroes = new List<DungeonHeroBase>();
	public PlayerData PlayerData = new PlayerData();
	public DungeonSaveData DungeonData = new DungeonSaveData();
	public EquipmentSaveData EquipmentData = new EquipmentSaveData();
	public TextMeshProUGUI DungeonNameText;
	public DungeonEnemyBase Enemy;
	public EquipmentData Weapon;	
	public double HeroAtkIncRate = 100f;
	public TextMeshProUGUI TimeText;
	public Image TimeFill;
	public float _time = 30f;

	private void Start()
	{
		DialogManager.Ins.SetDialogTransform();

		SoundManager.Ins.ChangeBGM("Time_for_Battle__30s_Loop");
		SoundManager.Ins.DissolveBGMVolume(1f, 1f);
		_time = 30f;
		PlayerData.Load();
		DungeonData.Load();
		EquipmentData.Load();

		DungeonNameText.text = LanguageManager.Ins.SetString("Dungeon") + " " + DungeonData.CurDungeonLv.ToString() + LanguageManager.Ins.SetString("Floor");

		HeroAtkIncRate = 100f;
		Weapon = null;
		
		for(int i = 0; i < EquipmentData.Datas.Count; i++)
		{
			if (EquipmentData.Datas[i].Type == EquipmentType.Weapon && EquipmentData.Datas[i].isEquip)
				Weapon = EquipmentData.Datas[i];		
		}

		if(Weapon != null)
		{
			EquipmentChart equipmentChart = CsvData.Ins.EquipmentChart[Weapon.Id];
			SEChart seChart = CsvData.Ins.SEChart[equipmentChart.EquipEffect];
			HeroAtkIncRate += ConstantData.CalcValue(double.Parse(seChart.EParam5), seChart.LvUpIncRate, Weapon.EnchantLv);
		}

		SetDungeon();
	}

	public void SetDungeon()
	{
		DungeonChart chart = CsvData.Ins.DungeonChart[DungeonData.CurDungeonLv.ToString()];
		SetHeroes();
		Enemy.SetEnemy(chart);
		StartCoroutine(DungeonSeq());
	}

	void SetHeroes()
	{
		int count = 0;

		for(int i = 0; i < PlayerData.Heroes.Count; i++)
		{
			if (PlayerData.Heroes[i].IsOwn)
			{
				Heroes[count].gameObject.SetActive(true);
				Heroes[count].SetHero(PlayerData.Heroes[i]);
				count++;
			}
		}
	}

	IEnumerator DungeonSeq()
	{
		yield return new WaitForSeconds(1f);

		for(int i = 0; i < DungeonHeroBase.Heroes.Count; i++)
		{
			StartCoroutine(DungeonHeroBase.Heroes[i].Attack());
		}

		StartCoroutine(TimeStart());

	}

	IEnumerator TimeStart()
	{
		while (_time > 0)
		{
			TimeText.text = Math.Round(_time, 2).ToString();
			TimeFill.fillAmount = _time / 30f;
			_time -= Time.deltaTime;
			yield return null;			
		}

		Lose();
	}

	public void Win()
	{
		StopAllCoroutines();

		for (int i = 0; i < DungeonHeroBase.Heroes.Count; i++)
		{
			DungeonHeroBase.Heroes[i].Stop();
		}
				
		DungeonHeroBase.Heroes.Clear();
		StartCoroutine(WinSequence());
	}

	IEnumerator WinSequence()
	{		
		//½Â¸® ½Ã°£
		int time = (int)(30f - Mathf.Round(_time));
		//½Â¸® º¸»ó Áõ°¡
		float rewardInc = Mathf.Round(30f - time) / 30f;
		
		DungeonChart chart = CsvData.Ins.DungeonChart[DungeonData.CurDungeonLv.ToString()];
		
		double getSoulStone = Math.Round(chart.RewardSoulStone * (1 + rewardInc), 0);
		PlayerData.ChangeSoulStone(getSoulStone);
		double getEnchantStone = Math.Round(chart.RewardEnchantStoneMin * (1 + rewardInc), 0);
		EquipmentData.GetEnchantStone(getEnchantStone);

		yield return new WaitForSeconds(1f);

		DialogManager.Ins.OpenDungeonWin(getSoulStone, getEnchantStone);
	}

	public void Lose()
	{
		StopAllCoroutines();

		for (int i = 0; i < DungeonHeroBase.Heroes.Count; i++)
		{			
			DungeonHeroBase.Heroes[i].Stop();
		}
				
		DungeonHeroBase.Heroes.Clear();
		StartCoroutine(LoseSequence());
	}

	IEnumerator LoseSequence()
	{
		yield return new WaitForSeconds(1f);

		MySceneManager.Ins.ChangeScene("Main");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			MySceneManager.Ins.ChangeScene("Main");
		}
	}
}
