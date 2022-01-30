using DUtil.CoroutineBinderHelper;
using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

namespace DUtil.CoroutineBinderHelper
{
	public class EnumeratorCoroutineBinder : ICoroutineBinder
	{
		public IEnumerator CachedIenumerator;
		private readonly MonoBehaviour _coroutineOwner;

		public EnumeratorCoroutineBinder(MonoBehaviour owner)
		{
			_coroutineOwner = owner;
		}

		public void Stop()
		{
			if (CachedIenumerator != null)
			{
				_coroutineOwner.StopCoroutine(CachedIenumerator);
				CachedIenumerator = null;
			}
		}

		public bool TryReapply(IEnumerator newCoroutine)
		{
			if (_coroutineOwner == null)
			{
				Debug.LogWarning("Object already destroyed");
				return false;
			}
			Stop();
			CachedIenumerator = newCoroutine;
			_coroutineOwner.StartCoroutine(newCoroutine);
			return true;
		}
	}

	public class CoroutineMananger : SingletonObject<CoroutineMananger>
	{
	}
}

namespace DUtil.CoroutineBinderHelper
{
	public class EnumerableCoroutineBinder : ICoroutineBinder
	{
		public IEnumerable Ienumerable;
		public IEnumerator Sequence;
		public Action Exited;
		private readonly MonoBehaviour _coroutineOwner;

		public EnumerableCoroutineBinder(MonoBehaviour owner)
		{
			_coroutineOwner = owner;
		}

		public void Stop()
		{
			if (Sequence != null && Sequence.Current != null)
			{
				_coroutineOwner.StopCoroutine(Sequence);
				Sequence = null;
				Exited?.Invoke();
			}
		}
	}
}

public interface ICoroutineBinder
{

}

public class WaitOneFrame : CustomYieldInstruction
{
	private int _frame;

	public override bool keepWaiting
	{
		get
		{
			return _frame == Time.frameCount;
		}
	}

	public WaitOneFrame()
	{
		_frame = Time.frameCount;
	}
}

public static class UniqueCoroutineExtension
{
	/// <summary>
	/// 코루틴을 캐시하여, 하나의 코루틴만 있도록 유지하고 원하는 타이밍에 중지할 수 있도록 합니다.
	/// </summary>
	public static Coroutine StartCoroutine(this MonoBehaviour owner, ref ICoroutineBinder binder, [NotNull] IEnumerable coroutine, Action Interuppted = null)
	{
		if (owner == null)
		{
			Debug.LogWarning("Object already destroyed");
			return null;
		}

		if (binder == null)
		{
			binder = new EnumerableCoroutineBinder(owner);
		}

		var safeCoroutineBinder = (EnumerableCoroutineBinder)binder;

		safeCoroutineBinder.Stop();
		safeCoroutineBinder.Ienumerable = coroutine;
		safeCoroutineBinder.Sequence = coroutine.GetEnumerator();
		safeCoroutineBinder.Exited = Interuppted;
		return owner.StartCoroutine(safeCoroutineBinder.Sequence);
	}

	/// <summary>
	/// 코루틴을 캐시하여, 하나의 코루틴만 있도록 유지하고 원하는 타이밍에 중지할 수 있도록 합니다.
	/// </summary>
	public static Coroutine StartCoroutine(this MonoBehaviour owner, ref ICoroutineBinder binder,
		[NotNull] IEnumerator coroutine)
	{
		if (owner == null)
		{
			Debug.LogWarning("Object already destroyed");
			return null;
		}

		if (binder == null)
		{
			binder = new EnumeratorCoroutineBinder(owner);
		}

		var safeCoroutineBinder = (EnumeratorCoroutineBinder)binder;

		safeCoroutineBinder.Stop();
		safeCoroutineBinder.CachedIenumerator = coroutine;
		return owner.StartCoroutine(safeCoroutineBinder.CachedIenumerator);
	}

	/// <summary>
	/// 코루틴이 정지되어 있으면 재실행합니다.
	/// </summary>
	public static Coroutine SustainCoroutine(this MonoBehaviour owner, ref ICoroutineBinder binder,
		[NotNull] IEnumerator coroutine)
	{
		if (owner == null)
		{
			Debug.LogWarning("Object already destroyed");
			return null;
		}

		if (binder == null)
		{
			binder = new EnumeratorCoroutineBinder(owner);
		}

		var safeCoroutineBinder = (EnumeratorCoroutineBinder)binder;

		safeCoroutineBinder.Stop();
		safeCoroutineBinder.CachedIenumerator = coroutine;
		return StartCoroutine(owner, ref binder, coroutine);
	}

	public static Coroutine StartCoroutine([CanBeNull] ref ICoroutineBinder binder, [NotNull] IEnumerator coroutine)
	{
		return StartCoroutine(CoroutineMananger.Ins, ref binder, coroutine);
	}

	public static bool CheckStoppedOrNull(this ICoroutineBinder binder)
	{
		if (binder == null)
		{
			return true;
		}
		var cBinder = ((EnumerableCoroutineBinder)binder);
		if (cBinder.Sequence == null || cBinder.Sequence.Current == null)
		{
			return true;
		}
		return false;
	}

	public static void StopCoroutine(this MonoBehaviour owner, ICoroutineBinder binder)
	{
		if (binder is EnumeratorCoroutineBinder)
		{
			((EnumeratorCoroutineBinder)binder).Stop();
			return;
		}
		else if (binder is EnumerableCoroutineBinder)
		{
			((EnumerableCoroutineBinder)binder).Stop();
			return;
		}

	}
	
	public static CustomYieldInstruction NULL(this MonoBehaviour owner)
	{
		return new WaitOneFrame();
	}
}