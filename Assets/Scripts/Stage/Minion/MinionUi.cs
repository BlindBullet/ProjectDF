using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;
using AllIn1SpriteShader;

public class MinionUi : MonoBehaviour
{
	public SpriteRenderer Bg;
	public SpriteRenderer MinionImg;
	public SpriteRenderer Frame;
	Material mat;

	public void Setup(MinionChart chart)
	{	
		mat = Frame.material;
		Frame.transform.GetComponent<AllIn1Shader>().ApplyMaterialToHierarchy();

		Bg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite("Bg_Circle_" + chart.Attr.ToString());
		MinionImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.Model);
		Frame.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite("MinionFrame_" + chart.Attr.ToString());
	}

	public void Summon()
	{		
		mat.SetFloat("_FadeAmount", 1f);
		mat.SetFloat("_HologramBlend", 1f);		

		Sequence seq = DOTween.Sequence();
		seq.Append(mat.DOFloat(0f, "_FadeAmount", 2f).SetEase(Ease.Linear))			
			.Insert(1f, mat.DOFloat(0f, "_HologramBlend", 1f).SetEase(Ease.Linear));
	}

	public void Unsummon()
	{
		mat.SetFloat("_FadeAmount", 0f);
		mat.SetFloat("_HologramBlend", 1f);

		mat.DOFloat(1f, "_FadeAmount", 2f).SetEase(Ease.Linear);	
	}

	


}
