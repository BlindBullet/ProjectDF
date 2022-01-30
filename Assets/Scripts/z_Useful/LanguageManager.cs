using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LanguageManager : SingletonObject<LanguageManager> {

	private void Awake()
	{
		currentLanguageType = (LanguageType)Enum.Parse(typeof(LanguageType), PlayerPrefs.GetString("CurrentLanguage", "Korean"));
		
		//if(DataController.Ins.PlayerData.RunApplicationCount == 0)
		{
			currentLanguageType = (LanguageType)Enum.Parse(typeof(LanguageType),Application.systemLanguage.ToString());
			PlayerPrefs.SetString("CurrentLanguage", currentLanguageType.ToString());
		}
	}
		
	public enum LanguageType
	{
		Korean,
		English,
		Japanese,
		
	}

	Dictionary<string, StringData> dicData = new Dictionary<string, StringData>();
	public LanguageType currentLanguageType;

	public void ChangeLanguage(LanguageType type)
	{
		currentLanguageType = type;
		//GameManager.Ins.RestartGame();
	}

	public string SetString(string id)
	{
		if (dicData != null)
			dicData = ChartManager.Ins.StringData;

		if (!dicData.ContainsKey(id))
		{
			Debug.LogWarning(id + ": 스트링 키가 없습니다.");
			return id;
		}

		switch (currentLanguageType)
		{
			case LanguageType.Korean:
				if (dicData[id].Korean == "")
				{
					Debug.LogWarning(id + ": 스트링 값이 비어있습니다.");
				}
				return dicData[id].Korean;
			case LanguageType.English:
				if (dicData[id].English == "")
				{
					Debug.LogWarning(id + ": 스트링 값이 비어있습니다.");
				}
				return dicData[id].English;
			default:
				Debug.LogWarning("현재 선택된 언어에 맞는 컬럼이 스트링 테이블에 없습니다.");
				return "";
		}
	}

}
