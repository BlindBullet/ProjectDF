using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager>
{
	public List<DungeonHeroBase> Heroes = new List<DungeonHeroBase>();
	public DungeonUi Ui;
	public PlayerData PlayerData = new PlayerData();
	public DungeonSaveData DungeonData = new DungeonSaveData();
	public EquipmentSaveData EquipmentData = new EquipmentSaveData();
	public DungeonEnemyBase Enemy;
	public EquipmentData Weapon;	
	public double HeroAtkIncRate = 100f;	

	private void Start()
	{
		SoundManager.Ins.ChangeBGM("Time_for_Battle__30s_Loop");
		SoundManager.Ins.DissolveBGMVolume(1f, 1f);

		PlayerData.Load();
		DungeonData.Load();
		EquipmentData.Load();

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

		yield return null;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			MySceneManager.Ins.ChangeScene("Main");
		}
	}
}
