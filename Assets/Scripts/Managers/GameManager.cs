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
		DialogManager.Ins.SetDialogTransform();
		//SaveLoadManager.Ins.LoadAllDatas();

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{	
			Debug.Log(TimeManager.Ins.ReceivedTime);
			Debug.Log(TimeManager.Ins.SinceTime);
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			TimeManager.Ins.GetTime();			
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