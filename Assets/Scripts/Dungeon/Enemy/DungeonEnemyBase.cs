using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class DungeonEnemyBase : MonoBehaviour
{	
	public SpriteRenderer EnemySprite;
	public double MaxHp;
	public double CurHp;
	public TextMeshProUGUI EnemyName;
	public TextMeshProUGUI HpText;
	public Image HpFill;
	Attr attr;
	Material mat;

	public void SetEnemy(DungeonChart chart)
	{
		EnemyName.text = LanguageManager.Ins.SetString(chart.Name);
		EnemySprite.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.AppearEnemy);
		mat = EnemySprite.GetComponent<Renderer>().material;
		MaxHp = chart.Hp;
		CurHp = chart.Hp;
		attr = chart.Attr;

		SetHpBar();
		Appear();
	}

	void Appear()
	{
		this.transform.DOMoveY(4.8f, 1f).SetEase(Ease.InOutQuad);
	}

	public double TakeDmg(double value, Attr _attr)
	{
		mat.SetColor("_HitEffectColor", Color.red);
		mat.SetFloat("_HitEffectBlend", 0.6f);
		mat.DOFloat(0f, "_HitEffectBlend", 0.5f).SetEase(Ease.InOutBounce);

		switch (attr)
		{
			case Attr.Red:
				if (_attr == Attr.Blue)
					value = value * 2f;
				break;
			case Attr.Blue:
				if (_attr == Attr.Green)
					value = value * 2f;
				break;
			case Attr.Green:
				if (_attr == Attr.Red)
					value = value * 2f;
				break;
		}

		CurHp -= value;
		SetHpBar();

		if (CurHp <= 0f)
			Die();

		return value;
	}

	void SetHpBar()
	{
		HpFill.fillAmount = (float)(CurHp / MaxHp);

		if (CurHp <= 0f)
			HpText.text = "0";
		else
			HpText.text = CurHp.ToCurrencyString();
	}

	void Die()
	{
		mat.DOFloat(1f, "_FadeAmount", 1.5f).SetEase(Ease.Linear);		
		DungeonManager.Ins.Win();
	}

}
