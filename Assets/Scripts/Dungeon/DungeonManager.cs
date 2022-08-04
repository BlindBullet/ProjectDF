using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager>
{
	public List<DungeonHeroBase> Heroes = new List<DungeonHeroBase>();
	public DungeonUi Ui;


	public void SetDungeon(int dungeonStageNo)
	{

	}
}
