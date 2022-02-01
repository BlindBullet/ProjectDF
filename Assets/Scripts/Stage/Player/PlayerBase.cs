using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public static PlayerBase Player;

	private void OnEnable()
	{
		Player = this;
	}




}
