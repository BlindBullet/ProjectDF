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

public enum GameState
{
	None,
	Lobby,
	StageReady,
	StageStart,
	StageWin,
	StageLose,

	

}
