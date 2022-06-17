using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NextStageSequence : MonoBehaviour
{
	public RectTransform ObjRect;
	public TextMeshProUGUI StageText;

	public void NextStageSeq()
	{
		StageText.text = "STAGE" + StageManager.Ins.PlayerData.Stage.ToString();

		Sequence seq = DOTween.Sequence();
		seq.Append(ObjRect.DOAnchorPosX(0f, 0.5f).SetEase(Ease.InOutQuad))
			.AppendCallback(() => SoundManager.Ins.PlaySFX("se_enchant_hero_2"))
			.AppendInterval(1f)
			.Append(ObjRect.DOAnchorPosX(900f, 0.5f).SetEase(Ease.InOutQuad))
			.AppendCallback(()=> ObjRect.anchoredPosition = new Vector2(-900f, ObjRect.anchoredPosition.y));
	}

}
