using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroEnchantBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{		
		StartCoroutine("EnchantBtnDownSeq");
	}

	public void OnPointerUp(PointerEventData eventData)
	{		
		StopCoroutine("EnchantBtnDownSeq");
	}

	IEnumerator EnchantBtnDownSeq()
	{
		yield return new WaitForSecondsRealtime(1f);

		float time = 0f;

		while (true)
		{			
			yield return new WaitForSecondsRealtime(0.2f * (1 - time));
						
			DialogHeroInfo._Dialog.Enchant();

			time += 0.05f;

			if (time >= 1f)
				time = 0.95f;
		}
	}


}