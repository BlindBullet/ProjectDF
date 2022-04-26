using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.U2D;
using DG.Tweening;

public class HeroIcon : MonoBehaviour
{
	public Button Btn;
	public Image IconBg;
	public Image IconImg;
	public Image IconFrame;	
	public GameObject LockIcon;
	public GameObject[] Stars;
	public GameObject SelectedFrame;
	public HeroData Data;
	Material selectedFrameMat;
	Material iconImgMat;
	public bool Dispatched = false;

	public void Setup(HeroData data, Action<HeroData> action = null)
	{
		Image selectedFrameImg = SelectedFrame.GetComponent<Image>();
		selectedFrameImg.material = new Material(selectedFrameImg.materialForRendering);
		selectedFrameMat = selectedFrameImg.materialForRendering;

		IconImg.material = new Material(IconImg.materialForRendering);
		iconImgMat = IconImg.materialForRendering;

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

		SetIcon(chart);
		SetStars(chart.Grade);
		SetLock(data.IsOwn);
	}

	public void Setup(HeroData data, HeroIcon icon, Action<HeroIcon, HeroData> action = null)
	{
		Image selectedFrameImg = SelectedFrame.GetComponent<Image>();
		selectedFrameImg.material = new Material(selectedFrameImg.materialForRendering);
		selectedFrameMat = selectedFrameImg.materialForRendering;

		IconImg.material = new Material(IconImg.materialForRendering);
		iconImgMat = IconImg.materialForRendering;

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

		SetIcon(chart);
		SetStars(chart.Grade);
		SetLock(data.IsOwn);
	}

	void SetIcon(HeroChart chart)
	{
		IconImg.sprite = Resources.Load<SpriteAtlas>("Sprites/Heroes/Heroes").GetSprite(chart.Model);
		IconFrame.sprite = Resources.Load<Sprite>("Sprites/Heroes/Frames/" + chart.Attr.ToString());
		IconBg.sprite = Resources.Load<Sprite>("Sprites/Heroes/Bgs/" + chart.Attr.ToString());
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
		selectedFrameMat.EnableKeyword("SHINE_ON");
		selectedFrameMat.DOFloat(6.28f, "_ShineRotate", 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("SF" + Data.Id);
	}

	public void CloseSelectedFrame()
	{
		DOTween.Kill("SF" + Data.Id);
		selectedFrameMat.DisableKeyword("SHINE_ON");
		SelectedFrame.SetActive(false);
	}

	public void IconGreyScale(bool on)
	{
		if (on)
		{
			iconImgMat.EnableKeyword("GREYSCALE_ON");
		}
		else
		{
			iconImgMat.DisableKeyword("GREYSCALE_ON");
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
		IconGreyScale(false);
		Btn.enabled = true;
	}

}
