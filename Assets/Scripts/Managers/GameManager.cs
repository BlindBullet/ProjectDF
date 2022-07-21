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

	// 홈이나 다른 버튼을 눌러 어플리케이션이 멈췄을 때 콜됨
	void OnApplicationPause(bool pause)
	{
		bool bPaused = false;

		if (pause)
		{
			bPaused = true;

			// todo : 어플리케이션을 내리는 순간에 처리할 행동들
			StageManager.Ins.PlayerData.SetOfflineTime();
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

	private void OnDestroy()
	{		
		StageManager.Ins.PlayerData.SetOfflineTime();
	}

}