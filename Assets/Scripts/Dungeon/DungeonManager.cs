using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager>
{
	public List<DungeonHeroBase> Heroes = new List<DungeonHeroBase>();
	public DungeonUi Ui;
	public PlayerData PlayerData = new PlayerData();
	public DungeonSaveData DungeonData = new DungeonSaveData();
	public DungeonEnemyBase Enemy;


	private void Start()
	{
		SoundManager.Ins.ChangeBGM("Time_for_Battle__30s_Loop");
		SoundManager.Ins.DissolveBGMVolume(1f, 1f);

		PlayerData.Load();
		DungeonData.Load();

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
		yield return null;
	}


}
