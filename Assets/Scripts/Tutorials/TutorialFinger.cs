using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialFinger : MonoBehaviour
{
	public Transform FingerTrf;

	private void Start()
	{
		FingerTrf.DOMoveY(FingerTrf.position.y + 0.15f, 0.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
	}

	public void SetPos(Vector2 pos, Vector2 dir)
	{
		this.transform.position = pos;		
	}

	

}
