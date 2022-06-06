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
	RewardedAd questAd;
	InterstitialAd questRefreshAd;
	InterstitialAd powerUpRefreshAd;
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
		questAd = new RewardedAd(isTestMode ? rewardTestID : questRewardId);
		LoadAd(AdType.QuestReward);
		powerUpRefreshAd = new InterstitialAd(isTestMode ? InterstitialAdTestId : powerUpRefreshRewardId);
		LoadAd(AdType.PowerUpRefresh);
		ascensionRewardAd = new RewardedAd(isTestMode ? rewardTestID : ascensionRewardId);
		LoadAd(AdType.AscensionReward);
		offlineRewardAd = new RewardedAd(isTestMode ? rewardTestID : ascensionRewardId);
		LoadAd(AdType.OfflineReward);
	}

	void LoadAd(AdType type)
	{
		switch (type)
		{
			case AdType.PowerUpRefresh:
				powerUpRefreshAd.LoadAd(request);
				break;
			case AdType.SuppliesReward:
				suppliesAd.LoadAd(request);
				break;
			case AdType.QuestRefresh:
				questRefreshAd.LoadAd(request);
				break;
			case AdType.QuestReward:
				questAd.LoadAd(request);
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

	public void ShowSuppliesAdReward(SuppliesChart data)
	{	
		suppliesAd.Show();
		suppliesAd.OnUserEarnedReward += (sender, e) =>
		{	
			StartCoroutine(DialogAdReward._Dialog.GetReward(data));			
		};

		LoadAd(AdType.SuppliesReward);
	}
	#endregion

	#region 퀘스트 리워드 광고	
	const string questRewardId = "ca-app-pub-7304648099168356/7860855094";	

	public void ShowQuestRewardAd(QuestChart chart)
	{	
		questAd.Show();			
		questAd.OnUserEarnedReward += (sender, e) =>
		{
			StartCoroutine(DialogAdReward._Dialog.GetReward(chart, true));			
		};

		LoadAd(AdType.QuestReward);
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

	#region 파워업 리스트 갱신 광고
	const string powerUpRefreshRewardId = "ca-app-pub-7304648099168356/2726634837";

	public void ShowPowerUpRefreshAd()
	{
		if (powerUpRefreshAd.IsLoaded())
			powerUpRefreshAd.Show();

		StartCoroutine(DialogSlotPowerUp._Dialog.Refresh());
		LoadAd(AdType.PowerUpRefresh);
	}
	#endregion

	#region 어센션 보상 광고
	const string ascensionRewardId = "ca-app-pub-7304648099168356/3207294219";

	public void ShowAscensionRewardAd()
	{	
		ascensionRewardAd.Show();
		ascensionRewardAd.OnUserEarnedReward += (sender, e) =>
		{
			StartCoroutine(DialogAscension._Dialog.GetAdReward());
		};

		LoadAd(AdType.AscensionReward);
	}
	#endregion

	#region 오프라인 보상 광고
	const string offlineRewardId = "ca-app-pub-7304648099168356/6992009449";

	public void ShowOfflineRewardAd(double value, double addValue)
	{
		offlineRewardAd.Show();
		offlineRewardAd.OnUserEarnedReward += (sender, e) =>
		{
			StartCoroutine(DialogOfflineReward._Dialog.ShowAdReward(value, addValue));
		};

		LoadAd(AdType.OfflineReward);
	}
	#endregion

}

public enum AdType
{
	SuppliesReward,
	QuestReward,
	QuestRefresh,
	PowerUpRefresh,
	AscensionReward,
	OfflineReward,

}