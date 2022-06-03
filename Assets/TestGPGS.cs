using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGPGS : MonoBehaviour
{
	private void Start()
	{
		GPGSBinder.Inst.Login((success, localuser) => Debug.Log($"{success}, {localuser.userName}, {localuser.id}, {localuser.state}, {localuser.underage}"));
	}

	
}
