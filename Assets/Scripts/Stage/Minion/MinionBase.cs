using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MinionFSM))]
public class MinionBase : MonoBehaviour
{	
	//���� ����, ���� ����
	//��Ʈ����Ʈ�� ������Ÿ�ϸ� ���.
	//Ÿ�� = ���ο��� ���� ����� ��
	//Ÿ���� ������ ���� ���¿��� ���� ���·� ����.
	//Ÿ���� �ٽ� ã��, �ش� Ÿ������ �̵�.
	//��ȯ �ð��� ������ ����.

	public MinionStat Stat;		
	public EnemyBase Target;
	public Transform ModelTrf;
	public bool IsDie = false;
	Vector2 min;
	Vector2 max;
	MinionChart data;
	Coroutine cMove = null;
	Vector2 pos;
	Vector2 dir;
	Vector2 targetPos;
	float mTimerCurrent = 0f;

	public void Init(MinionChart chart, HeroBase caster, float durationTime)
	{
		data = chart;
		Stat = new MinionStat(chart, caster);

		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		Target = null;
		IsDie = false;
		cMove = null;

		pos = transform.position;
		dir = Vector2.up;

		StartCoroutine(UnsummonSequence(durationTime));
		GetComponent<MinionFSM>().SetFSM(this);
	}

	private void Start()
	{
		MinionChart chart = CsvData.Ins.MinionChart["M2"];
		data = chart;

		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		Target = null;
		IsDie = false;
		cMove = null;

		pos = transform.position;
		dir = Vector2.up;

		StartCoroutine(UnsummonSequence(10000f));
		GetComponent<MinionFSM>().SetFSM(this);
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
				List<EnemyBase> enemies = EnemyBase.Enemies.OrderBy(a => Vector2.Distance(a.transform.position, StageManager.Ins.PlayerLine.position)).ToList();

				for (int i = 0; i < enemies.Count; i++)
				{
					if (enemies[i].transform.position.x < max.x && enemies[i].transform.position.x > min.x &&
						enemies[i].transform.position.y < max.y && enemies[i].transform.position.y > min.y)
					{
						if(enemies[i].Stat.CurHp > 0f)
						{
							Target = enemies[i];
							targetPos = Target.transform.position;
							break;
						}
					}
				}
			}
			yield return null;
		}
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
			ModelTrf.up = dir;

			switch (data.MoveType)
			{
				case MoveType.Direct:
					transform.Translate(dir * 5f * Time.deltaTime);
					break;
				case MoveType.Curve:
					mTimerCurrent += Time.deltaTime * (5f / 20f);
					_pos = transform.position.WithZ(0f);

					if (mTimerCurrent >= 1f)
					{
						Target = null;
					}
					else
					{
						if (Target.Stat.CurHp > 0f)
						{
							targetPos = Target.transform.position;
						}
						else
						{
							Target = null;
							yield break;
						}

						Vector2 pos1 = new Vector2(pos.x + pos1R.x, pos.y + pos1R.y);
						Vector2 pos2 = new Vector2(targetPos.x + pos2R.x, targetPos.y + pos2R.y);

						transform.position = BezierValue(pos1, pos2, mTimerCurrent);

						if (mTimerCurrent < 1f)
						{
							dir = (transform.position.WithZ(0f) - _pos.WithZ(0f)).normalized;
						}
					}
					break;
			}

			yield return null;
		}		
	}


	IEnumerator UnsummonSequence(float durationTime)
	{
		yield return new WaitForSeconds(durationTime);

		IsDie = true;
	}

	public void Unsummon()
	{

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


}

public class MinionStat
{
	public double Atk;
	public float Spd;

	public MinionStat(MinionChart chart, HeroBase caster)
	{
		Atk = caster.Stat.Atk * (chart.AtkP / 100f);
		Spd = chart.Spd;
	}
}