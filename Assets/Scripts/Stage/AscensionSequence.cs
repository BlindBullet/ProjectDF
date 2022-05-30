using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AscensionSequence : MonoBehaviour
{
	public Image Img;
	Material mat;

	private void Awake()
	{
		Img.material = new Material(Img.materialForRendering);
		mat = Img.materialForRendering;
		this.gameObject.SetActive(false);
	}

	public void FadeIn()
	{
		Sequence seq = DOTween.Sequence();
		seq.Append(Img.DOFade(1f, 2f).SetEase(Ease.InOutQuad))
			.AppendInterval(1f)
			.Append(Img.DOFade(0f, 2f).SetEase(Ease.InOutQuad))
			.AppendCallback(() => { this.gameObject.SetActive(false); });		
	}


}
