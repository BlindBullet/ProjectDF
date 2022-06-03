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
	public Button LanguageBtn;
	public TextMeshProUGUI LanguageBtnText;

	public void OpenDialog()
	{
		Title.text = LanguageManager.Ins.SetString("Setting");
		BgmText.text = LanguageManager.Ins.SetString("BGM");
		SfxText.text = LanguageManager.Ins.SetString("SFX");
		LanguageBtnText.text = LanguageManager.Ins.SetString("SelectLanguage");

		BgmToggle.isOn = StageManager.Ins.PlayerData.OnBGM;
		SfxToggle.isOn = StageManager.Ins.PlayerData.OnSFX;

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

		Show(false, true);
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
