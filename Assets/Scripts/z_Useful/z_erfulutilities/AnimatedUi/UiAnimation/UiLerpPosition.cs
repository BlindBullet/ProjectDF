using System;
using UnityEngine;

public class UiLerpPosition : UILerpAnimationTemplate<Vector3>
{
	[SerializeField]
	public Vector3 _offset;

	[SerializeField]
	private Transform _target;

	private Vector3 _sourceCache;

	protected override void OnSequenceStart(bool isPlayForward)
	{
		_sourceCache = transform.position;
	}

	protected override Vector3 GetFrom(bool isPlayForward)
	{
		return _sourceCache;
	}

	protected override Vector3 GetTo(bool isPlayForward)
	{
		return _target.transform.position + _offset;
	}

	protected override void OnTransition(Vector3 from, Vector3 to, float alpha, bool isForward)
	{
		transform.position = Vector3.Lerp(from, to, alpha);
	}

	public void SetTarget(Transform target)
	{
		_target = target;
	}
}