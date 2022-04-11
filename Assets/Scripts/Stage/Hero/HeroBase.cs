using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackRange))]
[RequireComponent(typeof(SkillController))]
[RequireComponent(typeof(HeroTween))]
[RequireComponent(typeof(HeroUi))]
public class HeroBase : MonoBehaviour
{
	public static List<HeroBase> Heroes = new List<HeroBase>();
	
	public Stat Stat;
	[HideInInspector] public HeroUi Ui;
	[HideInInspector] public SkillController SkillCon;
	[HideInInspector] public AttackRange Range;
	[HideInInspector] public HeroTween Tween;
	public Transform ProjectileAnchor;
	public HeroData Data;

	public void Init(HeroData data, SlotData slotData)
	{
		Data = data;
		Stat = new Stat();
		Stat.InitData(data, slotData.Lv);

		Range = GetComponent<AttackRange>();

		Ui = GetComponent<HeroUi>();        
		Ui.SetUp(data, slotData);

		SkillCon = GetComponent<SkillController>();
		SkillCon.Init(this, data);

		Tween = GetComponent<HeroTween>();
		Tween.SetTween();

		Range.StartSearch(this);
		Heroes.Add(this);

		StartCoroutine(LookTarget());
	}

	public void ChangeLv(int lv)
	{
		Stat.ChangeLv(lv);		
	}

	public void Appear()
	{
		//죽음 이후 스테이지가 다시 시작될 때 살아나기

		SkillCon.ReStart();
	}

	public void Die()
	{
		SkillCon.Stop();

		//죽음 연출
	}

	IEnumerator LookTarget()
	{
		while (true)
		{
			if (Range.AllTargetColl.Count <= 0)
			{
				Tween.Icon.rotation = Quaternion.identity;                
			}
			else
			{
				EnemyBase target = Range.AllTargetColl[0].GetComponent<EnemyBase>();
				Vector3 dir = target.transform.position - Tween.Icon.position;
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				Tween.Icon.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
			}

			yield return null;
		}
	}

	public void ChangeSpeed(float value)
	{
		Stat.ChangeSpeed(value);    
	}

	public void Destroy()
	{
		SkillCon.Stop();
		Destroy(this.gameObject);
	}

}
