using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MySceneManager : MonoSingleton<MySceneManager>
{
	public CanvasGroup FadeImg;
	float fadeDuration = 1f;
	public GameObject Loading;
	public TextMeshProUGUI LoadingText;

	private void Start()
	{
		DontDestroyOnLoad(this);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void ChangeScene(string sceneName)
	{	
		FadeImg.DOFade(1, fadeDuration)
			.OnStart(() =>
			{
				FadeImg.blocksRaycasts = true;
			})
			.OnComplete(() =>
			{				
				StartCoroutine(LoadScene(sceneName));
			});
	}

	IEnumerator LoadScene(string sceneName)
	{
		Loading.SetActive(true);

		AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
		async.allowSceneActivation = false;

		float pastTime = 0;
		float percentage = 0;

		while (!(async.isDone))
		{
			yield return null;

			pastTime += Time.deltaTime;

			if (percentage >= 90)
			{
				percentage = Mathf.Lerp(percentage, 100, pastTime);

				if (percentage == 100)
				{
					async.allowSceneActivation = true; //씬 전환 준비 완료
				}
			}
			else
			{
				percentage = Mathf.Lerp(percentage, async.progress * 100f, pastTime);
				if (percentage >= 90) pastTime = 0;
			}
			LoadingText.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		FadeImg.DOFade(0, fadeDuration)
			.OnStart(() =>
			{
				Loading.SetActive(false);
			})
			.OnComplete(() =>
			{
				FadeImg.blocksRaycasts = false;
			});
	}


	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}

