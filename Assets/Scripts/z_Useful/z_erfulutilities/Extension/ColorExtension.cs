using UnityEngine;

public static class ColorExtension
{
	public static Color Hash(uint s)
	{
		int r = (int)((s & 0xff000000) >> 24);   //0xFF000000
		int g = (int)((s & 0xff0000) >> 16);
		int b = (int)((s & 0xff00) >> 8);

		return new Color(((float)r) / 255f, ((float)g) / 255f, ((float)b) / 255f);
	}

	public static Color Hash(string hash)
	{
		uint s = (uint)hash.GetHashCode();
		return Hash(s);
	}

	public static string ToHtmlCol(this Color color)
	{
		return ToHtmlCol((Color32)color);
	}

	public static string ToHtmlCol(this Color32 color)
	{
		string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
		return hex;
	}

	public static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}

	public static Color WithR(this Color c, float r)
	{
		return new Color(r, c.g, c.b, c.a);
	}

	public static Color WithG(this Color c, float g)
	{
		return new Color(c.r, g, c.b, c.a);
	}

	public static Color WithB(this Color c, float b)
	{
		return new Color(c.r, c.g, b, c.a);
	}

	public static Color WithA(this Color c, float a)
	{
		return new Color(c.r, c.g, c.b, a);
	}

	public static float SumOfColorElement(this Color c)
	{
		return c.r + c.g + c.b;
	}
}