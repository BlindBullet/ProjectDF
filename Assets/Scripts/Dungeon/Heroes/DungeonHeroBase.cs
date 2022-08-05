using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using DG.Tweening;

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
	float originPosY = 0;
	RectTransform rect;

	public void SetHero(HeroData data)
	{
		this.data = data;
		chart = CsvData.Ins.HeroChart[data.Id][data.Grade];
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
	}

	public IEnumerator Attack()
	{
		yield return new WaitForSeconds(1f / Spd);

		rect.DOAnchorPosY(rect.anchoredPosition.y + 10f, 0.1f).SetEase(Ease.OutQuad);

		yield return new WaitForSeconds(0.1f);

		Debug.Log("АјАн");

		rect.DOAnchorPosY(originPosY, 0.1f).SetEase(Ease.Linear);

		yield return new WaitForSeconds(0.1f);
	}


}
