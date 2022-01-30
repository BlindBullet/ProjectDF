using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatToString
{
	public static string GetN0(float value)
	{
		string result = string.Empty;

		if (value == (int)value)
			result = value.ToString();
		else
			result = value.ToString("N0");

		return result;
	}

	public static string GetN1(float value)
	{
		string result = string.Empty;

		if (value == (int)value)
			result = value.ToString();
		//else if (value - (int)value < 0.1f)
		//	result = value.ToString("N0");
		else
			result = value.ToString("N1");

		return result;
	}

	public static string GetN2(float value)
	{
		string result = string.Empty;

		if (value == (int)value)
			result = value.ToString();
		else if (value - (int)value < 0.1f)
			result = value.ToString("N0");
		else
			result = value.ToString("N2");

		return result;
	}
}
