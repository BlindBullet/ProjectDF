using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DialogBossWarning : DialogController
{
	public GameObject BossWarningObj;	
	public CanvasGroup _CanvasGroup;

	public void OpenDialog()
	{	
		Show(false);

		_CanvasGroup.alpha = 0f;

		StartCoroutine(Sequence());
	}

	IEnumerator Sequence()
	{
		_CanvasGroup.DOFade(1, 2f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(4f);

		_CanvasGroup.DOFade(0, 2f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(2f);

		CloseDialog();
	}

}
