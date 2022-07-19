using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropSoulStone : DropIcon
{
	public override void Move()
	{
		Sequence seq = DOTween.Sequence();
		seq.Append(transform.DOMove(DropManager.Ins.SoulStoneTrf.position, 1f).SetEase(Ease.InOutCubic))
			.AppendCallback(()=> { this.gameObject.SetActive(false); });
	}

	public override void SetSfx()
	{
		SoundManager.Ins.PlaySFX("se_drop_soulstone");
	}
}
