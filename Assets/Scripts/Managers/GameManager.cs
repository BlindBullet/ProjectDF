using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{	
	private void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		DialogManager.Ins.SetDialogTransform();
		
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		StartCoroutine(ObjectPooling());
	}

	IEnumerator ObjectPooling()
	{
		foreach (KeyValuePair<string, FxChart> elem in CsvData.Ins.FxChart)
		{
			for (int i = 0; i < elem.Value.PoolCount; i++)
			{
				EffectManager.Ins.ObjectPoolingToStart(elem.Key, new Vector2(0, 30f), 0.1f);
			}

			yield return new WaitForSeconds(0.1f);
		}

		List<ProjectileController> projectiles = new List<ProjectileController>();

		foreach (KeyValuePair<string, List<ProjectileChart>> elem in CsvData.Ins.ProjectileChart)
		{
			projectiles.Clear();

			for (int i = 0; i < elem.Value[0].PoolingCount; i++)
			{
				ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/" + elem.Value[0].Model) as GameObject);
				projectile.transform.position = new Vector2(0, 30f);
				projectiles.Add(projectile);

				yield return new WaitForSeconds(0.1f);
			}

			for (int i = 0; i < projectiles.Count; i++)
			{
				ObjectManager.Ins.Push<ProjectileController>(projectiles[i]);
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackkeyManager.Ins.UseBackkey();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			
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
			StageManager.Ins.PlayerData.SetOfflineTime();
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

	private void OnDestroy()
	{		
		StageManager.Ins.PlayerData.SetOfflineTime();
	}

}