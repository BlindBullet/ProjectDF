using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DeploySlot : MonoBehaviour
{
	public static List<DeploySlot> Slots = new List<DeploySlot>();

	public int SlotNo;
	public Button Btn;
	public HeroIcon HeroIcon;
	public TextMeshProUGUI SlotNoText;
	public GameObject Arrow;
	public bool isDeploying = false;
	HeroData data;

	public void SetSlot(HeroData data, int slotNo)
	{
		this.data = data;
		SlotNo = slotNo;
		HeroIcon.Setup(data, OpenHeroInfo);
		SlotNoText.text = slotNo.ToString();
		Arrow.SetActive(false);
		Btn.gameObject.SetActive(false);
		Slots.Add(this);
	}

	void OpenHeroInfo(HeroData data)
	{
		if(!isDeploying)
			DialogManager.Ins.OpenHeroInfo(data);
	}

	public void SetDeployState(HeroData data)
	{
		isDeploying = true;
		Btn.gameObject.SetActive(true);
		OpenArrow();
		Btn.onClick.RemoveAllListeners();
		Btn.onClick.AddListener(() => { Deploy(data); });
	}

	void OpenArrow()
	{
		Arrow.SetActive(true);
		Arrow.transform.DOMoveY(Arrow.transform.position.y - 1f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
	}

	void Deploy(HeroData data)
	{		
		StageManager.Ins.DeployHero(data, SlotNo);		
		HeroIcon.Setup(data, OpenHeroInfo);		
		DialogHero._DialogHero.EndDeployState();
		SEManager.Ins.Apply();
	}

	public void EndDeployState()
	{
		isDeploying = false;
		Arrow.SetActive(false);
		Btn.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		Slots.Remove(this);
	}

}
