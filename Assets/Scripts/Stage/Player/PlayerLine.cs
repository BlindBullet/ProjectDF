using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerLine : MonoBehaviour
{
	public SpriteRenderer Img;
	Material mat;
	public int count = 1;

	private void Awake()
	{
		count = 1;
		mat = this.transform.GetComponent<Renderer>().material;		
	}

	public void Refresh()
	{
		count = 1;
		this.GetComponent<BoxCollider2D>().enabled = true;
		mat.DOFloat(0f, "_FadeAmount", 2f).SetEase(Ease.InOutQuad);
	}

	public void Destroy()
	{	
		if(count == 1)
		{
			this.GetComponent<BoxCollider2D>().enabled = false;

			count--;

			Sequence seq = DOTween.Sequence();
			seq.Append(mat.DOFloat(1f, "_FadeAmount", 1f).SetEase(Ease.InOutQuad))
				.AppendCallback(() => this.gameObject.SetActive(false));
		}
	}

	
}
