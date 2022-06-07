using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using DG.Tweening;

[RequireComponent(typeof(MinionFSM))]
[RequireComponent(typeof(MinionAttackController))]
public class MinionBase : MonoBehaviour
{
	public static List<MinionBase> Minions = new List<MinionBase>();
	//무브 상태, 공격 상태
	//히트리절트와 프로젝타일만 사용.
	//타겟 = 라인에서 가장 가까운 적
	//타겟이 죽으면 공격 상태에서 무브 상태로 변경.
	//타겟을 다시 찾고, 해당 타겟한테 이동.
	//스테이지가 끝나면 제자리로 돌아감.
	//소환 시간이 끝나면 죽음.

	public MinionStat Stat;
	[HideInInspector] public EnemyBase Target;
	public MinionUi Ui;
	public MinionAttackController AttackCon;
	public Transform ModelTrf;
	public Transform ProjectileAnchor;
	[HideInInspector] public bool IsDie = false;
	Vector2 min;
	Vector2 max;
	MinionChart data;
	Coroutine cMove = null;
	Vector3 originPos;
	Vector2 pos;
	Vector2 dir;
	Vector2 targetPos;
	float mTimerCurrent = 0f;
	
	public void Init(MinionChart chart, HeroBase caster, float durationTime)
	{
		this.transform.localScale = new Vector3(chart.Size, chart.Size, 1);

		data = chart;
		Ui.Setup(chart);
		Stat = new MinionStat(chart, caster);
		AttackCon.SetController(this, data);

		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		Target = null;
		IsDie = false;
		cMove = null;

		originPos = transform.position;
		pos = transform.position;
		dir = Vector2.up;

		StartCoroutine(SummonSequence(durationTime));
		Minions.Add(this);
	}
	IEnumerator SummonSequence(float durationTime)
	{
		Ui.Summon();

		yield return new WaitForSeconds(1.5f);

		GetComponent<MinionFSM>().SetFSM(this);
		StartCoroutine(UnsummonTimer(durationTime));
	}

	public void SearchTarget()
	{
		StartCoroutine(SearchTargetSequence());
	}

	IEnumerator SearchTargetSequence()
	{
		while(Target == null)
		{
			if (EnemyBase.Enemies.Count > 0)
			{
				List<EnemyBase> enemies = EnemyBase.Enemies.OrderBy(a => Vector2.Distance(a.transform.position, this.transform.position)).ToList();

				for (int i = 0; i < enemies.Count; i++)
				{
					if (enemies[i].transform.position.x < max.x && enemies[i].transform.position.x > min.x &&
						enemies[i].transform.position.y < max.y && enemies[i].transform.position.y > min.y)
					{
						if(enemies[i].Stat.CurHp > 0f)
						{
							Target = enemies[i];
							targetPos = Target.transform.position;
							yield break;
						}
					}
				}
			}

			yield return null;
		}
	}

