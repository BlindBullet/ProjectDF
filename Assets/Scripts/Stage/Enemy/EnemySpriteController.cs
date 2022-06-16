using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;
using System;

public class EnemySpriteController : MonoBehaviour
{
	public SpriteRenderer Model;
	public SpriteRenderer Frame;
	public SpriteRenderer Bg;
	Material frameMat;
	Material modelMat;
	Material bgMat;
	Vector2 originPos;
	public SpriteMask Mask;
	EnemyBase me;

	public void Setup(EnemyBase me, EnemyChart chart)
	{
		this.me = me;
		Model.transform.localPosition = Vector3.zero;
		frameMat = Frame.transform.GetComponent<Renderer>().material;
		modelMat = Model.transform.GetComponent<Renderer>().material;
		bgMat = Bg.transform.GetComponent<Renderer>().material;
		frameMat.SetFloat("_HitEffectBlend", 0f);
		frameMat.SetFloat("_FadeAmount", 0f);
		bgMat.SetFloat("_FadeAmount", 0f);
		modelMat.SetFloat("_FadeAmount", 0f);

		Model.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.Model);
		Frame.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Frame_" + chart.Shape.ToString() + "_" + chart.Attr.ToString());
		Bg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Bg_" + chart.Shape.ToString() + "_" + chart.Attr.ToString());
	}

	public IEnumerator Hit(bool isCrit, float stiffTime)
	{	
		modelMat.SetColor("_HitEffectColor", Color.red);
		modelMat.SetFloat("_HitEffectBlend", 0.6f);
		modelMat.DOFloat(0f, "_HitEffectBlend", 0.5f).SetEase(Ease.InOutBounce);

		if(stiffTime > 0f)
		{
			transform.DOShakePosition(0.5f, isCrit ? new Vector2(0.25f, 0.25f) : new Vector2(0.05f, 0.05f), isCrit ? 25 : 10, 0).SetEase(Ease.InOutBounce);
		}
		else
		{
			Model.transform.DOShakePosition(0.5f, isCrit ? new Vector2(0.25f, 0.25f) : new Vector2(0.05f, 0.05f), isCrit ? 25 : 10, 0).SetEase(Ease.InOutBounce);
		}

		yield return new WaitForSeconds(0.5f);

		me.cHit = null;
		Model.transform.localPosition = Vector3.zero;
	}

	public void Cast()
	{
		Sequence seq = DOTween.Sequence();
		seq.Append(Model.transform.DOShakePosition(0.5f, new Vector2(0.05f, 0.05f), 5, 0).SetEase(Ease.InOutBounce))
			.AppendCallback(() => Model.transform.localPosition = Vector3.zero);		
	}

	public void Die()
	{		
		frameMat.DOFloat(1f, "_FadeAmount", 1.5f).SetEase(Ease.Linear);
		bgMat.DOFloat(1f, "_FadeAmount", 1.5f).SetEase(Ease.Linear);
		modelMat.DOFloat(1f, "_FadeAmount", 1.5f).SetEase(Ease.Linear);
		DOTween.Kill(Model.transform);
		DOTween.Kill(this.transform);
	}

}
