using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnumUtility
{
	public static bool IsInLayer(this GameObject obj, LayerMask f_mask)
	{
		if (((1 << obj.layer) & f_mask.value) != (1 << obj.layer))
			return false;
		return true;
	}

	public static int GetElementCount<T>()
	{
		return Enum.GetNames(typeof(T)).Length;
	}

	public static bool TryParse<T>(string value)
	{
		return Enum.IsDefined(typeof(T), value);
	}

	public static T Parse<T>(string value)
	{
		return (T)Enum.Parse(typeof(T), value, true);
	}

	/// <summary>
	/// ParseSafe와 같으나 error출력없음
	/// </summary>
	public static T ParseNoneable<T>(string value, T defaultValue)
	{
		if (string.IsNullOrEmpty(value))
			return defaultValue;
		return EnumUtility.Parse<T>(value);
	}

	public static T ParserNonableUpper<T>(string value, T defaultValue)
	{
		if (string.IsNullOrEmpty(value))
			return defaultValue;
		return EnumUtility.Parse<T>(value.ToUpper());
	}

	public static T ParseSafe<T>(string value, T defaultValue)
	{
		if (string.IsNullOrEmpty(value))
			return defaultValue;

		if (Enum.IsDefined(typeof(T), value))
			return EnumUtility.Parse<T>(value);
		else
		{
#if UNITY_EDITOR
			Debug.LogError(value + "를 성공적으로 파싱하지 못해 기본값으로 대체됨");
#endif
			return defaultValue;
		}
	}

	public static T GetRandom<T>(params T[] ignores)
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		while (true)
		{
			T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
			if (!ignores.Contains(V))
			{
				return V;
			}
		}
	}

	public static bool IsEqual<T>(Enum first, T second) where T : struct
	{
		var asEnumType = first as T?;
		return asEnumType != null && EqualityComparer<T>.Default.Equals(asEnumType.Value, second);
	}

	/// <summary>
	/// Is Target and values flags crossed each other at least 1?
	/// </summary>
	public static bool IsCrossed<T>(this T target, T values)
		where T : struct, IConvertible
	{
		return (target.ToInt64(null) & values.ToInt64(null)) > 0;
	}

	/// <summary>
	/// Is Target containing every flags in comparsionEnums?
	/// </summary>
	public static bool ContainsFlags<T>(this T biggerEnum, T comparsionEnums)
	where T : struct, IConvertible
	{
		return (biggerEnum.ToInt64(null) & comparsionEnums.ToInt64(null)) == comparsionEnums.ToInt64(null);
	}

	public static bool IsFlag<T>(this System.Enum type, T value)
	{
#if UNITY_EDITOR
		try
		{
			return (int)(object)type == (int)(object)value;
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			return false;
		}
#else
        return (int)(object)type == (int)(object)value;
#endif
	}

	public static T AddFlag<T>(this System.Enum type, T value)
	{
#if UNITY_EDITOR
		try
		{
			return (T)(object)(((int)(object)type | (int)(object)value));
		}
		catch (Exception ex)
		{
			throw new ArgumentException(
				string.Format(
					"Could not append value from enumerated type '{0}'.",
					typeof(T).Name
					), ex);
		}
#else
        return (T)(object)(((int)(object)type | (int)(object)value));
#endif
	}

	public static T RemoveFlag<T>(this System.Enum type, T value)
	{
#if UNITY_EDITOR
		try
		{
			return (T)(object)(((int)(object)type & ~(int)(object)value));
		}
		catch (Exception ex)
		{
			throw new ArgumentException(
				string.Format(
					"Could not remove value from enumerated type '{0}'.",
					typeof(T).Name
					), ex);
		}
#else
        return (T)(object)(((int)(object)type & ~(int)(object)value));
#endif
	}
}