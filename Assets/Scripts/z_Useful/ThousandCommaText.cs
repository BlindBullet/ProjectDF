using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ThousandCommaText
{
    public static string GetThousandComma(int data)
	{
		if(data == 0)
		{
			return "0";
		}

		return string.Format("{0:#,###}", data);
	}
}
