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
	public TextMeshProUGUI HpText;
	public Image HpFill;
	Attr attr;
	Material mat;

	public void SetEnemy(DungeonChart chart)
	{
		EnemySprite.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.AppearEnemy);
		mat = EnemySprite.material;
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

	public void TakeDmg(double value, Attr _attr)
	{
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
	}

	void SetHpBar()
	{
		HpFill.fillAmount = (float)(CurHp / MaxHp);
		HpText.text = CurHp.ToCurrencyString();
	}

	void Die()
	{

	}

}
