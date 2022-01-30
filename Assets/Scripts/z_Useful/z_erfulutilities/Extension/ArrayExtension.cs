using System;
using System.Collections.Generic;

public static class ArrayExtension
{
	public static int GetArraySize<T>(this T[] source)
	{
		if (source == null)
		{
			return 0;
		}
		return source.Length;
	}

	public static IVector2 GetArraySize<T>(this T[][] source)
	{
		if (source == null)
		{
			return IVector2.zero;
		}

		var firstElem = source[0];
		if (firstElem == null)
		{
			return new IVector2(source.Length, 0);
		}

		return new IVector2(source.Length, firstElem.Length);
	}

	public static T[][] Create2d<T>(int w, int h)
	{
		var map = new T[w][];
		for (int i = 0; i < w; i++)
		{
			map[i] = new T[h];
		}

		return map;
	}

	public static int LastIndex<T>(this T[] target)
	{
		return target.Length - 1;
	}

	public static bool VerifyIndex<T>(this T[] target, int index)
	{
		if (target == null || target.Length == 0)
			return false;
		var len = target.Length;
		return index < 0 || index > len - 1;
	}

	public static int IndexOf<T>(this T[] source, T value)
	{
		return Array.IndexOf(source, value);
	}

	public static bool Contains<T>(this T[] source, T value)
	{
		return IndexOf(source, value) != -1;
	}

	public static int IndexOf<T>(this T[] source, Predicate<T> predicate)
	{
		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		int count = source.Length;
		for (int i = 0; i < count; ++i)
		{
			if (predicate(source[i]))
			{
				return i;
			}
		}

		return -1;
	}

	public static T FirstOrDefault<T>(this T[] source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		return source.Length > 0 ? source[0] : default(T);
	}

	public static T FirstOrDefault<T>(this T[] source, Func<T, bool> predicate)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		int count = source.Length;
		for (int i = 0; i < count; ++i)
		{
			if (predicate(source[i]))
			{
				return source[i];
			}
		}
		return default(T);
	}

	public static T LastOrDefault<T>(this T[] source)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		int count = source.Length;
		return count > 0 ? source[count - 1] : default(T);
	}

	public static T LastOrDefault<T>(this T[] source, Func<T, bool> predicate)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		for (int i = source.Length - 1; i >= 0; --i)
		{
			if (predicate(source[i]))
			{
				return source[i];
			}
		}
		return default(T);
	}

	public static bool Any<T>(this T[] source)
	{
		return source.Length > 0;
	}

	public static bool Any<T>(this T[] source, Predicate<T> predicate)
	{
		return IndexOf(source, predicate) != -1;
	}

	public static bool All<T>(this T[] source, Predicate<T> predicate)
	{
		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		int count = source.Length;
		for (int i = 0; i < count; ++i)
		{
			if (predicate(source[i]) == false)
			{
				return false;
			}
		}

		return true;
	}

	public static int Count<T>(this T[] source, Predicate<T> predicate)
	{
		if (predicate == null)
		{
			throw Error.ArgumentNull("predicate");
		}

		int result = 0;
		int count = source.Length;
		for (int i = 0; i < count; ++i)
		{
			if (predicate(source[i]))
			{
				result++;
			}
		}

		return result;
	}

	public static T Aggregate<T>(this T[] source, Func<T, T, T> func)
	{
		if (func == null)
		{
			throw Error.ArgumentNull("func");
		}

		int count = source.Length;
		if (count == 0)
		{
			throw Error.NoElements();
		}

		T result = source[0];
		for (int i = 1; i < count; i++)
		{
			result = func(result, source[i]);
		}

		return result;
	}

	public static TR Aggregate<TS, TR>(this TS[] source, TR seed, Func<TR, TS, TR> func)
	{
		if (func == null)
		{
			throw Error.ArgumentNull("func");
		}

		TR result = seed;

		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			result = func(result, source[i]);
		}

		return result;
	}

	public static int Sum(this int[] source)
	{
		int num = 0;

		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += source[i];
		}

		return num;
	}

	public static long Sum(this long[] source)
	{
		long num = 0;
		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += source[i];
		}

		return num;
	}

	public static float Sum(this float[] source)
	{
		float num = 0;
		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += source[i];
		}

		return num;
	}

	public static double Sum(this double[] source)
	{
		double num = 0;
		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += source[i];
		}

		return num;
	}

	public static int Sum<T>(this T[] source, Func<T, int> selector)
	{
		if (selector == null)
		{
			throw Error.ArgumentNull("selector");
		}

		int num = 0;
		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += selector(source[i]);
		}

		return num;
	}

	public static long Sum<T>(this T[] source, Func<T, long> selector)
	{
		if (selector == null)
		{
			throw Error.ArgumentNull("selector");
		}

		long num = 0;
		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += selector(source[i]);
		}

		return num;
	}

	public static float Sum<T>(this T[] source, Func<T, float> selector)
	{
		if (selector == null)
		{
			throw Error.ArgumentNull("selector");
		}

		float num = 0;
		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += selector(source[i]);
		}

		return num;
	}

	public static double Sum<T>(this T[] source, Func<T, double> selector)
	{
		if (selector == null)
		{
			throw Error.ArgumentNull("selector");
		}

		double num = 0;
		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			num += selector(source[i]);
		}

		return num;
	}

	public static void ForEach<T>(this T[] source, Action<T> action)
	{
		if (action == null)
		{
			throw Error.ArgumentNull("action");
		}

		int count = source.Length;
		for (int i = 0; i < count; i++)
		{
			action(source[i]);
		}
	}

	public static T Get<T>(this T[] source, int index)
	{
		return source.Get(index, default(T));
	}

	public static T Get<T>(this T[] source, int index, T defaultValue)
	{
		if (index < 0 || index >= source.Length)
		{
			return defaultValue;
		}
		return source[index];
	}

	public static void SetAll<T>(this T[] source, T value)
	{
		int count = source.Length;
		for (int i = 0; i < count; ++i)
		{
			source[i] = value;
		}
	}

	public static TResult[] Selectf<TSource, TResult>(this TSource[] source, Func<TSource, TResult> selector)
	{
		if (source == null)
		{
			throw Error.ArgumentNull("source");
		}

		if (selector == null)
		{
			throw Error.ArgumentNull("selector");
		}

		var result = new TResult[source.Length];

		int count = source.Length;
		for (int i = 0; i < count; ++i)
		{
			result[i] = selector(source[i]);
		}

		return result;
	}

	public static T[] Wheref<T>(this T[] source, Func<T, bool> predicate)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}

		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}

		var temp = new T[source.Length];

		int cnt = 0;
		for (int i = 0; i < temp.Length; ++i)
		{
			if (predicate(source[i]))
			{
				temp[cnt++] = source[i];
			}
		}

		var result = new T[cnt];
		Array.Copy(temp, result, cnt);
		return result;
	}

	public static List<T> ToList<T>(this T[] src)
	{
		var res = new List<T>();
		for (int i = 0; i < src.Length; i++)
		{
			res.Add(src[i]);
		}
		return res;
	}

	public static T Random<T>(this T[] source)
	{
		if (source.Length == 0 || source == null)
		{
			return default(T);
		}
		return source[UnityEngine.Random.Range(0, source.Length)];
	}
}