using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogCautionBar : DialogController
{
	public Image Bg;
	public TextMeshProUGUI CautionText;

	public void SetDialog(string cautionText)
	{
		CautionText.text = LanguageManager.Ins.SetString(cautionText);
		Show(false);
		StartCoroutine(DelayClose());
	}

	IEnumerator DelayClose()
	{
		float t = 2f;

		while (t > 0)
		{
			t -= Time.deltaTime;

			Bg.color = new Color(Bg.color.r, Bg.color.g, Bg.color.b, t);
			CautionText.alpha = t;

			yield return null;
		}
		
		CloseDialog();
	}

}
