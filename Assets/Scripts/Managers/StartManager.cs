using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StartManager : MonoBehaviour
{
	public SpriteRenderer Bg;
	public TextMeshProUGUI TouchToStartText;
	public Button BackPanelBtn;

	private void Start()
	{	
		StartCoroutine(StartSequence());
	}

	IEnumerator StartSequence()
	{
		Bg.DOFade(1f, 1.5f).SetEase(Ease.InOutCirc);

		yield return new WaitForSeconds(1.5f);

		//구글 로그인
		GPGSBinder.Inst.Login();

		yield return new WaitForSeconds(1f);

		TouchToStartText.gameObject.SetActive(true);

		TouchToStartText.DOFade(1f, 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);

		BackPanelBtn.onClick.RemoveAllListeners();
		BackPanelBtn.onClick.AddListener(() => MySceneManager.Ins.ChangeScene("Main"));
	}



}
