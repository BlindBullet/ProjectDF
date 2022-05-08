using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileController : MonoBehaviour
{	
	ProjectileChart data;
	List<HitresultChart> hitresults = new List<HitresultChart>();
	public Transform ModelTrf;
	HeroBase caster;
	int penCount;
	Vector2 dir;
	EnemyBase target;
	Vector2 targetPos;
	Vector2 pos;
	bool existTarget;

	float mTimerCurrent = 0f;	

	public void Setup(ProjectileChart data, List<HitresultChart> hitresults, HeroBase caster, Vector2 dir, EnemyBase target = null)
	{
		pos = new Vector2(this.transform.position.x, this.transform.position.y);
		this.data = data;
		this.hitresults = hitresults;
		this.caster = caster;
		this.dir = dir;
		if(target == null)
		{
			existTarget = false;
		}
		else
		{			
			existTarget = true;
			this.target = target;
			targetPos = target.transform.position;
		}		

		mTimerCurrent = 0f;		

		StartCoroutine(MoveSequence());
	}

	IEnumerator MoveSequence()
	{
		float time = 0;
		Vector3 _pos;
		dir = Quaternion.Euler(0, 0, data.Angle) * dir;
		Vector2 pos1R = Vector2.zero;
		Vector2 pos2R = Vector2.zero;

		if(data.MoveType == PMoveType.Curve)
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
				case PMoveType.Direct:
					transform.Translate(dir * data.Speed * Time.deltaTime);
					break;
				case PMoveType.Curve:
					mTimerCurrent += Time.deltaTime * (data.Speed / 20f);
					_pos = transform.position.WithZ(0f);
					
					//마지막 방향 및 속도를 구해야함.

					if (mTimerCurrent >= 1f)
					{
						DestroySequence();
					}
					else
					{
						if (target.Stat.CurHp > 0f)
						{
							targetPos = target.transform.position;
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

			time += Time.deltaTime;

			if(time >= data.Lifetime)
			{
				DestroySequence();
			}				

			yield return null;
		}		
	}

	void DestroySequence()
	{
		if (data.DestroyFx != null)
			EffectManager.Ins.ShowFx(data.DestroyFx, this.transform.position);

		if (data.DestroyResultGroupId != null)
		{
			List<ResultGroupChart> destroyResultGroups = CsvData.Ins.ResultGroupChart[data.DestroyResultGroupId];
			HitresultManager.Ins.RunResultGroup(destroyResultGroups, transform.position, caster);
		}

		this.transform.position = new Vector3(0, -100f, 0);
		ObjectManager.Ins.Push<ProjectileController>(this);
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

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{	
			EnemyBase enemyBase = collision.GetComponent<EnemyBase>();
			HitresultManager.Ins.SendHitresult(hitresults, enemyBase, caster);
			
			if (data.HitResultGroupId != null)
			{
				List<ResultGroupChart> hitResultGroups = CsvData.Ins.ResultGroupChart[data.HitResultGroupId];
				HitresultManager.Ins.RunResultGroup(hitResultGroups, transform.position, caster);
			}

			if(penCount == 0)
			{
				if (data.HitDestroyFx != null)
					EffectManager.Ins.ShowFx(data.HitDestroyFx, this.transform.position);
				
				penCount--;
								
				DestroySequence();				
			}	
		}
	}


}
