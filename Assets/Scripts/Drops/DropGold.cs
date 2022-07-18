using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropGold : DropIcon
{
	public override void Move()
	{
		Sequence seq = DOTween.Sequence();
		seq.Append(transform.DOMove(DropManager.Ins.GoldTrf.position, 1f).SetEase(Ease.InOutCubic))
			.AppendCallback(() => { this.gameObject.SetActive(false); });
	}

	public override void SetSfx()
	{
		
	}
	
}
