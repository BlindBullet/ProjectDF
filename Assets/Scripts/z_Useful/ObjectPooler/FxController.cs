using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxController : MonoBehaviour
{
	Vector3 scale;

	public void Setup(float duration)
	{		
		scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);		
		StartCoroutine(ShowFxSeq(duration));
	}

	IEnumerator ShowFxSeq(float duration)
	{
		yield return new WaitForSeconds(duration);

		transform.localScale = scale;
		this.gameObject.SetActive(false);
	}

	void OnDisable()
	{	
		ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 		
	}
}
