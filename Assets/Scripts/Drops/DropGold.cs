using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropGold : DropIcon
{
	public override void Move()
	{
		Sequence seq = DOTween.Sequence();
		seq.Append(transform.DOMove(StageManager.Ins.TopBar.GoldTrf.position.WithX(StageManager.Ins.TopBar.GoldTrf.position.x + 0.2f), 1f).SetEase(Ease.InOutCubic))
			.AppendCallback(() => { ObjectManager.Ins.Push<DropGold>(this); });
	}

	public override void SetSfx()
	{
		
	}
}
