using DUtil.ObservingBehaviourInternal;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace DUtil.ObservingBehaviourInternal
{
	using UnityEngine;

	public class ObservingBehaviourCore : MonoBehaviour
	{
		protected virtual void OnDisable()
		{
		}

		protected virtual void Update()
		{
		}
	}
}

public class ObservingBehaviour : ObservingBehaviourCore
{
	private List<IMonoObservable> _everyUpdateSubscriptions = new List<IMonoObservable>();
	private List<IMonoObservable> _plainSubscriptions = new List<IMonoObservable>();

	protected ObservingBehaviour Bind<T>([NotNull] PlainObservable<T> data, [NotNull] ObservableBase<T>.Handler callback)
	{
		_plainSubscriptions.Add(data);
		data.Subscribe(gameObject, callback);
		callback(data.Val);
		return this;
	}

	protected ObservingBehaviour Bind<T>([NotNull] UpdateObservable<T> data, [NotNull] ObservableBase<T>.Handler callback)
	{
		_everyUpdateSubscriptions.Add(data);
		data.Subscribe(gameObject, callback);
		callback(data.Val);
		return this;
	}

	protected virtual void OnDisabled()
	{
	}

	protected sealed override void OnDisable()
	{
		CacnelAllBroadcast();
		OnDisabled();
	}

	protected sealed override void Update()
	{
		for (int i = 0; i < _everyUpdateSubscriptions.Count; i++)
		{
			_everyUpdateSubscriptions[i].Notify();
		}
		OnUpdate();
	}

	protected virtual void OnUpdate()
	{
	}

	protected void CacnelAllBroadcast()
	{
		CancelBroadcast(_plainSubscriptions);
		CancelBroadcast(_everyUpdateSubscriptions);
	}

	private void CancelBroadcast(List<IMonoObservable> observers)
	{
		for (int i = 0; i < observers.Count; i++)
		{
			observers[i].UnsubscribeAll();
		}
		observers.Clear();
	}
}