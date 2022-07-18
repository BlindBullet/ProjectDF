using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AscensionSequence : MonoBehaviour
{
	public Image Img;
	public GameObject effect;
	
	private void Awake()
	{	
		this.gameObject.SetActive(false);
	}

	public void FadeIn()
	{		
		effect.SetActive(true);

		SoundManager.Ins.PlaySFX("se_rebirth");

		Sequence seq = DOTween.Sequence();
		seq.AppendInterval(3f)
			.Append(Img.DOFade(1f, 2f).SetEase(Ease.InOutQuad))
			.AppendInterval(1f)
			.Append(Img.DOFade(0f, 2f).SetEase(Ease.InOutQuad))
			.AppendInterval(10f)
			.AppendCallback(() => { this.gameObject.SetActive(false); });		
	}


}
