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
	public GameObject LockPanel;
	public GameObject LockIcon;
	public GameObject[] Stars;
	public GameObject SelectedFrame;
	public HeroData Data;
	Material selectedFrameMat;

	private void Awake()
	{
		Image selectedFrameImg = SelectedFrame.GetComponent<Image>();
		selectedFrameImg.material = new Material(selectedFrameImg.material);
		selectedFrameMat = selectedFrameImg.material;
	}

	public void Setup(HeroData data, Action<HeroData> action = null)
	{
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

	public void Setup(HeroData data, int slotNo, Action<HeroData> action = null)
	{

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
			LockPanel.SetActive(true);
			LockIcon.SetActive(true);
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
		selectedFrameMat.DOFloat(6.28f, "_ShineRotate", 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetId("SF");
	}

	public void CloseSelectedFrame()
	{
		DOTween.Kill("SF");
		SelectedFrame.SetActive(false);
	}

	public void DiasbleBtns(bool lockPanelOff = false)
	{
		if(!lockPanelOff)
			LockPanel.SetActive(true);

		Btn.enabled = false;
	}

	public void EnableBtns()
	{
		LockPanel.SetActive(false);
		Btn.enabled = true;
	}

}
