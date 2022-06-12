using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropSoulStone : DropIcon
{
	public override void Move()
	{
		Sequence seq = DOTween.Sequence();
		seq.Append(transform.DOMove(StageManager.Ins.TopBar.SoulStoneTrf.position.WithX(StageManager.Ins.TopBar.SoulStoneTrf.position.x + 0.2f), 1f).SetEase(Ease.InOutCubic))
			.AppendCallback(()=> { ObjectManager.Ins.Push<DropSoulStone>(this); });
	}

	public override void SetSfx()
	{
		SoundManager.Ins.PlaySFX("se_drop_soulstone");
	}
}
