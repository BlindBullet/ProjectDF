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
		
	}
		
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackkeyManager.Ins.UseBackkey(true);
		}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.S))
		{
			StageManager.Ins.ChangeGold(100000000000000000000000000000000000f);
			StageManager.Ins.ChangeMagicite(100000000000000000000f);
			StageManager.Ins.ChangeSoulStone(10000f);
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