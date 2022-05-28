using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class HeroTween : MonoBehaviour
{
	public RectTransform Icon;
	public RectTransform HeroImg;    
	Vector2 originPos;

	public void SetTween()
	{
		originPos = Icon.transform.position;        
	}

	public void Attack(float spd, EnemyBase target)
	{
		Vector3 dir = (target.transform.position - Icon.position).normalized;
		
		Sequence seq = DOTween.Sequence();
		seq.Append(Icon.DOMove(new Vector2(originPos.x + (dir.x / 5f), originPos.y + (dir.y / 5f)), 0.1f / spd).SetEase(Ease.OutQuad))
			.Append(Icon.DOMove(originPos, 0.1f / spd));
	}

	public void Stop()
	{
		DOTween.KillAll();
	}

	
}
