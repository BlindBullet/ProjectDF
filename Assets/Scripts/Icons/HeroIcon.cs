using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;
using DG.Tweening;
using TMPro;

public class HeroIcon : MonoBehaviour
{
	public Button Btn;
	public Image IconBg;
	public Image IconImg;
	public Image IconFrame;	
	public GameObject LockIcon;
	public GameObject[] Stars;
	public GameObject SelectedFrame;
	public GameObject EnchantLabelObj;
	public TextMeshProUGUI EnchantLvText;
	public HeroData Data;
	Material selectedFrameMat;
	Material iconImgMat;
	public bool Dispatched = false;

	public void Setup(HeroData data, Action<HeroData> action = null)
	{
		SetShader();
		Data = data;
		List<HeroChart> chartList = CsvData.Ins.HeroChart[data.Id];
		HeroChart chart = null;

		for(int i = 0; i < chartList.Count; i++)
		{
			if (chartList[i].Grade == data.Grade)
				chart = chartList[i];
		}

		
		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => { if (action != null) action(data); });

		SetIcon(chart, data);
		SetStars(chart.Grade);
		SetLock(data.IsOwn);
	}

	public void Setup(HeroData data, HeroIcon icon, Action<HeroIcon, HeroData> action = null)
	{
		SetShader();
		Data = data;
		List<HeroChart> chartList = CsvData.Ins.HeroChart[data.Id];
		HeroChart chart = null;

		for (int i = 0; i < chartList.Count; i++)
		{
			if (chartList[i].Grade == data.Grade)
				chart = chartList[i];
		}


		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => { if (action != null) action(this, data); });

		SetIcon(chart, data);
		SetStars(chart.Grade);
		SetLock(data.IsOwn);
	}

	void SetShader()
	{
		Image selectedFrameImg = SelectedFrame.GetComponent<Image>();
		selectedFrameImg.material = new Material(selectedFrameImg.materialForRendering);
		selectedFrameMat = selectedFrameImg.materialForRendering;

		IconImg.material = new Material(IconImg.materialForRendering);
		iconImgMat = IconImg.materialForRendering;

		iconImgMat.SetFloat("_GreyscaleBlend", 0f);
		selectedFrameMat.SetFloat("_ShineGlow", 0f);
	}

	void SetIcon(HeroChart chart, HeroData data)
	{
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Characters").GetSprite(chart.Model);
		IconFrame.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("HeroFrame_" + chart.Attr.ToString());
		IconBg.sprite = Resources.Load<SpriteAtlas>("Sprites/Icons").GetSprite("HeroBg_" + chart.Attr.ToString());

		if (data.EnchantLv > 0)
		{
			EnchantLabelObj.SetActive(true);
			EnchantLvText.text = "+" + data.EnchantLv;
		}
		else
		{
			EnchantLabelObj.SetActive(false);
		}		
	}

	void SetLock(bool isOpen)
	{
		if (!isOpen)
		{
			IconGreyScale(true);
			LockIcon.SetActive(true);
		}
		else
		{
			IconGreyScale(false);
			LockIcon.SetActive(false);
		}
	}
	
	void SetStars(int grade)
	{
		for(int i = 0; i < grade; i++)
		{
			Stars[i].SetActive(true);
		}
	}

	public void ShowSelectedFrame()
	{
		SelectedFrame.SetActive(true);
		selectedFrameMat.SetFloat("_ShineGlow", 1f);
		selectedFrameMat.DOFloat(6.28f, "_ShineRotate", 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("SF" + Data.Id);
	}

	public void CloseSelectedFrame()
	{
		DOTween.Kill("SF" + Data.Id);
		selectedFrameMat.SetFloat("_ShineGlow", 0f);
		SelectedFrame.SetActive(false);
	}

	public void IconGreyScale(bool on)
	{
		if (on)
		{
			iconImgMat.SetFloat("_GreyscaleBlend", 1f);			
		}
		else
		{
			iconImgMat.SetFloat("_GreyscaleBlend", 0f);			
		}
	}

	public void DiasbleBtns(bool lockPanelOff = false)
	{
		if (!lockPanelOff)
		{
			IconGreyScale(true);
		}	

		Btn.enabled = false;
	}

	public void EnableBtns()
	{
		if(Data.IsOwn)
			IconGreyScale(false);

		Btn.enabled = true;
	}

}
