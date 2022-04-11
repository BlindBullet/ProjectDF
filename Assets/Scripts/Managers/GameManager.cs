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

	// Ȩ�̳� �ٸ� ��ư�� ���� ���ø����̼��� ������ �� �ݵ�
	void OnApplicationPause(bool pause)
	{
		bool bPaused = false;

		if (pause)
		{
			bPaused = true;

			// todo : ���ø����̼��� ������ ������ ó���� �ൿ��

		}
		else
		{
			if (bPaused)
			{

				bPaused = false;

				//todo : �������� ���ø����̼��� �ٽ� �ø��� ������ ó���� �ൿ�� 
			}
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(0);
	}


}