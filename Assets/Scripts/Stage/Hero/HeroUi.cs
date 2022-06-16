using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.U2D;
using System;
using DG.Tweening;
using AllIn1SpriteShader;

public class HeroUi : MonoBehaviour
{
	public Button IconBtn;
	public Image IconFrame;
	public Image IconImg;
	public Image IconBg;    
	public Image SkillCoolTimeFrame;
	public GameObject[] Stars;    
	HeroBase me;	
	public TextMeshProUGUI SkillReadyText;
	bool skillReady = false;
	Material mat;
	Material frameMat;

	public void SetUp(HeroData data, SlotData slotData)
	{	
		IconBg.material = new Material(IconBg.materialForRendering);
		mat = IconBg.materialForRendering;
		mat.SetFloat("_FadeAmount", 0f);		
		IconBg.GetComponent<AllIn1Shader>().ApplyMaterialToHierarchy();		

		me = GetComponent<HeroBase>();        
		List<HeroChart> charts = CsvData.Ins.HeroChart[data.Id];
		HeroChart chart = null;

		for(int i = 0; i < charts.Count; i++)
		{
			if (charts[i].Grade == data.Grade)
				chart = charts[i];
		}

		for (int i = 0; i < Stars.Length; i++)
		{
			Stars[i].SetActive(false);
		}

		SetIcon(chart);
		SetStars(chart.Grade);      
		SetIconBtn();
	}

	void SetIcon(HeroChart chart)
	{
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.Model);
		IconFrame.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Frame_Circle_" + chart.Attr.ToString());
		IconBg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("Bg_Circle_" + chart.Attr.ToString());
	}

	public void SetCoolTimeFrame(float value)
	{
		SkillCoolTimeFrame.fillAmount = value;

		if(value >= 1f && !skillReady)
		{
			ShowSkillReadyText();			
		}
	}
		
	void SetIconBtn()
	{
		IconBtn.onClick.RemoveAllListeners();
		IconBtn.onClick.AddListener(() => 
		{
			SoundManager.Ins.PlaySFX("se_button_2");

			if (me.SkillCon.UseSkill())
			{
				CloseSkillReadyText();				
			}
		});
	}

	public void SetStars(int grade)
	{
		for (int i = 0; i < grade; i++)
		{
			Stars[i].SetActive(true);
		}
	}

	public void ShowSkillReadyText()
	{
		skillReady = true;
		SkillReadyText.DOFade(1f, 0f);
		SkillReadyText.gameObject.SetActive(true);		
		SkillReadyText.DOFade(0f, 1f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetId("ReadyText" + me.Data.Id);
	}

	public void CloseSkillReadyText()
	{
		DOTween.Kill("ReadyText" + me.Data.Id);
		SkillReadyText.gameObject.SetActive(false);
		skillReady = false;
	}

	public IEnumerator Die()
	{
		IconBtn.enabled = false;
		//SkillCoolTimeFrame.fillAmount = 0;
		CloseSkillReadyText();
				
		mat.DOFloat(1f, "_FadeAmount", 2f).SetEase(Ease.InOutQuad);

		yield return new WaitForSeconds(2f);				
	}


}
