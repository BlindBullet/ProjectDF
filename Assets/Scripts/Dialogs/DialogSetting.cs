using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSetting : DialogController
{
	public TextMeshProUGUI Title;
	public TextMeshProUGUI BgmText;
	public Toggle BgmToggle;
	public TextMeshProUGUI SfxText;
	public Toggle SfxToggle;
	public Toggle DmgTextToggle;
	public TextMeshProUGUI DmgText;
	
	public Button AekashicsBtn;
	public TextMeshProUGUI AekashicsDesc;
	public TextMeshProUGUI AekashicsBtnText;

	public void OpenDialog()
	{
		Title.text = LanguageManager.Ins.SetString("Setting");
		BgmText.text = LanguageManager.Ins.SetString("BGM");
		SfxText.text = LanguageManager.Ins.SetString("SFX");
		DmgText.text = LanguageManager.Ins.SetString("DmgText");
		AekashicsBtnText.text = "Aekashics.moe";
		AekashicsDesc.text = LanguageManager.Ins.SetString("desc_aekashics");

		BgmToggle.isOn = StageManager.Ins.PlayerData.OnBGM;
		SfxToggle.isOn = StageManager.Ins.PlayerData.OnSFX;
		DmgTextToggle.isOn = StageManager.Ins.PlayerData.OnDmgText;

		SetBGM();
		SetSFX();
		
		BgmToggle.onValueChanged.RemoveAllListeners();
		BgmToggle.onValueChanged.AddListener((bool a) => 
		{
			StageManager.Ins.PlayerData.SetBGM(a);
			SetBGM();
		});

		SfxToggle.onValueChanged.RemoveAllListeners();
		SfxToggle.onValueChanged.AddListener((bool a) =>
		{
			StageManager.Ins.PlayerData.SetSFX(a);
			SetSFX();
		});

		DmgTextToggle.onValueChanged.RemoveAllListeners();
		DmgTextToggle.onValueChanged.AddListener((bool a) =>
		{
			StageManager.Ins.PlayerData.SetDmgText(a);			
		});

		AekashicsBtn.onClick.RemoveAllListeners();
		AekashicsBtn.onClick.AddListener(() => 
		{
			GPGSBinder.Inst.LoadCustomLeaderboardArray(GPGSIds.leaderboard_top_stage, 10, 
				GooglePlayGames.BasicApi.LeaderboardStart.TopScores,
				GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime,
				(success, scoreData) => 
				{
					string log = $"{success}\n";
					var score = scoreData.Scores;
					
					for (int i = 0; i < score.Length; i++)
					{
						log += $"{1}, {score[i].rank}, {score[i].value}, {score[i].userID}, {score[i].date}\n";
					}
				});
			
			Application.OpenURL("http://www.akashics.moe/"); 
		} );

		Show(true, true);
	}

	void SetBGM()
	{
		if (BgmToggle.isOn)
			SoundManager.Ins.changeBGMVolume(1f);
		else
			SoundManager.Ins.changeBGMVolume(0f);
	}

	void SetSFX()
	{
		if (SfxToggle.isOn)
			SoundManager.Ins.changeSFXVolume(1f);
		else
			SoundManager.Ins.changeSFXVolume(0f);
	}


}
