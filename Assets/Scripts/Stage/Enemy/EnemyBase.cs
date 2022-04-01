using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
	public static List<EnemyBase> Enemies = new List<EnemyBase>();

	public EnemyStat Stat;
	public GameObject Model;
	public Rigidbody2D Rb;
	public Animator Anim;
	BoxCollider2D col;
	Coroutine cMove = null;	
	bool isPushing = false;
	Coroutine cPush = null;
	bool isBoss = false;

	public void Setup(EnemyChart data, int stageNo, bool isBoss = false)
	{
		Stat = new EnemyStat();
		Stat.SetStat(data, stageNo, isBoss);

		this.isBoss = isBoss;

		col = GetComponent<BoxCollider2D>();
		col.enabled = true;

		Rb.drag = 1f;
		Rb.angularDrag = 1f;
		cMove = StartCoroutine(MoveSequence());
		Enemies.Add(this);
	}

	IEnumerator MoveSequence()
	{
		float scaleX = Model.transform.localScale.x;
		Anim.SetTrigger("Idle");

		int randNo = Random.Range(0, 2);

		if(randNo == 0)
        {
			Model.transform.localScale = Model.transform.localScale.WithX(scaleX);
		}
        else
        {
			Model.transform.localScale = Model.transform.localScale.WithX(-scaleX);
		}

		while (true)
		{	
			Vector3 dir = Vector3.down;

			if (dir.y > 0.1f)
			{
				if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk_B"))
					Anim.SetTrigger("Walk_B");
			}
			else
			{
				if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk_F"))
					Anim.SetTrigger("Walk_F");
			}

			Rb.velocity = new Vector2(0, isPushing ? 0 : -Stat.Spd);			

			yield return null;
		}
		
	}

    public double TakeDmg(double atk)
    {
		atk = Math.Round(atk);
		Stat.CurHp -= atk;

		if (Stat.CurHp <= 0)
			Die();

		return atk;
    }

	public void Die()
    {
		StageManager.Ins.GetGold(Stat.Gold);
		StopCoroutine(cMove);
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