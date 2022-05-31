using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using TMPro;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemySkillController))]
[RequireComponent(typeof(EnemyBuffController))]
public class EnemyBase : MonoBehaviour
{
	public static List<EnemyBase> Enemies = new List<EnemyBase>();

	public EnemyStat Stat;
	public GameObject Model;
	[HideInInspector] public Rigidbody2D Rb;
	[HideInInspector] public EnemySpriteController SpriteCon;
	CircleCollider2D col;
	Coroutine cMove = null;		
	Coroutine cPush = null;
	Coroutine cStun = null;
	bool isBoss = false;
	bool isDie = false;
	public bool isStunned = false;
	[HideInInspector] public EnemySkillController SkillCon;
	[HideInInspector] public EnemyBuffController BuffCon;
	public Coroutine cHit;
	public TextMeshPro HpText;	

	public void Setup(EnemyChart chart, int stageNo, bool isBoss = false)
	{		
		isDie = false;

		Stat = new EnemyStat();
		Stat.SetStat(chart, stageNo, isBoss);
				
		SetHp();

		this.isBoss = isBoss;

		Rb = GetComponent<Rigidbody2D>();

		col = GetComponent<CircleCollider2D>();
		col.enabled = true;

		SpriteCon = GetComponent<EnemySpriteController>();
		SpriteCon.Setup(this, chart);

		SkillCon = GetComponent<EnemySkillController>();
		SkillCon.Setup(this, chart);

		BuffCon = GetComponent<EnemyBuffController>();
		BuffCon.Setup(this);

		cHit = null;
		Rb.mass = chart.Weight;
		Move();
		Enemies.Add(this);
	}

	public void Move()
	{
		if (cMove == null)
		{
			cMove = StartCoroutine(MoveSequence());
		}	
		else
		{
			StopCoroutine(cMove);
			cMove = StartCoroutine(MoveSequence());
		}
	}

	IEnumerator MoveSequence()
	{
		float scaleX = Model.transform.localScale.x;
		Rb.mass = 1f;

		while (true)
		{
			Vector3 dir = Vector3.down;
			float xSpd = 0f;
			Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
			Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

			if (transform.position.x < min.x)
				xSpd = Stat.Spd;
			if (transform.position.x > max.x)
				xSpd = -Stat.Spd;

			if(transform.position.y <= StageManager.Ins.PlayerLine.transform.position.y + 1.5f)
			{
				Rb.velocity = new Vector2(xSpd, -0.15f);
			}
			else
			{
				Rb.velocity = new Vector2(xSpd, -Stat.Spd);
			}

			yield return null;
		}
	}

	public void StopMove()
	{
		if(cMove != null)
			StopCoroutine(cMove);

		Rb.velocity = Vector2.zero;
		cMove = null;
	}

	public double TakeDmg(double atk, Attr attr, bool isCrit, float stiffTime)
	{
		float def = Stat.Def;

		switch (attr)
		{			
			case Attr.Red:
				if (Stat.Immunes.Contains(Attr.Red))
				{
					atk = 0f;
					break;
				}

				if (Stat.Attr == Attr.Green)
					atk = atk * 2f;
				
				atk = atk - (atk * (def / 100f));
				break;
			case Attr.Green:
				if (Stat.Immunes.Contains(Attr.Green))
				{
					atk = 0f;
					break;
				}
					
				if (Stat.Attr == Attr.Blue)
					atk = atk * 2f;
				
				atk = atk - (atk * (def / 100f));
				break;
			case Attr.Blue:
				if (Stat.Immunes.Contains(Attr.Blue))
				{
					atk = 0f;
					break;
				}	

				if (Stat.Attr == Attr.Red)
					atk = atk * 2f;
				
				atk = atk - (atk * (def / 100f));
				break;
		}

		if (atk < 0f)
			atk = 0f;

		atk = Math.Round(atk);
		Stat.CurHp -= atk;
		SetHp();

		if(cHit == null)
			cHit = StartCoroutine(SpriteCon.Hit(isCrit, stiffTime));

		if (Stat.CurHp <= 0 && !isDie)
		{
			Die();
		}

		return atk;
	}

