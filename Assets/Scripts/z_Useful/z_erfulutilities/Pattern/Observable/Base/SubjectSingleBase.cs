using System.Collections.Generic;
using UnityEngine;

public interface IMonoSubject
{
	void UnsubscribeAll();

	void OnUpdate();
}

public abstract class SubjectBase<T> : IMonoSubject
{
	public virtual void UnsubscribeAll()
	{
	}

	public virtual void OnUpdate()
	{
	}

	protected virtual void ValueChanged()
	{
	}
}

public abstract class SubjectSingleBase<T> : SubjectBase<T>
{
	protected static readonly EqualityComparer<T> Comparer = EqualityComparer<T>.Default;
	protected T _value;
	protected Dictionary<ObserveBehaviour, List<Handler>> subscribers = new Dictionary<ObserveBehaviour, List<Handler>>();

	public delegate void Handler(ObserveBehaviour owner, T changedVal);

	public virtual T Val
	{
		get { return _value; }

		set
		{
			if (Comparer.Equals(_value, value))
			{
				return;
			}
			_value = value;
			ValueChanged();
		}
	}

	public override void OnUpdate()
	{
		using (Reusable<List<ObserveBehaviour>> reusable = ReusableList<ObserveBehaviour>.Pop())
		{
			List<ObserveBehaviour> itemsToRemove = reusable;

			foreach (KeyValuePair<ObserveBehaviour, List<Handler>> elem in subscribers)
			{
				//Exclusion Case : pick disabled objects
				if ((elem.Key == null) || !elem.Key.gameObject.activeInHierarchy)
				{
					itemsToRemove.Add(elem.Key);
					continue;
				}

				//Send Event
				for (int i = elem.Value.Count - 1; i >= 0; --i)
				{
					elem.Value[i].Invoke(elem.Key, _value);
				}
			}

			//remove disable or dead objects(subscribers)
			for (int i = itemsToRemove.Count - 1; i >= 0; --i)
			{
				subscribers.Remove(itemsToRemove[i]);
			}
		}
	}

	public virtual void Subscribe(ObserveBehaviour obj, Handler callback)
	{
		List<Handler> list = subscribers.Get(obj);
		if (list == null)
		{
			list = new List<Handler>();
			subscribers.Add(obj, list);
		}
		list.Add(callback);
	}

	public virtual void Unsubscribe(ObserveBehaviour obj)
	{
		List<Handler> list = subscribers.Get(obj);
		list?.Clear();
	}

	public override void UnsubscribeAll()
	{
		subscribers.Clear();
	}
}

public abstract class SubjectCollectionBase<T> : SubjectBase<T>
{
	//Note : SubjectCollection과 SubjectBase를 합치려고 해 보았으나
	// 가장 핵심인 Handler를 generic으로 만들 수 없었다. (이중콜백도 가능하긴 하나 안떙김)
	// 적어도 코드는 똑같으니 복붙이나 하자


	public class HandlerSet
	{
		public List<Handler> CollectionHandlers = new List<Handler>();
		public List<SubjectSingleBase<T>.Handler> DirtyItemHandlers = new List<SubjectSingleBase<T>.Handler>();

	}

	protected static readonly MultiSetComparer<T> Comparer = new MultiSetComparer<T>();
	protected IList<T> _value;
	protected Dictionary<ObserveBehaviour, HandlerSet> subscribers = new Dictionary<ObserveBehaviour, HandlerSet>();


	public delegate void Handler(IEnumerable<T> changedValue);

	public IList<T> Val
	{
		get { return (IList<T>)_value; }
		set
		{
			if (Comparer.Equals(_value, value))
			{
				return;
			}
			_value = value;
			ValueChanged();
		}
	}

	public override void OnUpdate()
	{
		using (Reusable<List<ObserveBehaviour>> reusable = ReusableList<ObserveBehaviour>.Pop())
		{
			List<ObserveBehaviour> itemsToRemove = reusable;

			foreach (KeyValuePair<ObserveBehaviour, HandlerSet> elem in subscribers)
			{
				//Exclusion Case : pick disabled objects
				if ((elem.Key == null) || !elem.Key.gameObject.activeInHierarchy)
				{
					itemsToRemove.Add(elem.Key);
					continue;
				}

				//Send Event
				for (var i = elem.Value.CollectionHandlers.Count - 1; i >= 0; --i)
				{
					elem.Value.CollectionHandlers[i].Invoke(_value);
				}
			}

			//remove disable or dead objects(subscribers)
			for (var i = itemsToRemove.Count - 1; i >= 0; --i)
			{
				subscribers.Remove(itemsToRemove[i]);
			}
		}
	}

	public virtual void Subscribe(ObserveBehaviour obj, Handler listModifyCallback, SubjectSingleBase<T>.Handler itemDirtyCallback)
	{
		var list = subscribers.Get(obj);
		if (list == null)
		{
			list = new HandlerSet();
			subscribers.Add(obj, list);
		}

		if (listModifyCallback != null)
		{
			list.CollectionHandlers.Add(listModifyCallback);
		}

		if (itemDirtyCallback != null)
		{
			list.DirtyItemHandlers.Add(itemDirtyCallback);
		}
	}

	public virtual void Unsubscribe(ObserveBehaviour obj)
	{
		var list = subscribers.Get(obj);
		list?.CollectionHandlers.Clear();
		list?.DirtyItemHandlers.Clear();
	}

	public override void UnsubscribeAll()
	{
		subscribers.Clear();
	}

	public void NotifyDirty(T targetIntention)
	{
		foreach (KeyValuePair<ObserveBehaviour, HandlerSet> elem in subscribers)
		{
			for (var i = elem.Value.DirtyItemHandlers.Count - 1; i >= 0; --i)
			{
				elem.Value.DirtyItemHandlers[i].Invoke(elem.Key, targetIntention);
			}
		}

	}
}