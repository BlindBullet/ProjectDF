//[Created by Tae-Han on 2017-01-20.]

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//pooling system O(1).
public static class PoolHelperExtension
{

	public static T PopFromPool<T>(this T obj) where T : Component
	{
		if (obj == null)
		{
			return null;
		}
		return ObjectManager.Ins.Pop(obj);
	}

	public static void PushToPool(this Component obj)
	{
		ObjectManager.Ins.Push(obj);
	}

	/// <summary>
	/// 모든 오브젝트를 풀로 되돌린다. Pop으로 만든 오브젝트가 아니면 Destory한다
	/// </summary>
	public static void PushAll<T>(this IList<T> objs) where T : Component
	{
		foreach (var obj in objs)
		{
			ObjectManager.Ins.Push(obj);
		}
		objs.Clear();
	}

	public static void PushAll<VK, VT>(this IDictionary<VK, VT> objs) where VT : Component
	{
		foreach (var obj in objs)
		{
			ObjectManager.Ins.Push(obj.Value);
		}
		objs.Clear();
	}
}

public class ObjectManager : SingletonObject<ObjectManager>
{
	private Dictionary<int, int> _instanceToPrefabId = new Dictionary<int, int>();
	private Dictionary<int, PoolCollection> _poolGrave = new Dictionary<int, PoolCollection>();

	private const string AudioSourceName = "AudioSource";
	private Stack<AudioSource> _audios = new Stack<AudioSource>();

	public T Pop<T>([NotNull] GameObject prefab, bool isActive = true) where T : Component
	{
		int prefabId = prefab.GetInstanceID();
		if (!_poolGrave.ContainsKey(prefabId))
		{
			_poolGrave[prefabId] = new PoolCollection(prefab.GetComponent<T>(), null);
		}
		T res = null;
		while (res == null)
		{
			res = _poolGrave[prefabId].Pop<T>();
		}
		_instanceToPrefabId[res.GetInstanceID()] = prefabId;
		res.gameObject.SetActive(isActive);
		return res;
	}

	public T Pop<T>([NotNull] T prefab, bool isActive = true) where T : Component
	{
		int prefabId = prefab.GetInstanceID();
		if (!_poolGrave.ContainsKey(prefabId))
		{
			_poolGrave[prefabId] = new PoolCollection(prefab, null);
		}
		T res = null;
		while (res == null)
		{
			res = _poolGrave[prefabId].Pop<T>();
		}
		_instanceToPrefabId[res.GetInstanceID()] = prefabId;
		res.gameObject.SetActive(isActive);
		return res;
	}

	public GameObject Pop([NotNull] GameObject prefab, bool isActive = true)
	{
		return Pop(prefab.transform, isActive).gameObject;
	}

	public void Push<T>([NotNull] T obj) where T : Component
	{
		if (ApplicationIsQuitting || obj == null)
		{
			return;
		}
		obj.gameObject.SetActive(false);
		var objId = obj.GetInstanceID();
		if (!_instanceToPrefabId.ContainsKey(obj.GetInstanceID()))
		{
			Destroy(obj);
			return;
		}
		else
		{
			var prefabId = _instanceToPrefabId[objId];
			_poolGrave[prefabId].Push(obj);
		}
	}

	public void PushAfter<T>(T obj, float duration) where T : Component
	{
		if (ApplicationIsQuitting)
		{
			return;
		}
		StartCoroutine(DelayedPushSequence(obj, duration));
	}

	IEnumerator DelayedPushSequence<T>(T obj, float dur) where T : Component
	{
		yield return new WaitForSeconds(dur);
		Push(obj);
	}

	public void Create<T>(T prefab) where T : Component
	{
		var id = prefab.GetInstanceID();
		_instanceToPrefabId.ConvinceKey(id, id);
		if (!_poolGrave.ContainsKey(id))
		{
			_poolGrave[id] = new PoolCollection(prefab, null);
		}
		Push(prefab);
	}

	public class PoolCollection
	{
		public Transform _holder;

		public Component _prefab;

		private HashSet<Component> _alivePool = new HashSet<Component>();

		private Stack<Component> _deadPool = new Stack<Component>();

		private PoolExceededActions _poolLimitHandle;

		public PoolCollection(int prefabInstanceId, Transform holder, PoolExceededActions type)
		{
			_instanceId = prefabInstanceId;
			_holder = holder;
			_poolLimitHandle = type;
		}

		public PoolCollection(Component prefab, Transform holder)
		{
			_prefab = prefab;
			_instanceId = prefab.gameObject.GetInstanceID();
			_holder = holder;
		}

		private PoolCollection()
		{
		}

		public enum PoolExceededActions
		{
			None, KillOld, Block,
		}

		public int _instanceId { get; private set; }

		public int TotalCount => _deadPool.Count + _alivePool.Count;

		public void CreateDead<T>(T prefab) where T : Component
		{
			var item = Instantiate(prefab, _holder, false);
			item.gameObject.SetActive(false);
			_deadPool.Push(item);
		}

		public T CreateAlive<T>(T prefab) where T : Component
		{
			var item = Instantiate(prefab, _holder, false);
			_alivePool.Add(item);
			return item;
		}

		public T Pop<T>() where T : Component
		{
			T res = null;
			if (_deadPool.Count > 0)
			{
				res = (T)_deadPool.Pop();
			}
			else
			{
				res = (T)CreateAlive(_prefab);
			}
			return res;
		}

		public void Push<T>(T obj) where T : Component
		{
			_deadPool.Push(obj);
		}
	}
}