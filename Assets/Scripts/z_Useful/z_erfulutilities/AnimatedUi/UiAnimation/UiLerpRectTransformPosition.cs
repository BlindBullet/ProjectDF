using System;
using UnityEngine;

public class UiLerpRectTransformPosition : UILerpAnimationTemplate<Vector3>
{
	[SerializeField]
	public Vector3 _offset;

	[SerializeField]
	private RectTransform _target;

	private Vector3 _sourceCache;

	protected override void OnSequenceStart(bool isPlayForward)
	{
		_sourceCache = (_target? _target.GetAnchoredButtom().WithZ(0f)  : _rt.anchoredPosition.WithZ(0f))
			 + _offset;
	}

	protected override Vector3 GetFrom(bool isPlayForward)
	{
		return _sourceCache;
	}

	protected override Vector3 GetTo(bool isPlayForward)
	{
		return _target.GetAnchoredButtom().WithZ(0f) + _offset;
	}

	protected override void OnTransition(Vector3 from, Vector3 to, float alpha, bool isForward)
	{
		_rt.anchoredPosition = Vector3.Lerp(from, to, alpha);
	}

	public void SetTarget(RectTransform target)
	{
		_target = target;
	}
}