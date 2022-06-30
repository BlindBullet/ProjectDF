using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityAdsManager : MonoSingleton<UnityAdsManager>, IUnityAdsLoadListener, IUnityAdsShowListener
{
	public string AscensionRewarded = "AscensionRewarded";
	public string QuestRewarded = "QuestRewarded";	
	public string SuppliesRewarded = "SuppliesRewarded";
	public string OfflineRewarded = "OfflineRewarded";
	AdType adType;

	// Start is called before the first frame update
	void Start()
	{
		this.LoadAd();
	}

	public void LoadAd()
	{		
		Advertisement.Load(QuestRewarded, this);
		Advertisement.Load(SuppliesRewarded, this);
		Advertisement.Load(AscensionRewarded, this);
		Advertisement.Load(OfflineRewarded, this);
	}

	public void LoadAd(AdType type)
	{
		switch (type)
		{			
			case AdType.QuestReward:
				Advertisement.Load(QuestRewarded, this);
				break;
			case AdType.SuppliesReward:
				Advertisement.Load(SuppliesRewarded, this);
				break;
			case AdType.AscensionReward:
				Advertisement.Load(AscensionRewarded, this);
				break;
			case AdType.OfflineReward:
				Advertisement.Load(OfflineRewarded, this);
				break;
		}
	}

	public void OnUnityAdsAdLoaded(string placementId)
	{
		Debug.LogFormat("OnUnityAdsAdLoaded: {0}", placementId);
	}

	public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
	{
		DialogManager.Ins.OpenCautionBar("cant_play_ad");
		Debug.LogFormat("OnUnityAdsFailedToLoad: {0}, {1}, {2}", placementId, error, message);
	}


	// Implement a method to execute when the user clicks the button.
	public void ShowAd(AdType adType)
	{
		this.adType = adType;

		// Then show the ad:
		switch (adType)
		{
			case AdType.AscensionReward:
				Advertisement.Show(AscensionRewarded, this);
				break;
			case AdType.OfflineReward:
				Advertisement.Show(OfflineRewarded, this);
				break;
			case AdType.QuestRefresh:
				break;
			case AdType.QuestReward:
				Advertisement.Show(QuestRewarded, this);
				break;
			case AdType.SuppliesReward:
				Advertisement.Show(SuppliesRewarded, this);
				break;
		}		
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
		switch (placementId)
		{
			case "OfflineRewarded":
				StartCoroutine(DialogOfflineReward._Dialog.ShowAdReward());
				break;
			case "SuppliesRewarded":
				DialogAdReward dialog = DialogAdReward._Dialog;
				StartCoroutine(dialog.GetSuppliesReward(true));
				LoadAd(AdType.SuppliesReward);
				break;
			case "QuestRewarded":
				StartCoroutine(DialogAdReward._Dialog.GetQuestReward(true));
				LoadAd(AdType.QuestReward);
				break;
		}
	}
}
