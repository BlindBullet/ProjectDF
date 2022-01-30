using System;
using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions
{
	public static bool AddRange<T>(this HashSet<T> source, IEnumerable<T> items)
	{
		bool allAdded = true;
		foreach (T item in items)
		{
			allAdded &= source.Add(item);
		}
		return allAdded;
	}

	public static void AddSafe<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV value = default(TV))
	{
		if (dict.ContainsKey(key))
		{
			return;
		}
		dict.Add(key, value);
	}


	public static KeyValuePair<TK, TV> Append<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV value = default(TV))
	{
		dict[key] = value;
		return new KeyValuePair<TK, TV>(key, value);
	}

	public static TV Get<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
	{
		TV res;
		return dict.TryGetValue(key, out res) ? res : defaultValue;
	}

	public static void SetDefault<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
	{
		if (!dict.ContainsKey(key))
		{
			dict.Add(key, defaultValue);
		}
		else
		{
			dict[key] = defaultValue;
		}
	}

	public static IDictionary<TK, TV> ConvinceKey<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default(TV))
	{
		if (!dict.ContainsKey(key))
		{
			dict.Add(key, defaultValue);
		}
		return dict;
	}

	public static bool TryGetValueWithSubStringKey<T>(this Dictionary<string, T> source, string key, out T value)
		where T : class
	{
		if (source.TryGetValue(key, out value))
		{
			return true;
		}
		foreach (KeyValuePair<string, T> kv in source)
		{
			// 키가 ""인 경우 위에서 검출 되었어야 한다.

			if (kv.Key.Length > 0 && key.ContainsIgnoreCase(kv.Key))
			{
				value = kv.Value;
				return true;
			}
		}
		value = default(T);
		return false;
	}

	public static KeyValuePair<TK, TV> FirstOrDefault<TK, TV>(this IDictionary<TK, TV> source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		foreach (KeyValuePair<TK, TV> pair in source)
		{
			return pair;
		}

		return default(KeyValuePair<TK, TV>);
	}

	public static KeyValuePair<TK, TV> FirstOrDefault<TK, TV>(this IDictionary<TK, TV> source,
		Func<KeyValuePair<TK, TV>, bool> predicate)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		foreach (KeyValuePair<TK, TV> pair in source)
		{
			if (predicate(pair))
			{
				return pair;
			}
		}

		return default(KeyValuePair<TK, TV>);
	}

	public static string ToRecognizable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
	{
		return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
	}
}