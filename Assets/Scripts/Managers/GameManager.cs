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
		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		
	}
		
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Scene scene = SceneManager.GetActiveScene();

			if (scene.name == "Main")
			{
				BackkeyManager.Ins.UseBackkey(true);				
			}	
		}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.S))
		{

		}
#endif
	}

	// Ȩ�̳� �ٸ� ��ư�� ���� ���ø����̼��� ������ �� �ݵ�
	void OnApplicationPause(bool pause)
	{
		bool bPaused = false;

		if (pause)
		{
			bPaused = true;

			// todo : ���ø����̼��� ������ ������ ó���� �ൿ��
			Scene scene = SceneManager.GetActiveScene();

			if(scene.name == "Main")
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
		Scene scene = SceneManager.GetActiveScene();

		if (scene.name == "Main")
			StageManager.Ins.PlayerData.SetOfflineTime();		
	}

}