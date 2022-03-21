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

	private void OnEnable()
	{
		Enemies.Add(this);
	}

	public void Setup()
	{
		Stat = new EnemyStat();
		Stat.CurHp = 30f;
		Stat.Spd = 2f;

		col = GetComponent<BoxCollider2D>();
		col.enabled = true;

		cMove = StartCoroutine(MoveSequence());
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
				if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
					Anim.SetTrigger("Walk");
			}

			transform.position += dir * Stat.Spd * 0.5f * Time.deltaTime;
			
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
		StopCoroutine(cMove);
		col.enabled = false;
		Enemies.Remove(this);
		
		StartCoroutine(DieSequence());
	}

	IEnumerator DieSequence()
    {
		yield return new WaitForSeconds(1f);

		transform.position = new Vector3(0, 20f, 0);
		ObjectManager.Ins.Push<EnemyBase>(this);
    }

}