using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
	public static List<EnemyBase> Enemies = new List<EnemyBase>();

	public EnemyStat Stat;
	public GameObject Model;
	public Rigidbody2D Rb;
	public Animator Anim;

	private void OnEnable()
	{
		Enemies.Add(this);
	}

	IEnumerator MoveSequence()
	{
		float scaleX = Model.transform.localScale.x;
		Anim.SetTrigger("Idle_F");

		while (true)
		{
			if(GameManager.Ins.State == GameState.StageStart)
			{			
				Vector3 dir = (PlayerBase.Player.transform.position - transform.position).normalized;

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

				if (dir.x >= 0)
				{
					Model.transform.localScale = Model.transform.localScale.WithX(scaleX);
				}
				else
				{
					Model.transform.localScale = Model.transform.localScale.WithX(-scaleX);
				}

				transform.position += dir * Stat.Spd * 0.5f * Time.deltaTime;
			}

			yield return null;

		}
		
	}

	public void Setup()
	{
		Stat = new EnemyStat();
		Stat.MaxHp = 10;
		Stat.CurHp = 10;
		Stat.Atk = 1;
		Stat.Spd = 1f;

		StartCoroutine(MoveSequence());
	}

	private void OnDisable()
	{
		Enemies.Remove(this);
	}
}

[System.Serializable]
public class EnemyStat
{
	public int MaxHp;
	public int CurHp;
	public int Atk;
	public float Spd;

}