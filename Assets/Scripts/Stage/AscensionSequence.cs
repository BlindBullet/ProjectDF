using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscensionSequence : MonoBehaviour
{
	public _2dxFX_NewTeleportation Fx;

	public IEnumerator FadeIn()
	{
		transform.SetAsLastSibling();
		Fx._Alpha = 1f;
		Fx._Fade = 1f;

		float _time = 2f;
		float time = _time;

		while (time > 0)
		{
			time -= Time.deltaTime;

			float value = time / _time;

			Fx._Fade = value;

			yield return null;
		}

		Fx._Fade = 0f;
	}

	public IEnumerator FadeOut()
	{		
		float _time = 2f;
		float time = _time;

		while (time > 0)
		{
			time -= Time.deltaTime;

			float value = time / _time;

			Fx._Alpha = value;

			yield return null;
		}

		this.gameObject.SetActive(false);
	}

}
