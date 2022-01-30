using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ListExtensionMethod
{
	public static int GetSize<T>(this IList<T> source)
	{
		return ListExtension.GetSize(source);
	}

	public static T GetAt<T>(this IList<T> source, int index)
	{
		if (source == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (index < 0)
		{
			return source[source.Count + index];
		}
		return source[index];
	}
}

public static class ListExtension
{
	public static int GetSize<T>(IList<T> source)
	{
		if (source == null)
		{
			return 0;
		}
		return source.Count;
	}

	public static T Append<T>(this IList<T> source, T value)
	{
		source.Add(value);
		return value;
	}

	public static int IndexOfIgnoreCase(this IList<string> source, string value)
	{
		int count = source.Count;
		for (int i = 0; i < count; ++i)
		{
			if (source[i].Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}

		return -1;
	}

	public static int IndexOf<T>(this IList<T> source, Predicate<T> predicate)
	{
		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		int count = source.Count;
		for (int i = 0; i < count; ++i)
		{
			if (predicate(source[i]))
			{
				return i;
			}
		}

		return -1;
	}

	public static T FirstOrDefault<T>(this IList<T> source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		return source.Count > 0 ? source[0] : default(T);
	}

	public static T LastOrDefault<T>(this IList<T> source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		int count = source.Count;
		return count > 0 ? source[count - 1] : default(T);
	}

	public static void SetAll<T>(this IList<T> source, T value)
	{
		for (int i = 0; i < source.Count; ++i)
		{
			source[i] = value;
		}
	}

	public static List<TResult> Select<TSource, TResult>(this IList<TSource> source, Func<TSource, TResult> selector)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (selector == null)
		{
			throw Error.ArgumentNull("selector");
		}

		var result = new List<TResult>();

		int count = source.Count;
		for (int i = 0; i < count; ++i)
		{
			result.Add(selector(source[i]));
		}

		return result;
	}

	public static List<T> WhereList<T>(this IList<T> source, Func<T, bool> predicate)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		var result = new List<T>();

		int count = source.Count;
		for (int i = 0; i < count; ++i)
		{
			if (predicate(source[i]))
			{
				result.Add(source[i]);
			}
		}

		return result;
	}

	public static T Last<T>(this IList<T> source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (source.Count == 0)
		{
			throw Error.NoElements();
		}

		return source[source.Count - 1];
	}

	public static T Random<T>(this IList<T> source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (source.Count == 0)
		{
			throw Error.NoElements();
		}

		return source[UnityEngine.Random.Range(0, source.Count)];
	}

	public static IList<T> Shuffle<T>(this IList<T> source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		int n = source.Count;
		while (n > 1)
		{
			n--;
			int k = UnityEngine.Random.Range(0, n + 1);
			T value = source[k];
			source[k] = source[n];
			source[n] = value;
		}
		return source;
	}

	public static IList<T> ShuffleTake<T>(this IList<T> source, int count)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		var res = new List<T>();

		int cnt = count;
		int targetIndex = -1;
		IList<T> suffledSrc = source.ToList();
		IList<T> suffleCache = null;

		do
		{
			if (targetIndex < 0)
			{
				suffleCache = suffledSrc.Shuffle();
				targetIndex = suffleCache.Count - 1;
			}
			res.Add(suffleCache[targetIndex]);
			cnt--;
			targetIndex--;
		}
		while (cnt > 0);

		return res;
	}

	public static string AsString<T>(this IList<T> source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		using (Reusable<StringBuilder> reusable = ReusableStringBuilder.Pop())
		{
			StringBuilder builder = reusable.Value;
			builder.Append("{ ");
			for (int i = 0; i < source.Count; ++i)
			{
				if (i != 0)
				{
					builder.Append(", ");
				}
				builder.Append(source[i]);
			}

			builder.Append(" }");

			return builder.ToString();
		}
	}

	public static List<List<TKey>> Split<TKey>(this List<TKey> source, int splitCount)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (splitCount <= 0)
		{
			throw Error.ArgumentNull("invalid split count");
		}

		var res = new List<List<TKey>>();

		if (source.Count == 0)
		{
			return res;
		}

		for (int i = 0, imax = (source.Count / splitCount) + 1; i < imax; i++)
		{
			if (i * splitCount == source.Count)
			{
				break;
			}
			res.Add(new List<TKey>());
			for (int j = 0, jmax = (i + 1) * splitCount <= source.Count ? splitCount : source.Count % splitCount; j < jmax; j++)
			{
				res[i].Add(source[i * splitCount + j]);
			}
		}
		return res;
	}

	public static List<T> Fill<T>(this List<T> source, Func<T> defaultObj, int count)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}
		while (source.Count < count)
		{
			source.Add(default(T));
		}

		return source;
	}

	public static IList<T> Take<T>(this IList<T> source, int count)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}
		if (count < 0)
		{
			count = 0;
		}
		else if (count > source.Count)
		{
			count = source.Count;
		}

		var result = new List<T>(count);
		for (int i = 0; i < count; i++)
		{
			result.Add(source[i]);
		}
		return result;
	}

	public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
		Func<TSource, TKey> selector)
	{
		return source.MinBy(selector, null);
	}

	public static TS MinBy<TS, TK>(this IEnumerable<TS> source,
		Func<TS, TK> selector, IComparer<TK> comparer)
	{
		if (source == null) return default;
		if (selector == null) throw new ArgumentNullException("selector");
		comparer = comparer ?? Comparer<TK>.Default;

		using (var sourceIterator = source.GetEnumerator())
		{
			if (!sourceIterator.MoveNext())
			{
				return default;
			}
			var min = sourceIterator.Current;
			var minKey = selector(min);
			while (sourceIterator.MoveNext())
			{
				var candidate = sourceIterator.Current;
				var candidateProjected = selector(candidate);
				if (comparer.Compare(candidateProjected, minKey) < 0)
				{
					min = candidate;
					minKey = candidateProjected;
				}
			}
			return min;
		}
	}
}