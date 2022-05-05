using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using TMPro;
using Random = UnityEngine.Random;

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
		
	public TextMeshPro HpText;

	public void Setup(EnemyChart chart, int stageNo, bool isBoss = false)
	{
		Stat = new EnemyStat();
		Stat.SetStat(chart, stageNo, isBoss);
				
		SetHp();

		this.isBoss = isBoss;

		Rb = GetComponent<Rigidbody2D>();

		col = GetComponent<CircleCollider2D>();
		col.enabled = true;

		SpriteCon = GetComponent<EnemySpriteController>();
		SpriteCon.Setup(chart);

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

			if (transform.position.y <= StageManager.Ins.PlayerLine.position.y)
				StageManager.Ins.LoseStage();

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
		switch (attr)
		{
			case Attr.None:
				break;
			case Attr.Red:
				if (Stat.Attr == Attr.Green)
					atk = atk * 2f;
				break;
			case Attr.Green:
				if (Stat.Attr == Attr.Blue)
					atk = atk * 2f;
				break;
			case Attr.Blue:
				if (Stat.Attr == Attr.Red)
					atk = atk * 2f;
				break;
		}

		atk = Math.Round(atk);
		Stat.CurHp -= atk;
		SetHp();

		SpriteCon.Hit(isCrit, stiffTime);

		if (Stat.CurHp <= 0)
			Die();

		return atk;
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
		StageManager.Ins.GetGold(Stat.Gold);
		StopMove();

		Rb.mass = 100f;		
		Rb.velocity = Vector2.zero;

		Enemies.Remove(this);

		if (isBoss)
		{
			for(int i = 0; i < Enemies.Count; i++)
			{
				Enemies[i].Die();
			}
		}	

		StartCoroutine(DieSequence());
	}

	IEnumerator DieSequence()
	{
		SpriteCon.Die();

		yield return new WaitForSeconds(0.5f);

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
		Rb.mass = 100f;
		Rb.AddForce(new Vector2(0, value * 1000f));

		yield return new WaitForSeconds(time);
			
		cPush = null;

		if(cStun == null)
			Move();
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
		Rb.mass = 100f;

		yield return new WaitForSeconds(time);

		cStun = null;

		if (cPush == null)
			Move();
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