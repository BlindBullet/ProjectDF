using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileController : MonoBehaviour
{	
	ProjectileChart pChart;
	ProjectileData data;
	List<HitresultChart> hitresults = new List<HitresultChart>();
	public Transform ModelTrf;
	public TrailRenderer[] Trail;
	HeroBase caster;
	MinionBase minion;
	int penCount = 0;
	Vector2 dir;
	EnemyBase target;
	Vector2 targetPos;
	Vector2 pos;
	double playerAtk = 0f;
	bool existTarget;
	float angle;
	MoveType moveType;
	float lifeTime;

	float mTimerCurrent = 0f;	

	public void Setup(ProjectileChart chart, List<HitresultChart> hitresults, HeroBase caster, Vector2 dir, EnemyBase target = null)
	{
		pos = new Vector2(this.transform.position.x, this.transform.position.y);
		data = new ProjectileData();
		data.Setup(chart);
		this.hitresults = hitresults;
		this.caster = caster;
		this.minion = null;
		this.dir = dir;		

		if (target == null)
		{
			existTarget = false;
		}
		else
		{			
			existTarget = true;
			this.target = target;
			targetPos = target.transform.position;
		}

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = caster.Stat.PenCount;
		mTimerCurrent = 0f;

		StartCoroutine(MoveSequence());
	}

	public void Setup(ProjectileChart chart, List<HitresultChart> hitresults, MinionBase minion, Vector2 dir, EnemyBase target = null)
	{
		//������Ÿ���� ���� ��ü�� ��ȯ������ ���������� ����
		//��ȯ���� ��Ʈ����Ʈ�� �����ϴ� �޼��� �߰�
		
		pos = new Vector2(this.transform.position.x, this.transform.position.y);
		data = new ProjectileData();
		data.Setup(chart);
		this.hitresults = hitresults;
		this.caster = null;
		this.minion = minion;
		this.dir = dir;		

		if (target == null)
		{
			existTarget = false;
		}
		else
		{
			existTarget = true;
			this.target = target;
			targetPos = target.transform.position;
		}

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = minion.Stat.PenCount;
		mTimerCurrent = 0f;

		StartCoroutine(MoveSequence());
	}

	public void Setup(ProjectileChart chart, double atk, Vector2 dir, EnemyBase target = null)
	{
		//������Ÿ���� ���� ��ü�� ��ȯ������ ���������� ����
		//��ȯ���� ��Ʈ����Ʈ�� �����ϴ� �޼��� �߰�

		pos = new Vector2(this.transform.position.x, this.transform.position.y);
		data = new ProjectileData();
		data.Setup(chart);
		this.hitresults = null;
		this.caster = null;
		this.minion = null;
		this.dir = dir;		
		playerAtk = atk;		

		if (target == null)
		{
			existTarget = false;
		}
		else
		{
			existTarget = true;
			this.target = target;
			targetPos = target.transform.position;
		}

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = 0;
		mTimerCurrent = 0f;

		StartCoroutine(MoveSequence());
	}

	//���� �Ϲ� ����
	public void Setup(HeroBase caster, AttackData attackData, float angle, Vector2 dir, EnemyBase target = null)
	{
		pos = new Vector2(this.transform.position.x, this.transform.position.y);

		data = new ProjectileData();
		data.Setup(caster, attackData, angle);
		this.hitresults = null;
		this.caster = caster;
		this.minion = null;
		this.dir = dir;

		if (target == null)
		{
			existTarget = false;
		}
		else
		{
			existTarget = true;
			this.target = target;
			targetPos = target.transform.position;
		}

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = caster.Stat.PenCount + attackData.Piercing;
		mTimerCurrent = 0f;

		StartCoroutine(MoveSequence());
	}

	IEnumerator MoveSequence()
	{
		if (Trail.Length > 0)
		{
			for(int i = 0; i < Trail.Length; i++)
			{
				Trail[i].Clear();
			}
		}

		float time = 0;
		Vector3 _pos;
		dir = Quaternion.Euler(0, 0, data.Angle) * dir;
		Vector2 pos1R = Vector2.zero;
		Vector2 pos2R = Vector2.zero;

		if(data.MoveType == MoveType.Curve)
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
					transform.Translate(dir * data.Speed * Time.deltaTime);
					break;
				case MoveType.Curve:
					mTimerCurrent += Time.deltaTime * (data.Speed / 20f);
					_pos = transform.position.WithZ(0f);
					
					//������ ���� �� �ӵ��� ���ؾ���.

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
				if(caster != null)
				{
					if (data.DestroyFx != null)
						EffectManager.Ins.ShowFx(data.DestroyFx, this.transform.position);

					if (data.DestroyResult != null)
					{
						List<ResultGroupChart> destroyResultGroups = CsvData.Ins.ResultGroupChart[data.DestroyResult];
						HitresultManager.Ins.RunResultGroup(destroyResultGroups, transform.position, caster);
					}
				}

				DestroySequence();
			}				

			yield return null;
		}		
	}

	void DestroySequence()
	{
		this.transform.position = new Vector3(0, -100f, 0);

		if (Trail.Length > 0)
		{
			for (int i = 0; i < Trail.Length; i++)
			{
				Trail[i].Clear();
			}
		}
		
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
			Vector2 pos = enemyBase.transform.position;

			if (data.OnlyHitTarget && target != null)
			{				
				if(enemyBase != target)
				{
					return;
				}
			}

			if (caster != null && minion == null && hitresults != null)
			{
				HitresultManager.Ins.SendHitresult(hitresults, enemyBase, caster);
			}
			else if(minion != null && caster == null && hitresults != null)
			{
				HitresultManager.Ins.SendHitresult(hitresults, enemyBase, minion);
			}
			else if(caster == null && minion == null && hitresults == null)
			{
				EffectManager.Ins.ShowFx(ConstantData.PlayerTouchAtkHitFx, enemyBase.transform);
				double resultDmg = enemyBase.TakeDmg(playerAtk, Attr.None, false, 0f);
				FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToCurrencyString(), false);
			}
			else if(caster != null && minion == null && hitresults == null)
			{
				EffectManager.Ins.ShowFx("TestHitFx", enemyBase.transform);
				bool isCrit = Random.Range(0, 100f) <= caster.Stat.CritChance ? true : false;
				double resultDmg = enemyBase.TakeDmg(caster.Stat.Atk, caster.Stat.Attr, isCrit, 0f);
				FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToCurrencyString(), false);
			}
			
			if(penCount <= 0)
			{	
				if (data.HitDestroyFx != null)
				{
					EffectManager.Ins.ShowFx(data.HitDestroyFx, pos);
				}

				if (data.HitDestroyResult != null && caster != null && minion == null)
				{
					List<ResultGroupChart> hitResultGroups = CsvData.Ins.ResultGroupChart[data.HitDestroyResult];
					HitresultManager.Ins.RunResultGroup(hitResultGroups, pos, caster);
				}
				else if(data.HitDestroyResult != null && caster == null && minion != null)
				{						
					List<ResultGroupChart> hitResultGroups = CsvData.Ins.ResultGroupChart[data.HitDestroyResult];
					HitresultManager.Ins.RunResultGroup(hitResultGroups, pos, minion);
				}

				penCount--;
								
				DestroySequence();
			}	
		}
	}


}

