using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : SingletonObject<SaveLoadManager>
{
    public PlayerData PlayerData;

	public void LoadAllDatas()
	{
		PlayerData = new PlayerData();
		PlayerData.Load();

	}


}
