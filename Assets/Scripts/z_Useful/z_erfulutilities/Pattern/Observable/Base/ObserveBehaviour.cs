using System;
using DUtil.ObservingBehaviourInternal;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace DUtil.ObservingBehaviourInternal
{
	using UnityEngine;

	public class ObserveBehaviourCore : MonoBehaviour
	{
		private static ObjectManager _poolManager;

		protected T Pop<T>(T spearPrefab) where T : Component
		{
			if (_poolManager == null)
			{
				_poolManager = ObjectManager.Ins;
			}
			return _poolManager.Pop(spearPrefab);
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void Update()
		{
		}
	}
}

public class ObserveBehaviour : ObserveBehaviourCore
{
	private List<IMonoSubject> _updateRequiredSubjects = new List<IMonoSubject>();
	private List<IMonoSubject> _plainSubscriptions = new List<IMonoSubject>();
	protected Dictionary<UnityEventBase, Delegate> _eventToAction = new Dictionary<UnityEventBase, Delegate>();

	public void Blink()
	{
		foreach (var elem in _plainSubscriptions)
		{
			elem.OnUpdate();
		}
		foreach (var elem in _updateRequiredSubjects)
		{
			elem.OnUpdate();
		}
	}

	//protected void WeakSubscribe(UnityEvent inEvent, UnityAction inAction)
	//{
	//	if (inEvent == null || inAction == null)
	//	{
	//		return;
	//	}

	//	_eventToAction.Add(inEvent, inAction);
	//	inEvent.AddListener(inAction);
	//}

	//protected void WeakSubscribe<T0>(UnityEvent<T0> inEvent, UnityAction<T0> inAction)
	//{
	//	if (inEvent == null || inAction == null)
	//	{
	//		return;
	//	}

	//	_eventToAction.Add(inEvent, inAction);
	//	inEvent.AddListener(inAction);
	//}


	//protected void WeakSubscribe<T0, T1>(UnityEvent<T0, T1> inEvent, UnityAction<T0, T1> inAction)
	//{
	//	if (inEvent == null || inAction == null)
	//	{
	//		return;
	//	}

	//	_eventToAction.Add(inEvent, inAction);
	//	inEvent.AddListener(inAction);
	//}

	//protected void WeakSubscribe<T0, T1, T2>(UnityEvent<T0, T1, T2> inEvent, UnityAction<T0, T1, T2> inAction)
	//{
	//	if (inEvent == null || inAction == null)
	//	{
	//		return;
	//	}

	//	_eventToAction.Add(inEvent, inAction);
	//	inEvent.AddListener(inAction);
	//}


	protected ObserveBehaviour Bind<T>([NotNull] PlaneSubjectSingle<T> data, [NotNull] SubjectSingleBase<T>.Handler callback)
	{
		_plainSubscriptions.Add(data);
		data.Subscribe(this, callback);
		callback(this, data.Val);
		return this;
	}

	protected ObserveBehaviour Bind<T>([NotNull] PlaneSubjectList<T> data, [NotNull] SubjectCollectionBase<T>.Handler listCallback, SubjectSingleBase<T>.Handler dataDirtyCallback = null)
	{
		_plainSubscriptions.Add(data);
		data.Subscribe(this, listCallback, dataDirtyCallback);
		listCallback(data.Val);
		return this;
	}

	protected ObserveBehaviour Bind<T>([NotNull] ThrottleSubjectSingle<T> data, [NotNull] SubjectSingleBase<T>.Handler callback, bool IsValueChanged = true)
	{
		_updateRequiredSubjects.Add(data);
		data.Subscribe(this, callback);
		callback(this, data.Val);
		return this;
	}

	protected ObserveBehaviour Bind<T>([NotNull] UpdatingSubjectSingle<T> data, [NotNull] SubjectSingleBase<T>.Handler callback, bool IsValueChanged = true)
	{
		_updateRequiredSubjects.Add(data);
		data.Subscribe(this, callback);
		callback(this, data.Val);
		return this;
	}

	protected virtual void OnDisabled()
	{
	}
	// OnDisabled를 대신 사용할 것
	protected sealed override void OnDisable()
	{
		CacnelAllBroadcasts();
		//UnsubscribeUnityEvents();
		OnDisabled();
	}


	// OnUpdated를 대신 사용할 것
	protected sealed override void Update()
	{
		for (int i = 0; i < _updateRequiredSubjects.Count; i++)
		{
			_updateRequiredSubjects[i].OnUpdate();
		}
		OnUpdated();
	}

	protected virtual void OnUpdated()
	{
	}

	protected void CacnelAllBroadcasts()
	{
		CancelBroadcast(_plainSubscriptions);
		CancelBroadcast(_updateRequiredSubjects);
	}

	//private void UnsubscribeUnityEvents()
	//{
	//	foreach(var elem in _eventToAction)
	//	{
	//		elem.Key.RemoveListener(elem.Value);
	//	}
	//	_eventToAction.Clear();
	//}

	private void CancelBroadcast(List<IMonoSubject> observers)
	{
		for (int i = 0; i < observers.Count; i++)
		{
			observers[i].UnsubscribeAll();
		}
		observers.Clear();
	}
}