using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlaneSubjectSingle<T> : SubjectSingleBase<T>
{
	public PlaneSubjectSingle(T init = default(T)) : base()
	{
		_value = init;
	}

	protected override void ValueChanged()
	{
		base.OnUpdate();
		return;
	}
}



public class PlaneSubjectList<T> : SubjectCollectionBase<T>, IList<T>
{
	public PlaneSubjectList()
	{
		_value = new List<T>();
	}

	IList<T> GetValue()
	{
		return (IList<T>)_value;
	}

	#region Implementation of IEnumerable

	public IEnumerator<T> GetEnumerator()
	{
		return _value.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	#endregion

	#region Implementation of ICollection<T>

	public void Add(T item)
	{
		GetValue().Add(item);
		ValueChanged();
	}

	public void Clear()
	{
		GetValue().Clear();
		ValueChanged();
	}

	public void RemoveRange(int index, int count)
	{
		for (var i = GetValue().Count - index + count; i > index; i--)
		{
			if (i < 0 || i >= GetValue().Count)
			{
				return;
			}
			GetValue().RemoveAt(i);
		}
	}

	public bool Contains(T item)
	{
		return GetValue().Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		GetValue().CopyTo(array, arrayIndex);
	}


	public bool Remove(T item)
	{
		if (GetValue().Remove(item))
		{
			ValueChanged();
			return true;
		}

		return false;
	}

	public int Count
	{
		get { return GetValue().Count; }
	}

	public bool IsReadOnly
	{
		get { return GetValue().IsReadOnly; }
	}

	#endregion

	#region Implementation of IList<T>

	public int IndexOf(T item)
	{
		return GetValue().IndexOf(item);
	}

	public void Insert(int index, T item)
	{
		GetValue().Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		GetValue().RemoveAt(index);
	}

	public T this[int index]
	{
		get { return GetValue()[index]; }
		set { GetValue()[index] = value; }
	}

	#endregion

	#region Your Added Stuff

	protected override void ValueChanged()
	{
		base.OnUpdate();
		return;
	}

	#endregion
}

public struct FancyKeyValuePair<K, T>
{
	public K Key { get; set; }
	public T Value { get; set; }
}

public class PlaneSubjectDict<TK, T> : SubjectCollectionBase<List<FancyKeyValuePair<TK, T>>>
{

	private int length = 2;
	private int count;

	public PlaneSubjectDict()
	{
		Val = new List<List<FancyKeyValuePair<TK, T>>>();
	}
	private int Hash(TK key)
	{
		return Math.Abs(key.GetHashCode()) % length;
	}

	private void IncreaseTable()
	{
		length = Val.Count() * 2;
		var newTable = new List<FancyKeyValuePair<TK, T>>[length];
		foreach (var row in Val)
		{
			if (row == null) continue;

			foreach (var cell in row)
			{
				var newHash = Hash(cell.Key);

				if (newTable[newHash] == null) newTable[newHash] = new List<FancyKeyValuePair<TK, T>>();

				newTable[newHash].Add(new FancyKeyValuePair<TK, T> { Key = cell.Key, Value = cell.Value });
			}
		}
		Val = newTable;
	}
	public int GetLongestList()
	{
		var largest = 0;
		foreach (var item in Val)
		{
			if (item != null && item.Count > largest) largest = item.Count;
		}

		return largest;
	}
	public int CountHashKeys()
	{
		var keys = 0;
		foreach (var item in Val)
		{
			if (item != null) keys++;
		}

		return keys;
	}
	public bool ContainsKey(TK key)
	{
		var hash = Hash(key);
		return Val[hash] != null && Val[hash].Any();
	}
	public object this[TK key]
	{
		get
		{
			var hash = Hash(key);
			var row = Val[hash];

			if (row == null) return null;

			foreach (var cell in row)
			{
				if (cell.Key.Equals(key)) return cell.Value;
			}

			return null;
		}
	}
	public void Add(TK key, T value)
	{
		count += 1;

		if (count > Val.Count) IncreaseTable();

		var hash = Hash(key);

		if (Val[hash] == null)
		{
			Val[hash] = new List<FancyKeyValuePair<TK, T>>();
		}

		Val[hash].Add(new FancyKeyValuePair<TK, T> { Key = key, Value = value });
	}
}