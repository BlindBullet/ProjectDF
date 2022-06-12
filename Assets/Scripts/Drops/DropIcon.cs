using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropIcon : MonoBehaviour
{	
	public void Setup()
	{
		SetSfx();
		Move();
	}

	public abstract void SetSfx();
	public abstract void Move();
}
