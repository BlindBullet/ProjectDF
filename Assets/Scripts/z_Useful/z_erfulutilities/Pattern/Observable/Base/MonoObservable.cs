using System.Collections.Generic;
using UnityEngine;

public interface IMonoObservable
{
	void UnsubscribeAll();

	void Notify();
}

public abstract class ObservableBase<T> : IMonoObservable
{
	private static readonly EqualityComparer<T> Comparer = EqualityComparer<T>.Default;

	protected T _value;

	protected Dictionary<GameObject, List<Handler>> _subscribers;

	public T Val
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

	public ObservableBase()
	{
		_subscribers = new Dictionary<GameObject, List<Handler>>();
	}

	public delegate void Handler(T newState);

	protected abstract void ValueChanged();

	public virtual void Notify()
	{
		using (Reusable<List<GameObject>> reusable = ReusableList<GameObject>.Pop())
		{
			List<GameObject> itemsToRemove = reusable;

			foreach (KeyValuePair<GameObject, List<Handler>> elem in _subscribers)
			{
				//Exclusion Case : pick disabled objects
				if ((elem.Key == null) || !elem.Key.activeInHierarchy)
				{
					itemsToRemove.Add(elem.Key);
					continue;
				}

				//Send Event
				for (int i = elem.Value.Count - 1; i >= 0; --i)
				{
					elem.Value[i](_value);
				}
			}

			//remove disable or dead objects(subscribers)
			for (int i = itemsToRemove.Count - 1; i >= 0; --i)
			{
				_subscribers.Remove(itemsToRemove[i]);
			}
		}
	}

	public void Subscribe(GameObject obj, Handler callback)
	{
		List<Handler> list = _subscribers.Get(obj);
		if (list == null)
		{
			list = new List<Handler>();
			_subscribers.Add(obj, list);
		}
		list.Add(callback);
	}

	public void Unsubscribe(GameObject obj, Handler callback)
	{
		List<Handler> list = _subscribers.Get(obj);
		if (list != null)
		{
			list.Remove(callback);
		}
	}

	public void Unsubscribe(GameObject obj)
	{
		List<Handler> list = _subscribers.Get(obj);
		if (list != null)
		{
			list.Clear();
		}
	}

	public void UnsubscribeAll()
	{
		_subscribers.Clear();
	}
}
