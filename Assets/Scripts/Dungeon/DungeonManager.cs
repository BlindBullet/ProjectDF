using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager>
{
	public List<DungeonHeroBase> Heroes = new List<DungeonHeroBase>();
	public DungeonUi Ui;
	public PlayerData PlayerData = new PlayerData();
	public DungeonSaveData DungeonData = new DungeonSaveData();

	private void Start()
	{
		SoundManager.Ins.ChangeBGM("Time_for_Battle__30s_Loop");
		SoundManager.Ins.DissolveBGMVolume(1f, 1f);

		PlayerData.Load();
		DungeonData.Load();

		
	}

	public void SetDungeon()
	{
		


	}
}
