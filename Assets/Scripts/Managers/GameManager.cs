using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{

	private void Start()
	{
		DialogManager.Ins.SetDialogTransform();
		SaveLoadManager.Ins.LoadAllDatas();

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			HeroData data = StageManager.Ins.PlayerData.Heroes[5];
			StageManager.Ins.DeployHero(data, 1);
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			//CharacterSpawner.Ins.SpawnEnemy("Astronaut");

			EnemySpawner.Ins.SpawnEnemy("Diablo", 1);
			EnemySpawner.Ins.SpawnEnemy("Troll", 1);
			EnemySpawner.Ins.SpawnEnemy("Skeleton", 1);
			//CharacterSpawner.Ins.SpawnEnemy("Gremlin");
		}
	}

	// 홈이나 다른 버튼을 눌러 어플리케이션이 멈췄을 때 콜됨
	void OnApplicationPause(bool pause)
	{
		bool bPaused = false;

		if (pause)
		{
			bPaused = true;

			// todo : 어플리케이션을 내리는 순간에 처리할 행동들

		}
		else
		{
			if (bPaused)
			{

				bPaused = false;

				//todo : 내려놓은 어플리케이션을 다시 올리는 순간에 처리할 행동들 
			}
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(0);
	}


}