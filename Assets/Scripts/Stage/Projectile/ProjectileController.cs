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
	Rigidbody2D Rb;
	HeroBase caster;
	MinionBase minion;
	int penCount = 0;
	int bounce = 0;
	Vector2 dir;
	EnemyBase target;
	Vector2 targetPos;
	Vector2 pos;
	double atk = 0f;
	bool existTarget;
	//float angle;
	//MoveType moveType;
	//float lifeTime;
	//private Vector3 _lastBezierDir;

	//영웅 스킬로 생성되는 프로젝타일
	public void Setup(ProjectileChart chart, List<HitresultChart> hitresults, HeroBase caster, Vector2 dir, EnemyBase target = null)
	{
		Rb = GetComponent<Rigidbody2D>();
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

		if (data.BeginSfx != null)
			SoundManager.Ins.PlaySFX(data.BeginSfx);

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = data.PenCount;
		bounce = data.Bounce;		

		StartCoroutine(MoveSequence());
	}

	//소환수 스킬로 생성되는 프로젝타일
	public void Setup(ProjectileChart chart, List<HitresultChart> hitresults, MinionBase minion, Vector2 dir, EnemyBase target = null)
	{		
		Rb = GetComponent<Rigidbody2D>();
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

		if(data.BeginSfx != null)
			SoundManager.Ins.PlaySFX(data.BeginSfx);

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = data.PenCount;
		bounce = data.Bounce;

		StartCoroutine(MoveSequence());
	}

	//플레이어 터치로 생성되는 프로젝타일
	public void Setup(ProjectileChart chart, double atk, Vector2 dir, EnemyBase target = null)
	{		
		Rb = GetComponent<Rigidbody2D>();
		pos = new Vector2(this.transform.position.x, this.transform.position.y);
		data = new ProjectileData();
		data.Setup(chart);
		this.hitresults = null;
		this.caster = null;
		this.minion = null;
		this.dir = dir;		
		this.atk = atk;		

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

		SoundManager.Ins.PlayNASFX("poly_shoot_magic");

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = data.PenCount;
		bounce = data.Bounce;		
				
		StartCoroutine(MoveSequence());
	}

	//영웅 일반 공격
	public void Setup(HeroBase caster, AttackData attackData, float angle, Vector2 dir, EnemyBase target = null)
	{
		Rb = GetComponent<Rigidbody2D>();
		pos = new Vector2(this.transform.position.x, this.transform.position.y);
		
		data = new ProjectileData();
		data.Setup(caster, attackData, angle);
		this.hitresults = null;
		this.caster = caster;
		this.minion = null;
		this.dir = dir;

		atk = caster.Stat.Atk;		
		atk = atk + (atk * (attackData.AtkUp / 5f));

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

		switch (caster.Stat.Attr)
		{
			case Attr.Red:
				SoundManager.Ins.PlayNASFX("poly_shoot_arrow");
				break;
			case Attr.Blue:
				SoundManager.Ins.PlayNASFX("poly_shoot_arrow");
				break;
			case Attr.Green:
				SoundManager.Ins.PlayNASFX("poly_shoot_arrow");
				break;
		}

		if (data.BeginFx != null)
			EffectManager.Ins.ShowFx(data.BeginFx, this.transform);

		penCount = data.PenCount;
		bounce = data.Bounce;		

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

		if (data.MoveType == MoveType.Direct)
		{
			float _time = 0f;
			
			while (_time < data.Lifetime)
			{
				ModelTrf.up = dir;
				Rb.velocity = dir * data.Speed;

				yield return null;

				_time += Time.deltaTime;
			}

			ShowDestroyEffect();
			DestroySequence();
		}
		else if(data.MoveType == MoveType.Curve)
		{
			float startTime = Time.timeSinceLevelLoad;
			// 거리 / 속도 = 시간
			var beginDir = targetPos - transform.position.xy();
			float bezierDuration = (beginDir.magnitude / data.Speed) * 2f;
			Vector2 pos1 = new Vector2(pos.x + pos1R.x, pos.y + pos1R.y);
			Vector2 pos2 = new Vector2(targetPos.x + pos2R.x, targetPos.y + pos2R.y);
			Vector2 dirCache = beginDir;

			for (float curTime = startTime; curTime < startTime + bezierDuration; curTime = Time.timeSinceLevelLoad)
			{
				float alpha = (curTime - startTime) / bezierDuration;
				var curPos = Rb.position;
				var newPos = BezierValue(pos1, pos2, alpha);
				dirCache = newPos - curPos;
				Rb.position = newPos;
				ModelTrf.up = dirCache;
				if (curTime - startTime > data.Lifetime) break;
				yield return null;
			}

			Rb.velocity = dirCache.normalized * data.Speed;

			yield return new WaitForSeconds(data.Lifetime - bezierDuration);

			ShowDestroyEffect();
			DestroySequence();
		}
		else if(data.MoveType == MoveType.Beam)
		{
			//float startTime = Time.timeSinceLevelLoad;

			//for (float curTime = startTime; curTime < startTime + data.Lifetime; curTime = Time.timeSinceLevelLoad)
			//{
			//	var totalCount = TrailRenderer.pivot.Count;
			//	for (int i = 0; i < totalCount; i++)
			//	{
			//		trailRenderer[i] = BezierValue(pos1, pos2, i / totalCount);
			//	}
			//	//duration 따라 break;
			//	yield return null;
			//}
			//ShowDestroyEffect();
			//DestroySequence();
		}
		else if(data.MoveType == MoveType.HomingBeam)
		{
			//float startTime = Time.timeSinceLevelLoad;
			//for (float curTime = startTime; curTime < startTime + data.Lifetime; curTime = Time.timeSinceLevelLoad)
			//{
			//	// trailRenderer.pivot[0] = 발사한사람
			//	//trailRenderer.pivot[1] = 타겟
			//	//duration 따라 break;
			//	yield return null;
			//}
			//ShowDestroyEffect();
			//DestroySequence();
		}

		
	}

	void ShowDestroyEffect()
	{
		if (caster != null)
		{
			if (data.DestroyFx != null)
				EffectManager.Ins.ShowFx(data.DestroyFx, this.transform.position);

			if (data.DestroyResult != null)
			{
				List<ResultGroupChart> destroyResultGroups = CsvData.Ins.ResultGroupChart[data.DestroyResult];
				HitresultManager.Ins.RunResultGroup(destroyResultGroups, transform.position, caster);
			}
		}
	}

	void DestroySequence()
	{
		this.transform.position = new Vector3(0, -100f, 0);
		this.gameObject.SetActive(false);
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Wall"))
		{			
			if (bounce > 0)
			{
				bounce--;
				//atk = atk - (atk * (ConstantData.BounceDmgDecP / 100f));
				dir = Vector2.Reflect(dir, collision.contacts[0].normal);
				ModelTrf.up = dir;
				return;
			}
			else
			{				
				DestroySequence();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
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

			//영웅 스킬
			if (caster != null && minion == null && hitresults != null)
			{
				HitresultManager.Ins.SendHitresult(hitresults, enemyBase, caster);
			}
			//소환수 공격
			else if (minion != null && caster == null && hitresults != null)
			{
				HitresultManager.Ins.SendHitresult(hitresults, enemyBase, minion);
			}
			//플레이어 터치 공격
			else if (caster == null && minion == null && hitresults == null)
			{				
				EffectManager.Ins.ShowFx("TouchHitFx", enemyBase.transform);
				double resultDmg = enemyBase.TakeDmg(atk, Attr.None, false, 0f);
				FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToCurrencyString(), false, false);
			}
			//영웅 일반 공격
			else if(caster != null && minion == null && hitresults == null)
			{
				switch (caster.Stat.Attr)
				{
					case Attr.Red:						
						EffectManager.Ins.ShowFx("HitFxRed", enemyBase.transform);
						break;
					case Attr.Blue:
						EffectManager.Ins.ShowFx("HitFxBlue", enemyBase.transform);
						break;
					case Attr.Green:
						EffectManager.Ins.ShowFx("HitFxGreen", enemyBase.transform);
						break;
				}

				bool isCrit2 = Random.Range(0, 100f) <= caster.Stat.CritChance2 ? true : false;
				bool isCrit = Random.Range(0, 100f) <= caster.Stat.CritChance ? true : false;
				atk = isCrit ? atk + (atk * (caster.Stat.CritDmg / 100f)) : atk;				
				atk = isCrit2 ? atk + (atk * (caster.Stat.CritDmg2 / 100f)) : atk;
				double resultDmg = enemyBase.TakeDmg(atk, caster.Stat.Attr, isCrit, 0f);			
				bool isPush = Random.Range(0, 100) < data.PushProb ? true : false;
				if(isPush) enemyBase.Push(data.PushPower, 1f);

				FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToCurrencyString(), isCrit, isCrit2);
			}
			
			if(penCount <= 0)
			{	
				if (data.HitDestroyFx != null)
				{
					EffectManager.Ins.ShowFx(data.HitDestroyFx, pos);
				}

				if (data.HitDestroyResult != null && caster != null && minion == null && hitresults != null)
				{
					List<ResultGroupChart> hitResultGroups = CsvData.Ins.ResultGroupChart[data.HitDestroyResult];
					HitresultManager.Ins.RunResultGroup(hitResultGroups, pos, caster);
				}
				else if(data.HitDestroyResult != null && caster == null && minion != null && hitresults != null)
				{						
					List<ResultGroupChart> hitResultGroups = CsvData.Ins.ResultGroupChart[data.HitDestroyResult];
					HitresultManager.Ins.RunResultGroup(hitResultGroups, pos, minion);
				}
				else if(data.HitDestroyResult != null && caster != null && minion == null && hitresults == null)
				{
					List<ResultGroupChart> hitResultGroups = CsvData.Ins.ResultGroupChart[data.HitDestroyResult];
					HitresultManager.Ins.RunResultGroup(hitResultGroups, pos, caster);
				}
								
				DestroySequence();
			}

			penCount--;
			atk = atk - (atk * (ConstantData.PiercingDmgDecP / 100f));
		}
	}

	void OnDisable()
	{
		ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 
		CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
	}

}