	public void IdleMove()
	{
		transform.DOMove(transform.position.WithY(transform.position.y + 0.05f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetId("IdleMove");
	}

	public void StopIdleMove()
	{
		DOTween.Kill("IdleMove");
	}

	public void Move()
	{
		if (cMove == null)
			cMove = StartCoroutine(MoveSequence());
	}

	public void StopMove()
	{
		if(cMove != null)
		{
			StopCoroutine(cMove);
			cMove = null;
		}	
	}

	IEnumerator MoveSequence()
	{		
		pos = transform.position;
		targetPos = Target.transform.position;
		mTimerCurrent = 0f;
		
		Vector3 _pos;
		Vector2 pos1R = Vector2.zero;
		Vector2 pos2R = Vector2.zero;

		if (data.MoveType == MoveType.Curve)
		{
			if (data.BPos1R != null)
			{
				pos1R = new Vector2(data.BPos1[0] + (Random.Range(-data.BPos1R[0], data.BPos1R[0])), data.BPos1[1] + (Random.Range(-data.BPos1R[1], data.BPos1R[1])));
			}
			else
			{
				pos1R = new Vector2(data.BPos1[0], data.BPos1[1]);
			}

			if (data.BPos2R != null)
			{
				pos2R = new Vector2(data.BPos2[0] + (Random.Range(-data.BPos2R[0], data.BPos2R[0])), data.BPos2[1] + (Random.Range(-data.BPos2R[1], data.BPos2R[1])));
			}
			else
			{
				pos2R = new Vector2(data.BPos2[0], data.BPos2[1]);
			}
		}

		while (true)
		{			
			dir = (Target.transform.position - this.transform.position).normalized;
			ModelTrf.up = dir;

			if(Target.Stat.CurHp <= 0f)
			{
				Target = null;
				yield break;
			}

			switch (data.MoveType)
			{
				case MoveType.Direct:
					if(Target != null)
					{	
						transform.Translate(dir * data.MoveSpd * Time.deltaTime);
					}
					else
					{

					}
					break;
				case MoveType.Curve:
					mTimerCurrent += Time.deltaTime * (5f / 20f);
					_pos = transform.position.WithZ(0f);

					if (Target.Stat.CurHp > 0f)
					{
						targetPos = Target.transform.position;
					}

					Vector2 pos1 = new Vector2(pos.x + pos1R.x, pos.y + pos1R.y);
					Vector2 pos2 = new Vector2(targetPos.x + pos2R.x, targetPos.y + pos2R.y);

					transform.position = BezierValue(pos1, pos2, mTimerCurrent);

					if (mTimerCurrent < 1f)
					{
						dir = (transform.position.WithZ(0f) - _pos.WithZ(0f)).normalized;
					}					
					break;
			}

			yield return null;
		}		
	}


	IEnumerator UnsummonTimer(float durationTime)
	{
		yield return new WaitForSeconds(durationTime);

		IsDie = true;
	}

	public void Unsummon()
	{
		StartCoroutine(UnsummonSequence());
	}

	IEnumerator UnsummonSequence()
	{
		Ui.Unsummon();
		Minions.Remove(this);

		yield return new WaitForSeconds(2f);

		ObjectManager.Ins.Push<MinionBase>(this);
	}

	public bool CalcRange()
	{
		bool result = false;

		float dist = Vector2.Distance(this.transform.position, Target.transform.position);

		if(dist <= data.Range)
		{
			result = true;
		}

		return result;
	}

	Vector2 BezierValue(Vector2 p1, Vector2 p2, float value)
	{
		Vector2 a = Vector2.Lerp(pos, p1, value);
		Vector2 b = Vector2.Lerp(p1, p2, value);
		Vector2 c = Vector2.Lerp(p2, targetPos, value);

		Vector2 d = Vector2.Lerp(a, b, value);
		Vector2 e = Vector2.Lerp(b, c, value);

		Vector2 f = Vector2.Lerp(d, e, value);

		return f;
	}

	public void BackToOriginPos()
	{
		StopIdleMove();

		dir = (originPos - this.transform.position).normalized;
		ModelTrf.up = dir;

		Sequence seq = DOTween.Sequence();
		seq.Append(this.transform.DOMove(originPos, 2f).SetEase(Ease.Linear))
			.AppendCallback(()=> IdleMove());		
	}



}

public class MinionStat
{
	public double Atk;
	public float Spd;
	public int PenCount;
	public float CritChance;
	public float CritDmg;
	public Attr Attr;

	public MinionStat(MinionChart chart, HeroBase caster)
	{
		Attr = chart.Attr;
		Atk = caster.Stat.Atk * (chart.AtkP / 100f);
		Atk = Atk + (Atk * ((StageManager.Ins.PlayerStat.MinionAtkInc[Attr] - StageManager.Ins.PlayerStat.MinionAtkDec[Attr]) / 100f));
		Spd = chart.Spd;
		Spd = Spd + (Spd * ((StageManager.Ins.PlayerStat.MinionSpdInc[Attr] - StageManager.Ins.PlayerStat.MinionSpdDec[Attr]) / 100f));
		PenCount = chart.PenCount;
		CritChance = 0f;
		CritDmg = 100f;
	}
	
}