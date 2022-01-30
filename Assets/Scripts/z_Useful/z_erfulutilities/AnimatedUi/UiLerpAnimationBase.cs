using System.Collections;
using UnityEngine;
using DUtils;
using System;

namespace UiLerpAnimationInternal
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class UiLerpAnimationBase : MonoBehaviour
	{
		protected RectTransform _rt;

		[SerializeField]
		protected float AniTime = 0.7f;

		[SerializeField]
		protected AnimationCurve Curve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 4.85f, 4.85f),
			new Keyframe(0.15f, 0.729f, 1.489f, 1.160f),
			new Keyframe(1f, 1f, 0f, 0f),
		});

		protected bool _isInitalized;

		protected int _enabledFrame;

		public bool _autoPlay;

		public virtual void Play(bool playForward, bool skipAnimation)
		{
			OnPlay(playForward, skipAnimation);
		}

		protected virtual void Awake()
		{
			if(_rt == null)
			{
				_rt = GetComponent<RectTransform>();
			}
			_isInitalized = true;
		}

		protected virtual void OnEnable()
		{
			_enabledFrame = Time.frameCount;
			if (_autoPlay)
			{
				Play(true, false);
			}
		}

		protected abstract void OnPlay(bool playForward, bool skipAnimation);

	}
}

public abstract class UILerpAnimationTemplate<T> : UiLerpAnimationInternal.UiLerpAnimationBase
{
	private ICoroutineBinder _sequence;

	protected virtual void OnSequenceStart(bool isPlayForward)
	{

	}

	protected virtual void OnBegin(T from, bool isPlayForward)
	{

	}

	protected abstract void OnTransition(T from, T to, float alpha, bool isPlayForward);

	protected virtual void OnEnd(T to, bool isPlayForward)
	{

	}

	protected virtual void OnInterrupted()
	{

	}

	protected abstract T GetFrom(bool isPlayForward);

	protected abstract T GetTo(bool isPlayForward);

	protected sealed override void OnPlay(bool isPlayForward, bool skipAnimation)
	{
		if (_enabledFrame == Time.frameCount || !skipAnimation)
		{
			this.StartCoroutine(ref this._sequence, PlaySequence(isPlayForward, skipAnimation, _enabledFrame == Time.frameCount));
		}
		else if (skipAnimation)
		{
			ForceSet(isPlayForward);
		}

	}

	void ForceSet(bool isPlayForward)
	{

		T from = GetFrom(isPlayForward);
		T to = GetTo(isPlayForward);

		OnSequenceStart(isPlayForward);
		OnBegin(from, isPlayForward);
		OnTransition(from, to, 1f, isPlayForward);
		OnEnd(isPlayForward ? GetTo(isPlayForward) : GetFrom(isPlayForward), isPlayForward);
	}

	private IEnumerator PlaySequence(bool isPlayForward, bool skipAnimation, bool delayOneFrame)
	{
		if (delayOneFrame)
		{
			yield return null;
		}

		if (skipAnimation)
		{
			ForceSet(isPlayForward);
			yield break;
		}

		OnSequenceStart(isPlayForward);

		T from = GetFrom(isPlayForward);
		T to = GetTo(isPlayForward);

		OnBegin(from, isPlayForward);

		

		for (float time = 0; time < AniTime; time += Time.deltaTime)
		{
			OnTransition(from, to, Curve.Evaluate(time / AniTime), isPlayForward);
			yield return null;
		}
		OnTransition(from, to, 1f, isPlayForward);

		OnEnd(to, isPlayForward);
	}
}