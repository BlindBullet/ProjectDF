using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotLvUpBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Slot Slot;
	
	public void OnPointerDown(PointerEventData eventData)
	{
		StartCoroutine("BtnDownSeq");		
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		StopCoroutine("BtnDownSeq");		
	}

	IEnumerator BtnDownSeq()
	{
		yield return new WaitForSeconds(2f);

		float time = 0f;

		while (true)
		{
			yield return new WaitForSeconds(0.2f * (1 - time));

			Slot.LevelUp();

			time += 0.05f;

			if (time >= 1f)
				time = 0.95f;
		}
	}


}
