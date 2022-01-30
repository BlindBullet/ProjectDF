using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLerpColor: UILerpAnimationTemplate<Color>
{
	[SerializeField]
	private Color _from = Color.white;

	[SerializeField]
	private Color _to = Color.white;

	[SerializeField]
	private Image _target;

	protected override Color GetFrom(bool isPlayForward)
	{
		return _target.color;
	}

	protected override Color GetTo(bool isPlayForward)
	{
		return _to;
	}

	protected override void OnTransition(Color from, Color to, float alpha, bool isPlayForward)
	{
		if (isPlayForward)
		{
			_target.color = Color.Lerp(from, _to, alpha);
		}
		else
		{
			_target.color = Color.Lerp(from, _from, alpha);
		}
	}
}
