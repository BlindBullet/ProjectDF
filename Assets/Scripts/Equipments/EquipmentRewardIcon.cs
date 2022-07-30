using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.U2D;

public class EquipmentRewardIcon : MonoBehaviour
{	
	public Image Bg;
	public Image Image;
	public Image Frame;
	public TextMeshProUGUI GradeText;
	public TextMeshProUGUI LvText;
	public GameObject Grade4Fx;
	public GameObject Grade5Fx;

	public void SetIcon(EquipmentChart chart)
	{
		Bg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Equipment_Bg_" + chart.Grade.ToString());
		Frame.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Equipment_Frame_" + chart.Grade.ToString());
		Image.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite(chart.Icon);

		switch (chart.Grade)
		{
			case 1:
				GradeText.text = LanguageManager.Ins.SetString("equipment_grade_1");
				break;
			case 2:
				GradeText.text = LanguageManager.Ins.SetString("equipment_grade_2");
				break;
			case 3:
				GradeText.text = LanguageManager.Ins.SetString("equipment_grade_3");
				break;
			case 4:
				GradeText.text = LanguageManager.Ins.SetString("equipment_grade_4");				
				break;
			case 5:
				GradeText.text = LanguageManager.Ins.SetString("equipment_grade_5");				
				break;
		}

		LvText.text = chart.Level.ToString() + LanguageManager.Ins.SetString("EquipmentLv");

		StartCoroutine(DirectionSeq(chart));
	}

	IEnumerator DirectionSeq(EquipmentChart chart)
	{
		transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

		transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(0.35f);

		switch (chart.Grade)
		{
			case 4:
				Grade4Fx.SetActive(true);
				SoundManager.Ins.PlaySFX("se_enchant_hero_2");
				break;
			case 5:
				Grade5Fx.SetActive(true);
				SoundManager.Ins.PlaySFX("AppearAngel");
				break;
			default:
				SoundManager.Ins.PlaySFX("se_button_2");
				break;
		}
	}


}
