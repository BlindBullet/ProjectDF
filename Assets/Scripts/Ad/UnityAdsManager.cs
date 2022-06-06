using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityAdsManager : MonoSingleton<UnityAdsManager>, IUnityAdsLoadListener, IUnityAdsShowListener
{
	public string adUnitId = "Rewarded_Android";
	AdType adType;

	// Start is called before the first frame update
	void Start()
	{   
		this.LoadAd();        
	}

	public void LoadAd()
	{
		// IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
		Debug.Log("Loading Ad: " + adUnitId);
		Advertisement.Load(adUnitId, this);
	}
	public void OnUnityAdsAdLoaded(string placementId)
	{
		Debug.LogFormat("OnUnityAdsAdLoaded: {0}", placementId);
	}

	public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
	{
		Debug.LogFormat("OnUnityAdsFailedToLoad: {0}, {1}, {2}", placementId, error, message);
	}


	// Implement a method to execute when the user clicks the button.
	public void ShowAd(AdType adType)
	{
		this.adType = adType;

		// Then show the ad:
		Advertisement.Show(adUnitId, this);
	}

	public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
	{
		DialogManager.Ins.OpenCautionBar("cant_play_ad");
	}

	public void OnUnityAdsShowStart(string placementId)
	{
		Debug.LogFormat("OnUnityAdsShowStart: {0}", placementId);
	}

	public void OnUnityAdsShowClick(string placementId)
	{
		Debug.LogFormat("OnUnityAdsShowClick: {0}", placementId);
	}

	public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
	{
		if (adUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
		{
			switch (adType)
			{
				case AdType.AscensionReward:
					StartCoroutine(DialogAscension._Dialog.GetAdReward());
					break;
				case AdType.OfflineReward:
					StartCoroutine(DialogOfflineReward._Dialog.ShowAdReward());
					break;
				case AdType.QuestReward:
					StartCoroutine(DialogAdReward._Dialog.GetQuestReward(true));
					break;
				case AdType.SuppliesReward:
					DialogAdReward dialog = DialogAdReward._Dialog;
					StartCoroutine(dialog.GetSuppliesReward(true));
					break;
			}

			// Load another ad:
			Advertisement.Load(adUnitId, this);
		}
	}
}
