using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoseStagePanel : MonoBehaviour
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
		seq.Append(mat.DOFloat(0f, "_FadeAmount", 2f).SetEase(Ease.InOutQuad))
			.AppendInterval(0.5f)
			.Append(mat.DOFloat(1f, "_FadeAmount", 2f).SetEase(Ease.Linear))			
			.AppendCallback(() => { this.gameObject.SetActive(false); });
	}

}
