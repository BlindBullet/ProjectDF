using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{	
	[SerializeField] private CameraController camCon;
	[SerializeField] private LobbyUI _lobbyUI;	

	public GameState State;
	public bool StartBattle = false;
	Coroutine cRun = null;

	private void Start()
	{
		DialogManager.Ins.SetDialogTransform();
		SaveLoadManager.Ins.LoadAllDatas();

		State = GameState.Lobby;

		SetLobbyUi();

		StartCoroutine(GameSequence());
	}

	void SetLobbyUi()
	{
		
	}

	IEnumerator GameSequence()
	{
		camCon.SetLobbyCam();
		
		while (State != GameState.StageReady)
		{
			yield return null;
		}
		
		yield return new WaitForSeconds(1f);

		State = GameState.StageStart;

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			State = GameState.StageReady;
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			EnemySpawner.Ins.SpawnEnemy("Gremlin");
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

public enum GameState
{
	None,
	Lobby,
	StageReady,
	StageStart,
	StageWin,
	StageLose,

	

}
