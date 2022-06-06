using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoSingleton<AdmobManager>
{
	public bool isTestMode;
	const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
	const string InterstitialAdTestId = "ca-app-pub-3940256099942544/1033173712";
	RewardedAd suppliesAd;
	RewardedAd questRewardAd;
	InterstitialAd questRefreshAd;	
	RewardedAd ascensionRewardAd;
	RewardedAd offlineRewardAd;	
	AdRequest request;	

	void Start()
	{
		// 모바일 광고 SDK를 초기화함.
		MobileAds.Initialize(initStatus => { });

		request = new AdRequest.Builder().Build();
				
		suppliesAd = new RewardedAd(isTestMode ? rewardTestID : suppliesRewardId);
		LoadAd(AdType.SuppliesReward);
		questRefreshAd = new InterstitialAd(isTestMode ? InterstitialAdTestId : questRefreshRewardId);
		LoadAd(AdType.QuestRefresh);
		questRewardAd = new RewardedAd(isTestMode ? rewardTestID : questRewardId);
		LoadAd(AdType.QuestReward);		
		ascensionRewardAd = new RewardedAd(isTestMode ? rewardTestID : ascensionRewardId);
		LoadAd(AdType.AscensionReward);
		offlineRewardAd = new RewardedAd(isTestMode ? rewardTestID : offlineRewardId);
		LoadAd(AdType.OfflineReward);
				
		this.suppliesAd.OnUserEarnedReward += SuccessSuppliesAd;		
		this.suppliesAd.OnAdFailedToShow += FailedSuppliesAd;

		this.questRewardAd.OnUserEarnedReward += SuccessQuestRewardAd;
		this.questRewardAd.OnAdFailedToShow += FailedQuestRewardAd;

		this.ascensionRewardAd.OnUserEarnedReward += SuccessAscensionRewardAd;
		this.ascensionRewardAd.OnAdFailedToShow += FailedAscensionRewardAd;

		this.offlineRewardAd.OnUserEarnedReward += SuccessOfflineRewardAd;
		this.offlineRewardAd.OnAdFailedToShow += FailedOfflineRewardAd;
	}

	void LoadAd(AdType type)
	{
		switch (type)
		{			
			case AdType.SuppliesReward:
				suppliesAd.LoadAd(request);
				break;
			case AdType.QuestRefresh:
				questRefreshAd.LoadAd(request);
				break;
			case AdType.QuestReward:
				questRewardAd.LoadAd(request);
				break;
			case AdType.AscensionReward:
				ascensionRewardAd.LoadAd(request);
				break;
			case AdType.OfflineReward:
				offlineRewardAd.LoadAd(request);
				break;
		}
	}

	#region 보급품 리워드 광고
	const string suppliesRewardId = "ca-app-pub-7304648099168356/5785904836";
	RewardedAd rewardAd;

	public void ShowSplliesAd()
	{
		suppliesAd.Show();
	}

	void SuccessSuppliesAd(object sender, Reward args)
	{
		DialogAdReward dialog = DialogAdReward._Dialog;
		StartCoroutine(dialog.GetSuppliesReward(true));
		LoadAd(AdType.SuppliesReward);
	}

	void FailedSuppliesAd(object sender, AdErrorEventArgs args)
	{
		Debug.Log("광고 실패");
	}
	#endregion

	#region 퀘스트 리워드 광고	
	const string questRewardId = "ca-app-pub-7304648099168356/7860855094";	

	public void ShowQuestRewardAd()
	{	
		questRewardAd.Show();		
	}
	public void SuccessQuestRewardAd(object sender, Reward args)
	{	
		StartCoroutine(DialogAdReward._Dialog.GetQuestReward(true));
		LoadAd(AdType.QuestReward);
	}

	void FailedQuestRewardAd(object sender, AdErrorEventArgs args)
	{
		Debug.Log("광고 실패");
	}
	#endregion

	#region 어센션 보상 광고
	const string ascensionRewardId = "ca-app-pub-7304648099168356/3207294219";

	public void ShowAscensionRewardAd()
	{	
		ascensionRewardAd.Show();
	}

	void SuccessAscensionRewardAd(object sender, Reward args)
	{
		StartCoroutine(DialogAscension._Dialog.GetAdReward());
		LoadAd(AdType.AscensionReward);
	}

	void FailedAscensionRewardAd(object sender, AdErrorEventArgs args)
	{
		Debug.Log("광고 실패");
	}
	#endregion

	#region 오프라인 보상 광고
	const string offlineRewardId = "ca-app-pub-7304648099168356/6992009449";

	public void ShowOfflineRewardAd()
	{
		offlineRewardAd.Show();		
	}

	void SuccessOfflineRewardAd(object sender, Reward args)
	{
		StartCoroutine(DialogOfflineReward._Dialog.ShowAdReward());
		LoadAd(AdType.OfflineReward);
	}

	void FailedOfflineRewardAd(object sender, AdErrorEventArgs args)
	{
		Debug.Log("광고 실패");
	}
	#endregion

	#region 퀘스트 갱신 광고	
	const string questRefreshRewardId = "ca-app-pub-7304648099168356/3848144816";

	public void ShowQuestRefreshAd()
	{
		if (questRefreshAd.IsLoaded())
			questRefreshAd.Show();

		StartCoroutine(DialogQuest._Dialog.ResetQuest());
		LoadAd(AdType.QuestRefresh);
	}
	#endregion
}

public enum AdType
{
	SuppliesReward,
	QuestReward,
	QuestRefresh,	
	AscensionReward,
	OfflineReward,

}