public class ProjectileData
{
	public bool OnlyHitTarget;
	public MoveType MoveType;	
	public float Angle;
	public float Speed;
	public int PenCount;
	public int Bounce;
	public float PushProb;
	public float PushPower;	
	public float[] BPos1;
	public float[] BPos1R;
	public float[] BPos2;
	public float[] BPos2R;
	public float Lifetime;
	public string BeginSfx;
	public string BeginFx;
	public string HitDestroyFx;
	public string DestroyFx;
	public string HitDestroyResult;
	public string DestroyResult;

	public void Setup(ProjectileChart chart)
	{
		OnlyHitTarget = chart.OnlyHitTarget;
		MoveType = chart.MoveType;
		PenCount = chart.PenCount;
		Bounce = chart.Bounce;
		PushPower = 0f;
		PushProb = 0f;
		Angle = chart.Angle;
		Speed = chart.Speed;
		BPos1 = chart.BPos1;
		BPos1R = chart.BPos1R;
		BPos2 = chart.BPos2;
		BPos2R = chart.BPos2R;
		Lifetime = chart.Lifetime;
		BeginSfx = chart.BeginSfx;
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
		PenCount = data.Piercing;
		Bounce = data.Bounce;
		PushPower = data.Push + 2f;
		PushProb = data.Push > 0 ? 2.5f : 0f;
		Angle = angle;
		Speed = 20f;
		Lifetime = 5f;
		DestroyFx = null;
		DestroyResult = null;
		BeginSfx = null;
		BeginFx = null;
		HitDestroyFx = null;
		HitDestroyResult = null;

		switch (caster.Stat.Attr)
		{
			case Attr.Red:				
				switch (data.Boom)
				{
					case 1:
						HitDestroyFx = "boom_red_1";
						HitDestroyResult = "boom_red_1";
						break;
					case 2:
						HitDestroyFx = "boom_red_2";
						HitDestroyResult = "boom_red_2";
						break;
					case 3:
						HitDestroyFx = "boom_red_3";
						HitDestroyResult = "boom_red_3";
						break;
					case 4:
						HitDestroyFx = "boom_red_4";
						HitDestroyResult = "boom_red_4";
						break;
					case 5:
						HitDestroyFx = "boom_red_5";
						HitDestroyResult = "boom_red_5";
						break;
				}
				break;
			case Attr.Blue:
				BeginFx = null;				
				switch (data.Boom)
				{
					case 1:
						HitDestroyFx = "boom_blue_1";
						HitDestroyResult = "boom_blue_1";
						break;
					case 2:
						HitDestroyFx = "boom_blue_2";
						HitDestroyResult = "boom_blue_2";
						break;
					case 3:
						HitDestroyFx = "boom_blue_3";
						HitDestroyResult = "boom_blue_3";
						break;
					case 4:
						HitDestroyFx = "boom_blue_4";
						HitDestroyResult = "boom_blue_4";
						break;
					case 5:
						HitDestroyFx = "boom_blue_5";
						HitDestroyResult = "boom_blue_5";
						break;
				}
				break;
			case Attr.Green:
				BeginFx = null;				
				switch (data.Boom)
				{
					case 1:
						HitDestroyFx = "boom_green_1";
						HitDestroyResult = "boom_green_1";						
						break;
					case 2:
						HitDestroyFx = "boom_green_2";
						HitDestroyResult = "boom_green_2";
						break;
					case 3:
						HitDestroyFx = "boom_green_3";
						HitDestroyResult = "boom_green_3";
						break;
					case 4:
						HitDestroyFx = "boom_green_4";
						HitDestroyResult = "boom_green_4";
						break;
					case 5:
						HitDestroyFx = "boom_green_5";
						HitDestroyResult = "boom_green_5";
						break;
				}
				break;
		}

		

	}

}