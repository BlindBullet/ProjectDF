using System;
using System.Collections.Generic;
using UnityEngine;

public class SingletonObject<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static bool ApplicationIsQuitting = false;
	private static T _instance;

	private static object _lock = new object();

	private static Dictionary<Type, T> _singletonObjects
		= new Dictionary<Type, T>();

	private bool _isInitilized = false;
	private bool _isSceneObjectDestroyed = false;

	public static T Ins
	{
		get
		{
			if (ApplicationIsQuitting)
			{
				Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}

			lock (_lock)
			{
				if (_instance == null)
				{
					if (_singletonObjects.ContainsKey(typeof(T)) && _singletonObjects[typeof(T)] != null)
					{
						_instance = _singletonObjects[typeof(T)];
						var temp = _instance as SingletonObject<T>;
						temp.TryInitilize();
					}
					else
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = "(singleton) " + typeof(T).ToString();
						DontDestroyOnLoad(singleton);
						_singletonObjects[typeof(T)] = _instance;

						var temp = _instance as SingletonObject<T>;
						temp.TryInitilize();
					}
				}
				return _instance;
			}
		}
	}

	protected virtual void Start()
	{
		if (!_singletonObjects.ContainsKey(typeof(T)) || _singletonObjects[typeof(T)] != this)
		{
			_isSceneObjectDestroyed = true;
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// When Unity quits, it destroys objects in a random order. In principle, a Singleton is only
	/// destroyed when application quits. If any script calls Instance after it have been destroyed,
	/// it will create a buggy ghost object that will stay on the Editor scene even after stopping
	/// playing the Application. Really bad! So, this was made to be sure we're not creating that
	/// buggy ghost object.
	/// </summary>
	protected void OnDestroy()
	{
		if (!_isSceneObjectDestroyed)
		{
			ApplicationIsQuitting = true;
		}
	}

	protected virtual void OnCreate()
	{
	}

	private void TryInitilize()
	{
		if (_isInitilized)
		{
			return;
		}
		_isInitilized = true;
		OnCreate();
	}
}