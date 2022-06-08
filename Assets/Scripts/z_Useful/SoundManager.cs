using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : SingletonObject<SoundManager>
{		
	public int audioSourceCount = 5;
	public int normalAttackaudioSourceCount = 1;

	public string bgmPath = "Sounds/Bgm/";
	public string sfxPath = "Sounds/Sfx/";
	
	private AudioSource BGMsource;
	private AudioSource[] SFXsource;
	private AudioSource[] NA_SFXsource;

	public delegate void CallBack();
	CallBack BGMendCallBack;

	void OnEnable()
	{
		float volume = PlayerPrefs.GetFloat("volumeBGM", 1f);

		BGMsource = gameObject.AddComponent<AudioSource>();
		BGMsource.volume = volume;
		BGMsource.playOnAwake = false;
		BGMsource.loop = true;

		//sfx 소스 초기화
		SFXsource = new AudioSource[audioSourceCount];
		NA_SFXsource = new AudioSource[normalAttackaudioSourceCount];

		volume = PlayerPrefs.GetFloat("volumeSFX", 1f);		

		for (int i = 0; i < SFXsource.Length; i++)
		{
			SFXsource[i] = gameObject.AddComponent<AudioSource>();
			SFXsource[i].playOnAwake = false;
			SFXsource[i].volume = volume;
		}

		for (int i = 0; i < NA_SFXsource.Length; i++)
		{
			NA_SFXsource[i] = gameObject.AddComponent<AudioSource>();
			NA_SFXsource[i].playOnAwake = false;
			NA_SFXsource[i].volume = volume;
		}
	}

	/**********SFX***********/	

	public void PlaySFX(string name, bool loop = false, float pitch = 1)//효과음 재생
	{
		AudioSource a = GetEmptySource();
		a.loop = loop;
		a.pitch = pitch;
		a.clip = Resources.Load<AudioClip>(sfxPath + name);
		a.Play();
	}

	public void PlayNASFX(string name, bool loop = false, float pitch = 1)//효과음 재생
	{
		AudioSource a = GetEmptyNASource();
		a.loop = loop;
		a.pitch = pitch;
		a.clip = Resources.Load<AudioClip>(sfxPath + name);
		a.Play();
	}

	public void StopSFXByName(string name)
	{
		for (int i = 0; i < SFXsource.Length; i++)
		{
			if (SFXsource[i].clip.name == name)
				SFXsource[i].Stop();
		}
	}

	private AudioSource GetEmptySource()//비어있는 오디오 소스 반환
	{
		int lageindex = 0;
		float lageProgress = 0;
		for (int i = 0; i < SFXsource.Length; i++)
		{
			if (!SFXsource[i].isPlaying)
			{
				return SFXsource[i];
			}

			//만약 비어있는 오디오 소스를 못찿으면 가장 진행도가 높은 오디오 소스 반환(루프중인건 스킵)

			float progress = SFXsource[i].time / SFXsource[i].clip.length;
			if (progress > lageProgress && !SFXsource[i].loop)
			{
				lageindex = i;
				lageProgress = progress;
			}
		}
		return SFXsource[lageindex];
	}

	private AudioSource GetEmptyNASource()//비어있는 오디오 소스 반환
	{
		int lageindex = 0;
		float lageProgress = 0;
		for (int i = 0; i < NA_SFXsource.Length; i++)
		{
			if (!NA_SFXsource[i].isPlaying)
			{
				return NA_SFXsource[i];
			}

			//만약 비어있는 오디오 소스를 못찿으면 가장 진행도가 높은 오디오 소스 반환(루프중인건 스킵)

			float progress = NA_SFXsource[i].time / NA_SFXsource[i].clip.length;
			if (progress > lageProgress && !NA_SFXsource[i].loop)
			{
				lageindex = i;
				lageProgress = progress;
			}
		}
		return NA_SFXsource[lageindex];
	}

	/**********BGM***********/

	private AudioClip changeClip;//바뀌는 클립
	private bool isChanging = false;
	private float startTime;
	public string currentBgm = "";

	[SerializeField]
	[Header("Changing speed"), Tooltip("브금 바꾸는 속도")]
	public float ChangingSpeed;

	public void ChangeBGM(string name, bool isSmooth = false, CallBack callback = null)//브금 변경 (브금이름 , 부드럽게 바꾸기)
	{
		currentBgm = name;

		BGMendCallBack = callback;

		changeClip = null;

		changeClip = Resources.Load<AudioClip>(bgmPath + name);

		if (changeClip == null)//없으면 탈주
			return;

		if (!isSmooth)
		{
			BGMsource.clip = changeClip;
			BGMsource.Play();
		}
		else
		{
			startTime = Time.time;
			isChanging = true;
		}
	}

	private void Update()
	{
		if (!isChanging) return;

		float progress = (Time.time - startTime) * ChangingSpeed;//부드러운 오디오 전환
		BGMsource.volume = Mathf.Lerp(PlayerPrefs.GetFloat("volumeBGM", 1f), 0, progress);

		if (progress > 1)
		{
			isChanging = false;
			BGMsource.volume = PlayerPrefs.GetFloat("volumeBGM", 1f);
			BGMsource.clip = changeClip;
			BGMsource.Play();
		}
	}

	public void StopBGM()
	{
		BGMsource.Stop();
	}

	public void SetPitch(float pitch)
	{
		BGMsource.pitch = pitch;
	}

	//비주얼라이저용 오디오 샘플
	public float[] GetAudioSample(int sampleCount, FFTWindow fft)
	{
		float[] samples = new float[sampleCount];

		BGMsource.GetSpectrumData(samples, 0, fft);

		if (samples != null)
			return samples;
		else
			return null;
	}

	//볼륨
	public void changeBGMVolume(float volume)
	{
		PlayerPrefs.SetFloat("volumeBGM", volume);
		BGMsource.volume = volume;
	}

	public void DissolveBGMVolume(float endValue, float time)
	{		
		BGMsource.DOFade(endValue, time).SetEase(Ease.Linear);
	}

	public void changeSFXVolume(float volume)
	{
		PlayerPrefs.SetFloat("volumeSFX", volume);
		for (int i = 0; i < SFXsource.Length; i++)
		{
			SFXsource[i].volume = volume;
		}

		for(int i = 0; i < NA_SFXsource.Length; i++)
		{
			NA_SFXsource[i].volume = volume;
		}
	}
	
}
