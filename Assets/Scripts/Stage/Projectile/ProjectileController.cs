using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileController : MonoBehaviour
{
	Transform targetTrf;
	public double Atk;
	public float Spd;
	
	public void Setup(Transform targetTrf)
	{
		this.targetTrf = targetTrf;
		StartCoroutine(MoveSequence());
	}

	IEnumerator MoveSequence()
	{
		Vector3 dir = (targetTrf.position - transform.position).normalized;

		while (true)
		{	
			transform.position += dir * Spd * Time.deltaTime;
			yield return null;
		}		
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			collision.GetComponent<EnemyBase>().TakeDmg(Atk);

			if (collision.GetComponent<EnemyBase>().Stat.CurHp <= 0)
				ObjectManager.Ins.Push<EnemyBase>(collision.GetComponent<EnemyBase>());

			ObjectManager.Ins.Push<ProjectileController>(this);
		}
	}

	//public void OnBecameInvisible()
	//{
	//	ObjectManager.Ins.Push<ProjectileController>(this);
	//}
}
