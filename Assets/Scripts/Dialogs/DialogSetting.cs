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
	
	public Button AekashicsBtn;
	public TextMeshProUGUI AekashicsDesc;
	public TextMeshProUGUI AekashicsBtnText;

	public void OpenDialog()
	{
		Title.text = LanguageManager.Ins.SetString("Setting");
		BgmText.text = LanguageManager.Ins.SetString("BGM");
		SfxText.text = LanguageManager.Ins.SetString("SFX");
		AekashicsBtnText.text = "Aekashics.moe";
		AekashicsDesc.text = LanguageManager.Ins.SetString("desc_aekashics");

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

		AekashicsBtn.onClick.RemoveAllListeners();
		AekashicsBtn.onClick.AddListener(() => Application.OpenURL("http://www.akashics.moe/"));

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
