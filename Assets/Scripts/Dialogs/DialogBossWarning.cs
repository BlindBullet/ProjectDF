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
		StartCoroutine(WarningSfxSeq());
	}

	IEnumerator Sequence()
	{
		_CanvasGroup.DOFade(1, 2f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(4f);

		_CanvasGroup.DOFade(0, 2f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(2f);

		CloseDialog();
	}

	IEnumerator WarningSfxSeq()
	{
		for(int i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds(1f);
			SoundManager.Ins.PlaySFX("BossWarning");
		}
	}

}
