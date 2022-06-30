using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsMain : MonoBehaviour, IUnityAdsInitializationListener
{
	public string GameId = "4786815";
	public bool TestMode;

	void Start()
	{	
		Advertisement.Initialize(GameId, TestMode, this);		
	}

	public void OnInitializationComplete()
	{
		Debug.Log("UnityAds Init Complete");
	}

	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		Debug.Log("UnityAds Init Failed");
	}

}
