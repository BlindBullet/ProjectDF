using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerLine : MonoBehaviour
{
	public SpriteRenderer Img;
	Material mat;

	private void Awake()
	{
		mat = this.transform.GetComponent<Renderer>().material;		
	}

	public void Refresh()
	{
		this.GetComponent<BoxCollider2D>().enabled = true;
		mat.DOFloat(0f, "_FadeAmount", 2f).SetEase(Ease.InOutQuad);
	}

	public void Destroy()
	{
		this.GetComponent<BoxCollider2D>().enabled = false;

		Sequence seq = DOTween.Sequence();
		seq.Append(mat.DOFloat(1f, "_FadeAmount", 1f).SetEase(Ease.InOutQuad))
			.AppendCallback(() => this.gameObject.SetActive(false));
	}

	
}
