using UnityEngine;

public static class VectorN
{
	public static Vector3 X(float v)
	{
		return new Vector3(v, 0, 0);
	}

	public static Vector3 Y(float v)
	{
		return new Vector3(0, v, 0);
	}

	public static Vector3 Z(float v)
	{
		return new Vector3(0, 0, v);
	}
}

public static class VectorExtension
{
	public static Vector2 xy(this Vector3 v)
	{
		return new Vector2(v.x, v.y);
	}

	public static Vector3 WithX(this Vector3 v, float x)
	{
		return new Vector3(x, v.y, v.z);
	}

	public static Vector3 WithY(this Vector3 v, float y)
	{
		return new Vector3(v.x, y, v.z);
	}

	public static Vector3 WithZ(this Vector3 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}

	public static Vector2 WithX(this Vector2 v, float x)
	{
		return new Vector2(x, v.y);
	}

	public static Vector2 WithMultiplyX(this Vector2 v, float mul)
	{
		return new Vector2(v.x * mul, v.y);
	}

	public static Vector2 WithY(this Vector2 v, float y)
	{
		return new Vector2(v.x, y);
	}

	public static Vector3 WithZ(this Vector2 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}

	public static Vector3 Abs(this Vector3 v)
	{
		return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	}
	public static Vector2 Round(this Vector2 v)
	{
		return new Vector2(
			Mathf.Round(v.x),
			Mathf.Round(v.y));
	}

	public static Vector3 Round(this Vector3 v)
	{
		return new Vector3(
			Mathf.Round(v.x),
			Mathf.Round(v.y),
			Mathf.Round(v.z));
	}

	public static Vector3 Round(this Vector3 vector3, int decimalPlaces)
	{
		float multiplier = 1;
		for (int i = 0; i < decimalPlaces; i++)
		{
			multiplier *= 10f;
		}
		return new Vector3(
			Mathf.Round(vector3.x * multiplier) / multiplier,
			Mathf.Round(vector3.y * multiplier) / multiplier,
			Mathf.Round(vector3.z * multiplier) / multiplier);
	}

	public static Vector3 Clamp(this Vector3 v, Vector3 min, Vector3 max)
	{
		return new Vector3(
			Mathf.Clamp(v.x, min.x, max.x),
			Mathf.Clamp(v.y, min.y, max.y),
			Mathf.Clamp(v.z, min.z, max.z));
	}

	public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max)
	{
		return new Vector3(
			Mathf.Clamp(v.x, min.x, max.x),
			Mathf.Clamp(v.y, min.y, max.y));
	}


	// axisDirection - unit vector in direction of an axis (eg, defines a line that passes through
	// zero) ConerPoint - the ConerPoint to find nearest on line for
	public static Vector3 NearestCornerPointOnAxis(this Vector3 axisDirection, Vector3 ConerPoint, bool isNormalized = false)
	{
		if (!isNormalized) axisDirection.Normalize();
		var d = Vector3.Dot(ConerPoint, axisDirection);
		return axisDirection * d;
	}

	// lineDirection - unit vector in direction of line ConerPointOnLine - a ConerPoint on the line
	// (allowing us to define an actual line in space) ConerPoint - the ConerPoint to find nearest
	// on line for
	public static Vector3 NearestCornerPointOnLine(
		this Vector3 lineDirection, Vector3 ConerPoint, Vector3 ConerPointOnLine, bool isNormalized = false)
	{
		if (!isNormalized) lineDirection.Normalize();
		var d = Vector3.Dot(ConerPoint - ConerPointOnLine, lineDirection);
		return ConerPointOnLine + (lineDirection * d);
	}

	public static void Random(this ref Vector3 myVector, Vector3 min, Vector3 max)
	{
		myVector = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
	}

	public static Vector3 Random(Vector3 min, Vector3 max)
	{
		return new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
	}
}