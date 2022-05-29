using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackRange))]
[RequireComponent(typeof(SkillController))]
[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(BuffController))]
[RequireComponent(typeof(HeroTween))]
[RequireComponent(typeof(HeroUi))]
[RequireComponent(typeof(CharacterAnchor))]
public class HeroBase : MonoBehaviour
{
	public static List<HeroBase> Heroes = new List<HeroBase>();
		
	public HeroStat Stat;
	[HideInInspector] public HeroUi Ui;
	[HideInInspector] public AttackController AttackCon;
	[HideInInspector] public SkillController SkillCon;
	[HideInInspector] public BuffController BuffCon;
	[HideInInspector] public AttackRange Range;
	[HideInInspector] public HeroTween Tween;
	public CharacterAnchor Anchor;
	public Transform ProjectileAnchor;
	public HeroData Data;

	public void Init(HeroData data, SlotData slotData)
	{
		Data = data;
		Stat = new HeroStat();
		Stat.InitData(data, slotData.Lv);

		Range = GetComponent<AttackRange>();

		Ui = GetComponent<HeroUi>();        
		Ui.SetUp(data, slotData);

		AttackCon = GetComponent<AttackController>();
		AttackCon.Init(this, slotData);

		SkillCon = GetComponent<SkillController>();
		SkillCon.Init(this, data);

		BuffCon = GetComponent<BuffController>();
		BuffCon.Init(this);

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

	IEnumerator LookTarget()
	{
		while (true)
		{			
			if (Range.Targets.Count <= 0)
			{
				Tween.Icon.rotation = Quaternion.identity;                
			}
			else
			{
				EnemyBase target = Range.Targets[0];
				Vector3 dir = target.transform.position - Tween.Icon.position;
				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				Tween.Icon.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
			}

			yield return null;
		}
	}

	public void Destroy()
	{
		Tween.Stop();
		AttackCon.Stop();
		SkillCon.Stop();
		Heroes.Remove(this);
		Destroy(this.gameObject);
	}

	public void Stop()
	{
		AttackCon.Stop();
		SkillCon.Stop();
	}

	public void Lose()
	{
		AttackCon.Stop();
		SkillCon.Stop();
		StartCoroutine(Ui.Die());		
	}


}
