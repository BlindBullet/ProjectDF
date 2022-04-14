using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseStagePanel : MonoBehaviour
{
	public _2dxFX_BurnFX Fx;

	public IEnumerator FadeIn()
	{
		Fx.Destroyed = 1f;

		float _time = 2f;
		float time = _time;

		while (time > 0)
		{
			time -= Time.deltaTime;

			float value = time / _time;

			Fx.Destroyed = value;

			yield return null;
		}

		Fx.Destroyed = 0f;
	}

	public IEnumerator FadeOut()
	{
		Fx.Destroyed = 0f;

		float _time = 2f;
		float time = _time;

		while (time > 0)
		{
			time -= Time.deltaTime;

			float value = 1f - (time / _time);

			Fx.Destroyed = value;

			yield return null;
		}

		Fx.Destroyed = 1f;
		this.gameObject.SetActive(false);
	}

}
