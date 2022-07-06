using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using GooglePlayGames.Android;
using Ugi.PlayInstallReferrerPlugin;


public class AdmobManager : MonoSingleton<AdmobManager>
{
	public bool isTestMode;
	const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
	const string InterstitialAdTestId = "ca-app-pub-3940256099942544/1033173712";
	RewardedAd suppliesAd;
	RewardedAd questRewardAd;
	InterstitialAd questRewardAd2;
	InterstitialAd questRefreshAd;	
	RewardedAd ascensionRewardAd;
	RewardedAd offlineRewardAd;	
	AdRequest request;
	[HideInInspector] public bool isOfflineAdLoaded = false;
	[HideInInspector] public bool isSuppliesRewardAdLoaded = false;
	[HideInInspector] public bool isQuestRewardAdLoaded = false;
	[HideInInspector] public bool isQuestRewardAdInterstitial = false;
	[HideInInspector] public bool isAscensionRewardAdLoaded = false;
	[HideInInspector] public bool isReal = false;

	private void Start()
	{
		Debug.Log("어플리케이션 인스톨러 네임 :" + Application.installerName);
		// 플레이스토어 어플리케이션 인스톨러 네임: com.android.vending
		// 원스토어 
		isReal = false;

		string installerName = Application.installerName;

		if (installerName == "com.android.vending")
			isReal = true;

		// 모바일 광고 SDK를 초기화함.
		MobileAds.Initialize(initStatus => { });

		offlineRewardAd = new RewardedAd(isTestMode ? rewardTestID : offlineRewardId);
		LoadAd(AdType.OfflineReward);
		suppliesAd = new RewardedAd(isTestMode ? rewardTestID : suppliesRewardId);
		LoadAd(AdType.SuppliesReward);
		questRefreshAd = new InterstitialAd(isTestMode ? InterstitialAdTestId : questRefreshRewardId);
		LoadAd(AdType.QuestRefresh);
		questRewardAd2 = new InterstitialAd(isTestMode ? InterstitialAdTestId : questRefreshReward2Id);
		LoadAd(AdType.QuestReward2);
		questRewardAd = new RewardedAd(isTestMode ? rewardTestID : questRewardId);
		LoadAd(AdType.QuestReward);
		ascensionRewardAd = new RewardedAd(isTestMode ? rewardTestID : ascensionRewardId);
		LoadAd(AdType.AscensionReward);

		this.suppliesAd.OnAdLoaded += OnLoadedSuppliesRewardAd;
		this.suppliesAd.OnAdFailedToLoad += OnFailLoadedSuppliesRewardAd;
		this.suppliesAd.OnUserEarnedReward += SuccessSuppliesAd;
		this.suppliesAd.OnAdFailedToShow += FailedSuppliesAd;

		this.questRewardAd.OnAdLoaded += OnLoadedQuestRewardAd;
		this.questRewardAd.OnAdFailedToLoad += OnFailLoadedQuestRewardAd;
		this.questRewardAd.OnUserEarnedReward += SuccessQuestRewardAd;
		this.questRewardAd.OnAdFailedToShow += FailedQuestRewardAd;

		this.ascensionRewardAd.OnAdLoaded += OnLoadedAscensionRewardAd;
		this.ascensionRewardAd.OnAdFailedToLoad += OnFailLoadedAscensionRewardAd;
		this.ascensionRewardAd.OnUserEarnedReward += SuccessAscensionRewardAd;
		this.ascensionRewardAd.OnAdFailedToShow += FailedAscensionRewardAd;

		this.offlineRewardAd.OnAdLoaded += OnLoadedOfflineRewardAd;
		this.offlineRewardAd.OnAdFailedToLoad += OnFailLoadedOfflineRewardAd;
		this.offlineRewardAd.OnUserEarnedReward += SuccessOfflineRewardAd;
		this.offlineRewardAd.OnAdFailedToShow += FailedOfflineRewardAd;
	}

	public void LoadAd(AdType type)
	{
		request = new AdRequest.Builder().Build();

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
			case AdType.QuestReward2:
				questRewardAd2.LoadAd(request);
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

	public void ShowSuppliesAd()
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
		Debug.Log("애드몹 광고 실패");		
	}

	public void OnLoadedSuppliesRewardAd(object sender, EventArgs args)
	{
		Debug.Log("로드 완료");
		isSuppliesRewardAdLoaded = true;
	}

	public void OnFailLoadedSuppliesRewardAd(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("로드 실패");
		isSuppliesRewardAdLoaded = false;
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
		Debug.Log("애드몹 광고 실패");		
	}

	public void OnLoadedQuestRewardAd(object sender, EventArgs args)
	{
		Debug.Log("로드 완료");
		isQuestRewardAdLoaded = true;
	}

	public void OnFailLoadedQuestRewardAd(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("로드 실패");
		isQuestRewardAdLoaded = false;
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
		Debug.Log("애드몹 광고 실패");
		//UnityAdsManager.Ins.ShowAd(AdType.AscensionReward);
	}

	public void OnLoadedAscensionRewardAd(object sender, EventArgs args)
	{
		Debug.Log("로드 완료");
		isAscensionRewardAdLoaded = true;
	}

	public void OnFailLoadedAscensionRewardAd(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("로드 실패");
		isAscensionRewardAdLoaded = false;
	}
	#endregion

	#region 오프라인 보상 광고
	const string offlineRewardId = "ca-app-pub-7304648099168356/6992009449";

	public void ShowOfflineRewardAd()
	{
		offlineRewardAd.Show();
	}

	public void OnLoadedOfflineRewardAd(object sender, EventArgs args) 
	{
		Debug.Log("로드 완료");
		isOfflineAdLoaded = true;		
	}

	public void OnFailLoadedOfflineRewardAd(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("로드 실패");
		isOfflineAdLoaded = false;
	}

	void SuccessOfflineRewardAd(object sender, Reward args)
	{
		StartCoroutine(DialogOfflineReward._Dialog.ShowAdReward());		
	}

	void FailedOfflineRewardAd(object sender, AdErrorEventArgs args)
	{
		Debug.Log("애드몹 광고 실패");		
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

	#region 퀘스트 리워드 전면 광고	
	const string questRefreshReward2Id = "ca-app-pub-7304648099168356/4319200910";

	public void ShowQuestRewardAd2()
	{
		if (questRewardAd2.IsLoaded())
		{
			questRewardAd2.Show();
			StartCoroutine(DialogAdReward._Dialog.GetQuestReward(true));
			LoadAd(AdType.QuestReward2);
		}
		else
		{
			LoadAd(AdType.QuestReward2);

			if (questRewardAd2.IsLoaded())
			{
				questRewardAd2.Show();
				StartCoroutine(DialogAdReward._Dialog.GetQuestReward(true));
				LoadAd(AdType.QuestReward2);
			}
			else
			{
				DialogManager.Ins.OpenCautionBar("cant_play_ad");
			}
		}
	}
	#endregion
}

public enum AdType
{
	SuppliesReward,
	QuestReward,
	QuestReward2,
	QuestRefresh,	
	AscensionReward,
	OfflineReward,

}