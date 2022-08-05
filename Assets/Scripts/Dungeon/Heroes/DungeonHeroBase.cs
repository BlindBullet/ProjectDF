using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using DG.Tweening;
using System;

public class DungeonHeroBase : MonoBehaviour
{
	public static List<DungeonHeroBase> Heroes = new List<DungeonHeroBase>();

	public Image IconBg;
	public Image IconFrame;
	public Image IconImg;
	public GameObject[] Stars;
	HeroData data;
	HeroChart chart;
	public double Atk;
	public float Spd;
	public Attr Attr;
	float originPosY = 0;
	RectTransform rect;

	public void SetHero(HeroData data)
	{
		this.data = data;		
		var heroes = CsvData.Ins.HeroChart[data.Id];

		for(int i = 0; i < heroes.Count; i++)
		{
			if (heroes[i].Grade == data.Grade)
				chart = heroes[i];
		}

		rect = GetComponent<RectTransform>();
		originPosY = rect.anchoredPosition.y;
		SetIcon();
		SetStat();
		Heroes.Add(this);
	}

	void SetIcon()
	{
		IconBg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite("Bg_Circle_" + chart.Attr.ToString());
		IconFrame.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite("Frame_Circle_" + chart.Attr.ToString());
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.Model);

		for(int i = 0; i < data.Grade; i++)
		{
			Stars[i].SetActive(true);
		}
	}

	void SetStat()
	{
		Atk = ConstantData.GetHeroAtk(chart.Atk, 1, data.EnchantLv) * (DungeonManager.Ins.HeroAtkIncRate / 100f);
		Spd = chart.Spd;
		Attr = chart.Attr;
	}

	public IEnumerator Attack()
	{		
		while (true)
		{
			yield return new WaitForSeconds(1f / Spd);

			rect.DOAnchorPosY(rect.anchoredPosition.y + 20f, 0.1f).SetEase(Ease.OutQuad);

			yield return new WaitForSeconds(0.1f);

			double value = DungeonManager.Ins.Enemy.TakeDmg(Atk, Attr);

			switch (Attr)
			{
				case Attr.Red:
					EffectManager.Ins.ShowFx("HitFxRed", DungeonManager.Ins.Enemy.transform.position);					
					break;
				case Attr.Blue:
					EffectManager.Ins.ShowFx("HitFxBlue", DungeonManager.Ins.Enemy.transform.position);					
					break;
				case Attr.Green:
					EffectManager.Ins.ShowFx("HitFxGreen", DungeonManager.Ins.Enemy.transform.position);					
					break;
			}

			FloatingTextManager.Ins.ShowDungeonDmg(
				new Vector3(DungeonManager.Ins.Enemy.transform.position.x, DungeonManager.Ins.Enemy.transform.position.y + 2f, DungeonManager.Ins.Enemy.transform.position.z), value.ToCurrencyString(), Attr);
			rect.DOAnchorPosY(originPosY, 0.1f).SetEase(Ease.Linear);

			yield return new WaitForSeconds(0.1f);
		}
	}

	public void Stop()
	{
		StopAllCoroutines();
		DOTween.Kill(this.gameObject);
		Heroes.Remove(this);
	}
}
