using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtensionMethods
{
    public static string ToRich(this string value, Color col)
    {
        return "<color=" + col.ToHtmlCol() + ">" + value + "</color>";
    }

	public static string ToRich(this string value, Color col, int sizePct)
	{
		return $"<size={sizePct}%><color={col.ToHtmlCol()}>{value}</color></size>";
	}

	public static string ToRich(this string value, string htmlColor)
    {
        return "<color=" + htmlColor + ">" + value + "</color>";
    }

    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    // Named format strings from object attributes. Eg:
    // string blaStr = aPerson.ToString("My name is {FirstName} {LastName}.")
    // From: http://www.hanselman.com/blog/CommentView.aspx?guid=fde45b51-9d12-46fd-b877-da6172fe1791
    public static string ToString(this object anObject, string aFormat)
    {
        return ToString(anObject, aFormat, null);
    }

    public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
    {
        StringBuilder sb = new StringBuilder();
        Type type = anObject.GetType();
        Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
        MatchCollection mc = reg.Matches(aFormat);
        int startIndex = 0;
        foreach (Match m in mc)
        {
            Group g = m.Groups[2]; //it's second in the match between { and }
            int length = g.Index - startIndex - 1;
            sb.Append(aFormat.Substring(startIndex, length));

            string toGet = string.Empty;
            string toFormat = string.Empty;
            int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
            if (formatIndex == -1) //no formatting, no worries
            {
                toGet = g.Value;
            }
            else //pickup the formatting
            {
                toGet = g.Value.Substring(0, formatIndex);
                toFormat = g.Value.Substring(formatIndex + 1);
            }

            //first try properties
            PropertyInfo retrievedProperty = type.GetProperty(toGet);
            Type retrievedType = null;
            object retrievedObject = null;
            if (retrievedProperty != null)
            {
                retrievedType = retrievedProperty.PropertyType;
                retrievedObject = retrievedProperty.GetValue(anObject, null);
            }
            else //try fields
            {
                FieldInfo retrievedField = type.GetField(toGet);
                if (retrievedField != null)
                {
                    retrievedType = retrievedField.FieldType;
                    retrievedObject = retrievedField.GetValue(anObject);
                }
            }

            if (retrievedType != null) //Cool, we found something
            {
                string result = string.Empty;
                if (toFormat == string.Empty) //no format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, null) as string;
                }
                else //format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
                }
                sb.Append(result);
            }
            else //didn't find a property with that name, so be gracious and put it back
            {
                sb.Append("{");
                sb.Append(g.Value);
                sb.Append("}");
            }
            startIndex = g.Index + g.Length + 1;
        }
        if (startIndex < aFormat.Length) //include the rest (end) of the string
        {
            sb.Append(aFormat.Substring(startIndex));
        }
        return sb.ToString();
    }

    public static bool ContainsIgnoreCase(this string source, string toCheck)
    {
        return source.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static float ToFloat(this string source, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
    {
        float value;
        float.TryParse(source, style, NumberFormatInfo.CurrentInfo, out value);
        return value;
    }

    public static int ToInt(this string source, NumberStyles style = NumberStyles.Integer)
    {
        int value;
        int.TryParse(source, style, NumberFormatInfo.CurrentInfo, out value);
        return value;
    }

    public static T ToEnum<T>(this string source, T value = default(T))
    {
        T res;
        return TryEnum(source, out res, true) ? res : value;
    }

    public static bool TryEnum<T>(this string source, out T value, bool showError = false)
    {
        try
        {
            value = (T)Enum.Parse(typeof(T), source, true);
            return true;
        }
        catch (Exception e)
        {
            if (showError)
            {
                Debug.LogWarning(e);
            }

            value = default(T);
            return false;
        }
    }

    public static string[] SplitAndTrim(this string text, char sep)
    {
        string[] texts = text.Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < texts.Length; ++i)
        {
            texts[i] = texts[i].Trim();
        }

        return texts;
    }

    public static string RemoveFromEnd(this string s, string suffix)
    {
        if (s.EndsWith(suffix))
        {
            return s.Substring(0, s.Length - suffix.Length);
        }
        return s;
    }

    public static string RemoveFromBegin(this string s, string prefix)
    {
        if (s.StartsWith(prefix))
        {
            return s.Substring(prefix.Length, s.Length - prefix.Length);
        }
        return s;
    }

	public static bool IsNullOrEmpty(this string s)
	{
		if(s == null)
		{
			return true;
		}
		return s == string.Empty;
	}

    public static StringBuilder Clear(this StringBuilder sb)
    {
        sb.Clear();
        return sb;
    }
}

public static class Error
{
    public static Exception ArgumentNull(string paramName)
    {
        return new ArgumentNullException(paramName);
    }

    public static Exception NoElements()
    {
        return new InvalidOperationException("NoElements");
    }
}