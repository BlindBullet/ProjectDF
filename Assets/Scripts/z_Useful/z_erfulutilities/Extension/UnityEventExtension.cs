using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UnityEventExtension
{

	public static class Auto
	{
		public static bool _<T>(out T result, T input) where T : class
		{
			result = input;
			return input != null;
		}
	}


	public static void SetListner(this Button.ButtonClickedEvent clickEvent, UnityAction clickAction)
	{
		if (clickEvent == null)
		{
			return;
		}
		clickEvent.RemoveAllListeners();
		clickEvent.AddListener(clickAction);
	}



}