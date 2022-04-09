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
	public Animator Anim;
	CircleCollider2D col;
	Coroutine cMove = null;	
	bool isPushing = false;
	Coroutine cPush = null;
	bool isBoss = false;

	public SpriteRenderer ModelSprite;
	public TextMeshPro HpText;

	public void Setup(EnemyChart chart, int stageNo, bool isBoss = false)
	{
		Stat = new EnemyStat();
		Stat.SetStat(chart, stageNo, isBoss);

		SetModel(chart);
		SetHp();

		this.isBoss = isBoss;

		Rb = GetComponent<Rigidbody2D>();

		col = GetComponent<CircleCollider2D>();
		col.enabled = true;

		Rb.drag = 1f;
		Rb.angularDrag = 1f;
		cMove = StartCoroutine(MoveSequence());
		Enemies.Add(this);
	}

	void SetModel(EnemyChart chart)
	{
		ModelSprite.sprite = Resources.Load<SpriteAtlas>("Sprites/Enemies/Enemies").GetSprite(chart.Model);
	}

	IEnumerator MoveSequence()
	{
		float scaleX = Model.transform.localScale.x;

		if(Anim != null)
			Anim.SetTrigger("Idle");

		//int randNo = Random.Range(0, 2);

		//if(randNo == 0)
		//{
		//	Model.transform.localScale = Model.transform.localScale.WithX(scaleX);
		//}
		//else
		//{
		//	Model.transform.localScale = Model.transform.localScale.WithX(-scaleX);
		//}

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

			if (dir.y > 0.1f)
			{
				if (Anim != null)
				{
					if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk_B"))
						Anim.SetTrigger("Walk_B");
				}	
			}
			else
			{
				if (Anim != null)
				{
					if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk_F"))
						Anim.SetTrigger("Walk_F");
				}	
			}

			Rb.velocity = new Vector2(xSpd, isPushing ? 0 : -Stat.Spd);			

			yield return null;
		}
		
	}

	public double TakeDmg(double atk)
	{
		atk = Math.Round(atk);
		Stat.CurHp -= atk;
		SetHp();

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
		StopCoroutine(cMove);

		if (Anim != null)
			Anim.SetTrigger("Die");

		Rb.drag = 100f;
		Rb.angularDrag = 100f;
		Rb.velocity = Vector2.zero;
		col.enabled = false;
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
		yield return new WaitForSeconds(1f);

		transform.position = new Vector3(0, 20f, 0);
		ObjectManager.Ins.Push<EnemyBase>(this);
	}

	public void Push(float value, float time)
	{
		if(cPush == null)
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
		isPushing = true;
		Rb.AddForce(new Vector2(0, value), ForceMode2D.Impulse);

		yield return new WaitForSeconds(time);

		isPushing = false;
		cPush = null;
	}


}