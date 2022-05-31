using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class TimeManager : MonoSingleton<TimeManager>
{
	public string[] urls;
	public DateTime ReceivedTime;
	public float SinceTime = 0;
	Coroutine cTimer = null;
	
	public IEnumerator GetTime()
	{
		yield return StartCoroutine(WebChk());

		if (cTimer != null)
			StopCoroutine(cTimer);

		cTimer = StartCoroutine(Timer());
	}

	public DateTime GetCurrentTime()
	{
		return ReceivedTime.AddSeconds(SinceTime);
	}

	IEnumerator WebChk()
	{
		UnityWebRequest request = new UnityWebRequest();

		int randNo = UnityEngine.Random.Range(0, urls.Length);
		
		using (request = UnityWebRequest.Get(urls[randNo]))
		{
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				Debug.Log(request.error);
				ReceivedTime = DateTime.UtcNow;
			}
			else
			{
				string date = request.GetResponseHeader("date"); //이곳에서 반송된 데이터에 시간 데이터가 존재				
				ReceivedTime = DateTime.Parse(date).ToUniversalTime();
				Debug.Log(urls[randNo] + " UTC " + ReceivedTime);				
			}
		}
	}

	IEnumerator Timer()
	{
		SinceTime = 0f;

		while (true)
		{
			SinceTime += Time.unscaledDeltaTime;
			yield return null;
		}
	}
}