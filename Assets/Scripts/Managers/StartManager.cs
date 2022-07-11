using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Google.Play.AppUpdate;
using Google.Play.Common;

public class StartManager : MonoBehaviour
{
	public SpriteRenderer Bg;
	public TextMeshProUGUI TouchToStartText;
	public Button BackPanelBtn;
	AppUpdateManager appUpdateManager;

	private void Start()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		appUpdateManager = new AppUpdateManager();
#endif
		StartCoroutine(StartSequence());
	}

	IEnumerator StartSequence()
	{
		Bg.DOFade(1f, 1.5f).SetEase(Ease.InOutCirc);

		yield return new WaitForSeconds(0.5f);

		SoundManager.Ins.ChangeBGM("promotion_005");

		yield return new WaitForSeconds(1.5f);

		//구글 로그인
		GPGSBinder.Inst.Login();

		yield return new WaitForSeconds(1f);

		StartCoroutine(CheckForUpdate());

		TouchToStartText.gameObject.SetActive(true);

		TouchToStartText.DOFade(1f, 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

		BackPanelBtn.onClick.RemoveAllListeners();
		BackPanelBtn.onClick.AddListener(() => 
		{
			SoundManager.Ins.PlaySFX("se_button_2");
			StartCoroutine(ChangeScene());			
		});
	}

	IEnumerator CheckForUpdate()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
		  appUpdateManager.GetAppUpdateInfo();

		// Wait until the asynchronous operation completes.
		yield return appUpdateInfoOperation;

		if (appUpdateInfoOperation.IsSuccessful)
		{
			var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
			// Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
			// IsUpdateTypeAllowed(), etc. and decide whether to ask the user
			// to start an in-app update.

			var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

			// Creates an AppUpdateRequest that can be used to monitor the
			// requested in-app update flow.
			var startUpdateRequest = appUpdateManager.StartUpdate(
			  // The result returned by PlayAsyncOperation.GetResult().
			  appUpdateInfoResult,
			  // The AppUpdateOptions created defining the requested in-app update
			  // and its parameters.
			  appUpdateOptions);

			while (!startUpdateRequest.IsDone)
			{
				// For flexible flow,the user can continue to use the app while
				// the update downloads in the background. You can implement a
				// progress bar showing the download status during this time.
				yield return null;
			}
		}
		else
		{
			// Log appUpdateInfoOperation.Error.
		}
#endif
		yield return null;
	}

	IEnumerator ChangeScene()
	{
		BackPanelBtn.enabled = false;
		TouchToStartText.gameObject.SetActive(false);

		SoundManager.Ins.DissolveBGMVolume(0f, 2f);

		yield return new WaitForSeconds(1.5f);

		MySceneManager.Ins.ChangeScene("Main");
	}

}
