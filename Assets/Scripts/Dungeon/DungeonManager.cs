using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager>
{
	public List<DungeonHeroBase> Heroes = new List<DungeonHeroBase>();
	public DungeonUi Ui;
	public PlayerData PlayerData = new PlayerData();
	public DungeonSaveData DungeonData = new DungeonSaveData();

	public void SetDungeon()
	{
		PlayerData.Load();
		DungeonData.Load();


	}
}
