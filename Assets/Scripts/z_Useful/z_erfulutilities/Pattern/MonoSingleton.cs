using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Ins { get; private set; }
	void Awake() => Ins = FindObjectOfType(typeof(T)) as T;
}