	public double TakeDmg(float valueP, Attr attr)
	{
		float def = Stat.Def;
		double value = Stat.MaxHp * (valueP / 100f);

		switch (attr)
		{
			case Attr.Red:
				if (Stat.Immunes.Contains(Attr.Red))
				{
					value = 0f;
					break;
				}

				if (Stat.Attr == Attr.Green)
					value = value * 2f;

				value = value - (value * (def / 100f));
				break;
			case Attr.Green:
				if (Stat.Immunes.Contains(Attr.Green))
				{
					value = 0f;
					break;
				}

				if (Stat.Attr == Attr.Blue)
					value = value * 2f;

				value = value - (value * (def / 100f));
				break;
			case Attr.Blue:
				if (Stat.Immunes.Contains(Attr.Blue))
				{
					value = 0f;
					break;
				}

				if (Stat.Attr == Attr.Red)
					value = value * 2f;

				value = value - (value * (def / 100f));
				break;
		}

		if (value < 0f)
			value = 0f;

		Stat.CurHp -= value;
		SetHp();

		SpriteCon.Hit(false, 0f);

		if (Stat.CurHp <= 0 && !isDie)
		{
			Die();
		}

		return value;
	}

	public void TakeHeal(float value)
	{
		double resultValue = Stat.MaxHp * (value / 100f);
		
		if (Stat.CurHp + resultValue >= Stat.MaxHp)
		{
			resultValue = Stat.MaxHp - Stat.CurHp;			
		}

		Stat.CurHp = Stat.CurHp + resultValue;

		SetHp();

		if(resultValue > 0f)
			FloatingTextManager.Ins.ShowHeal(this.transform.position, resultValue.ToCurrencyString());
	}

	void SetHp()
	{
		if (Stat.CurHp <= 0)
			HpText.text = "";
		else
			HpText.text = ExtensionMethods.ToCurrencyString(Stat.CurHp);
	}

	public void Die()
	{
		isDie = true;		
		StartCoroutine(DieSequence());
	}

	IEnumerator DieSequence()
	{	
		StopMove();
		Rb.mass = 100f;
		Rb.velocity = Vector2.zero;

		SkillCon.UseDieSkill();

		Enemies.Remove(this);

		SpriteCon.Die();

		yield return new WaitForSeconds(0.5f);

		double getGold = Stat.Gold;

		StageManager.Ins.GetGold(getGold);
		FloatingTextManager.Ins.ShowGold(this.transform.position, "+" + getGold.ToCurrencyString());
		col.enabled = false;
		SpriteCon.Mask.enabled = false;

		yield return new WaitForSeconds(1f);

		transform.position = new Vector3(0, 20f, 0);
		SpriteCon.Mask.enabled = true;
		ObjectManager.Ins.Push<EnemyBase>(this);
	}

	public void Push(float value, float time)
	{
		StopMove();

		if (cPush == null)
		{
			cPush = StartCoroutine(PushSequence(value, time));
		}	
		else
		{
			StopCoroutine(cPush);
			cPush = StartCoroutine(PushSequence(value, time));
		}
	}

	IEnumerator PushSequence(float value, float time)
	{
		isStunned = true;
		Rb.mass = 100f;
		Rb.AddForce(new Vector2(0, value * 1000f));

		yield return new WaitForSeconds(time);
			
		cPush = null;

		if(cStun == null)
		{
			Move();
			isStunned = false;
		}	
	}

	public void Stun(float time)
	{
		StopMove();

		if(cStun == null)
		{
			cStun = StartCoroutine(StunSequence(time));
		}
		else
		{
			StopCoroutine(cStun);
			cStun = StartCoroutine(StunSequence(time));
		}
	}

	IEnumerator StunSequence(float time)
	{
		isStunned = true;
		Rb.mass = 100f;

		yield return new WaitForSeconds(time);

		cStun = null;

		if (cPush == null)
		{
			isStunned = false;
			Move();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{			
			StageManager.Ins.LoseStage();
		}
	}

	public void Stop()
	{
		if(cMove != null)
			StopCoroutine(cMove);

		Rb.mass = 100f;
		Rb.velocity = Vector2.zero;
	}

	public void Destroy()
	{
		transform.position = new Vector3(0, 20f, 0);
		ObjectManager.Ins.Push<EnemyBase>(this);
		Enemies.Remove(this);
	}

	
}