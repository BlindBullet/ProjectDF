using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
	public static void DebugRay(this Transform value, Vector3 normal, Color color)
	{
		Debug.DrawRay(value.position, normal.normalized, color);
	}

	public static void DebugRay(this Transform value, Vector3 normal, Color color, float duration)
	{
		Debug.DrawRay(value.position, normal.normalized, color, duration);
	}

	public static List<T> GetComponentWithoutRoot<T>(this GameObject obj) where T : Component
	{
		List<T> res = new List<T>();
		foreach (Transform child in obj.transform.root)
		{
			T[] scripts = child.GetComponentsInChildren<T>();
			if (scripts != null)
			{
				foreach (T sc in scripts)
					res.Add(sc);
			}
		}
		return res;
	}

	public static Transform ResetTransform(this Transform obj)
	{
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.zero;
		return obj;
	}
}