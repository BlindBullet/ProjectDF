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
        originPos = Icon.anchoredPosition;
    }

    public void Attack(float spd)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Icon.DOAnchorPosY(Icon.anchoredPosition.y + 20f, 0.1f / spd).SetEase(Ease.OutQuad))
            .Append(Icon.DOAnchorPosY(originPos.y, 0.1f / spd));        
    }

    
}
