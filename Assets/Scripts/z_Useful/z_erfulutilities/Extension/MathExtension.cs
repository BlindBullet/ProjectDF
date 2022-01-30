using System;
using UnityEngine;

public static class MathExt
{
	#region Arithmetic 
	public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }

    public static float Sqr(this float val)
    {
        return val * val;
    }

    public static double Sqr(this double val)
    {
        return val * val;
    }

    public static float Norm(float val, float min, float max)
    {
	    return (val - min) / (max - min);
    }

	public static float SignOrZero(float v)
	{
		if (v > 0f) { return 1f; }
		else if (v < 0f) { return -1f; }
		return 0f;
	}

	public static double AbsDiff(this double val, double comparsion)
    {
        return Math.Abs(comparsion - val);
    }

	public static int Mod(this int x, int m)
	{
        //Modulo: 00:00:07.2661827((n % x) + x) % x)
        //If: 00:00:13.5378989((k %= n) < 0) ? k + n : k
        return (x % m + m) % m;
    }

    public static int DivideRoundDown(this int x, int d)
    {
        return (x / d) + ((x % d) >> 31);
    }
    #endregion

    #region sequence
    /// <summary>
    /// 등비수열의 합
    /// </summary>
    public static double GeometricSequenceSum(double baseValue, double commaRatio, int n)
    {
        double sum = 0;

        if (commaRatio.AbsDiff(1) < double.Epsilon)
            sum = baseValue * commaRatio * n;
        else
            sum = (baseValue * (1 - Math.Pow(commaRatio, n))) / (1 - commaRatio);

        return sum;
    }
	#endregion

	#region casting

	/// <summary>
	/// Eleminate Banker's Rounding
	/// </summary>
	public static int ToIntSafe(this double val)
    {
        return (int)(val + 0.49d);
    }

    /// <summary>
    /// Eleminate Banker's Rounding
    /// </summary>
    public static int ToIntSafe(this float val)
    {
        return (int)(val + 0.49f);
    }

    public static float Pct2Ratio(this float percent)
    {
        return percent / 100.0f;
    }

    public static float Pct2Ratio(this int percent)
    {
        return percent / 100.0f;
    }

	public static float NanToZero(this float value)
	{
		return Single.IsNaN(value) ? 0f : value;
	}

    #endregion casting

    #region algebra

    /// <summary>
    /// 누진 합
    /// </summary>
    public static double ProgressiveSum(int target, int[] sections, double[] ratios)
    {
        //case 1 : target is lower then first section
        if (target <= sections[0])
            return ratios[0] * target;

        //2. normal case
        double res =  ratios[0] * sections[0];
        for (int i = 1; i < sections.Length; i++)
        {
            if (target <= sections[i])
            {
                var sumVal = ratios[i] * (target - sections[i - 1]);
                res += sumVal;
                return res;
            }
            else
            {
                var sumVal =  ratios[i] * (sections[i] - sections[i - 1]);
                res += sumVal;
            }
        }
        var sumVal2 = ratios[ratios.Length - 1] * (target - sections[sections.Length - 1]);
        res += sumVal2;
        return res;
    }

	/// <summary>
	/// 누진 곱(sections : n , ratios : n + 1) section 범위 : 초과~이하
	/// </summary>
	public static double ProgressiveExponentProduct(int target, int[] sections, double[] ratios)
    {
        //1. special case : target is lower then first section
        if (target <= sections[0])
            return Math.Pow(ratios[0], target);

        //2. normal case
        double res =  Math.Pow(ratios[0], sections[0]);
        for (int i = 1; i < sections.Length; i++)
        {
            if (target <= sections[i])
            {
                res *= Math.Pow(ratios[i], target - sections[i - 1]);
                return res;
            }
            else
            {
                res *= Math.Pow(ratios[i], sections[i] - sections[i - 1]);
            }
        }
        res *= Math.Pow(ratios[ratios.Length - 1], target - sections[sections.Length - 1]);
        return res;
    }

    #endregion algebra

    #region Numerical Analysis

    public static Vector3 QuadraticBezier(Vector3 p1, Vector3 p2, Vector3 p3, float step)
    {
        Vector3 ap1 = Vector3.Lerp(p1, p2, step);
        Vector3 ap2 = Vector3.Lerp(p2, p3, step);
        return Vector3.Lerp(ap1, ap2, step);
    }

    public static Vector3 CubicBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float step)
    {
        Vector3 ap1  = Vector3.Lerp(p1, p2, step);
        Vector3 ap2  = Vector3.Lerp(p2, p3, step);
        Vector3 ap3  = Vector3.Lerp(p3, p4, step);

        Vector3 bp1  = Vector3.Lerp(ap1, ap2, step);
        Vector3 bp2  = Vector3.Lerp(ap2, ap3, step);

        return Vector3.Lerp(bp1, bp2, step);
    }

	#endregion Numerical Analysis

	#region Trigonometric
	/// <summary>
	/// //ArcTan 곡선의 -1 ~ 1값을 리턴. 
	/// </summary>
	public static float ArcTanNormalized(float val, float standard, float scale)
    {
        return (Mathf.Atan(val - standard) * 0.6366f) * scale;
    }

    public static float ArcTanNormalized(double val, double standard, double scale)
    {
        return (float)((Math.Atan(val - standard) * 0.6366) * scale);
    }

    public static float ArcTanNormalizedZeroToOne(float val, float standard, float scale)
    {
        //0~1값 리턴
        return (Mathf.Atan(val - standard) * 0.6366f) * scale * 0.5f + 0.5f;
    }

    public static float ArcTanNormalizedZeroToOne(double val, double standard, double scale)
    {
        return (float)((Math.Atan(val - standard) * 0.6366) * scale * 0.5 + 0.5);
    }

    public static float VectorToAngleAtan(Vector2 f_dir)
    {
        return Mathf.Atan2(f_dir.y, f_dir.x) * Mathf.Rad2Deg;
    }

	public static Vector2 AngleToVector(float angleInDegree)
	{
		return new Vector2(Mathf.Cos(angleInDegree * Mathf.Deg2Rad), Mathf.Sin(angleInDegree * Mathf.Deg2Rad));
	}

	#endregion Trigonometric
}