public class ProjectileData
{
	public bool OnlyHitTarget;
	public MoveType MoveType;	
	public float Angle;
	public float Speed;
	public float[] BPos1;
	public float[] BPos1R;
	public float[] BPos2;
	public float[] BPos2R;
	public float Lifetime;	
	public string BeginFx;
	public string HitDestroyFx;
	public string DestroyFx;
	public string HitDestroyResult;
	public string DestroyResult;

	public void Setup(ProjectileChart chart)
	{
		OnlyHitTarget = chart.OnlyHitTarget;
		MoveType = chart.MoveType;		
		Angle = chart.Angle;
		Speed = chart.Speed;
		BPos1 = chart.BPos1;
		BPos1R = chart.BPos1R;
		BPos2 = chart.BPos2;
		BPos2R = chart.BPos2R;
		Lifetime = chart.Lifetime;		
		BeginFx = chart.BeginFx;
		HitDestroyFx = chart.HitDestroyFx;
		DestroyFx = chart.DestroyFx;
		HitDestroyResult = chart.HitDestroyResult;
		DestroyResult = chart.DestroyResult;
	}

	public void Setup(HeroBase caster, AttackData data, float angle)
	{		
		OnlyHitTarget = false;
		MoveType = MoveType.Direct;
		Angle = angle;
		Speed = 20f;
		Lifetime = 5f;
		DestroyFx = null;
		DestroyResult = null;

		switch (caster.Stat.Attr)
		{
			case Attr.Red:
				BeginFx = null;
				HitDestroyFx = null;
				break;
			case Attr.Blue:
				BeginFx = null;
				HitDestroyFx = null;
				break;
			case Attr.Green:
				BeginFx = null;
				HitDestroyFx = null;
				break;
		}

		switch (data.Boom)
		{
			case 1:
				HitDestroyResult = null;
				break;
			case 2:
				HitDestroyResult = null;
				break;
			case 3:
				HitDestroyResult = null;
				break;
		}

	}

}