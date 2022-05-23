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
	public EnemySkillController SkillCon;
	public EnemyBuffController BuffCon;

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
		SpriteCon.Setup(chart);

		SkillCon = GetComponent<EnemySkillController>();
		SkillCon.Setup(this, chart);

		BuffCon = GetComponent<EnemyBuffController>();
		BuffCon.Setup(this);

		Rb.mass = chart.Weight;
		Move();
		Enemies.Add(this);
	}

	void Move()
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

			Rb.velocity = new Vector2(xSpd, -Stat.Spd);

			yield return null;
		}
	}

	void StopMove()
	{
		if(cMove != null)
			StopCoroutine(cMove);

		Rb.velocity = Vector2.zero;
		cMove = null;
	}

	public double TakeDmg(double atk, Attr attr, bool isCrit, float stiffTime)
	{
		float def = 0f;

		switch (attr)
		{
			case Attr.None:
				def += Stat.Def;
				atk = atk - (atk * (def / 100f));
				break;
			case Attr.Red:
				if (Stat.Immunes.Contains(Attr.Red))
				{
					atk = 0f;
					break;
				}

				if (Stat.Attr == Attr.Green)
					atk = atk * 2f;

				def += Stat.Def;

				if (Stat.AttrDefs.ContainsKey(Attr.Red))
					def += Stat.AttrDefs[Attr.Red];

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

				def += Stat.Def;

				if (Stat.AttrDefs.ContainsKey(Attr.Green))
					def += Stat.AttrDefs[Attr.Green];

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

				def += Stat.Def;

				if (Stat.AttrDefs.ContainsKey(Attr.Blue))
					def += Stat.AttrDefs[Attr.Blue];

				atk = atk - (atk * (def / 100f));
				break;
		}

		if (atk < 0f)
			atk = 0f;

		atk = Math.Round(atk);
		Stat.CurHp -= atk;
		SetHp();

		SpriteCon.Hit(isCrit, stiffTime);

		if (Stat.CurHp <= 0 && !isDie)
		{
			Die();
		}	

		return atk;
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

		StageManager.Ins.GetGold(Stat.Gold);
		FloatingTextManager.Ins.ShowGold(this.transform.position, "+" + Stat.Gold.ToCurrencyString());
		col.enabled = false;

		yield return new WaitForSeconds(1f);

		transform.position = new Vector3(0, 20f, 0);		
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