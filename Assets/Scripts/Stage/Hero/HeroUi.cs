using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using System;
using DG.Tweening;

public class HeroUi : MonoBehaviour
{
	public Button IconBtn;
	public Image IconFrame;
	public Image IconImg;
	public Image IconBg;    
	public Image SkillCoolTimeFrame;
	public GameObject[] Stars;    
	HeroBase me;
	public _2dxFX_DestroyedFX ImgDestroyFx;
	public _2dxFX_DestroyedFX BgDestroyFx;
	public _2dxFX_DestroyedFX FrameDestroyFx;

	public void SetUp(HeroData data, SlotData slotData)
	{
		me = GetComponent<HeroBase>();        
		HeroChart chart = CsvData.Ins.HeroChart[data.Id][data.Grade - 1];

		SetIcon(chart);
		SetStars(chart.Grade);        
		SetIconBtn();
	}

	void SetIcon(HeroChart chart)
	{
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Heroes/Heroes").GetSprite(chart.Model);
		IconFrame.sprite = Resources.Load<Sprite>("Sprites/Heroes/Frames/" + chart.Attr.ToString());
		IconBg.sprite = Resources.Load<Sprite>("Sprites/Heroes/Bgs/" + chart.Attr.ToString());
	}

	public void SetCoolTimeFrame(float value)
	{
		SkillCoolTimeFrame.fillAmount = value;
	}
		
	void SetIconBtn()
	{
		IconBtn.onClick.RemoveAllListeners();
		IconBtn.onClick.AddListener(() => me.SkillCon.UseSkill());
	}

	void SetStars(int grade)
	{
		for (int i = 0; i < grade; i++)
		{
			Stars[i].SetActive(true);
		}
	}

	public IEnumerator Die()
	{
		IconBtn.enabled = false;
		SkillCoolTimeFrame.fillAmount = 0;

		float _time = 2f;
		float time = _time;

		while(time > 0)
		{
			time -= Time.deltaTime;

			float value = 1f - (time / _time);

			ImgDestroyFx.Destroyed = value;
			BgDestroyFx.Destroyed = value;
			FrameDestroyFx.Destroyed = value;

			yield return null;
		}

		ImgDestroyFx.Destroyed = 1f;
		BgDestroyFx.Destroyed = 1f;
		FrameDestroyFx.Destroyed = 1f;
	}


}
