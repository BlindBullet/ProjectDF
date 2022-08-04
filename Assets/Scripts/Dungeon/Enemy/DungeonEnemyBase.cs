using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;

public class DungeonEnemyBase : MonoBehaviour
{	
	public SpriteRenderer EnemySprite;

	public void SetEnemy(DungeonChart chart)
	{
		EnemySprite.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.AppearEnemy);
		Appear();
	}

	void Appear()
	{
		this.transform.DOMoveY(4.8f, 1f).SetEase(Ease.InOutQuad);
	}

}
