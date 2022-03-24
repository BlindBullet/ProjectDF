using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileController : MonoBehaviour
{	
	ProjectileChart data;
	List<HitresultChart> hitresults = new List<HitresultChart>();
	HeroBase caster;

	public void Setup(ProjectileChart data, List<HitresultChart> hitresults, HeroBase caster)
    {
		this.data = data;
		this.hitresults = hitresults;
		this.caster = caster;

		StartCoroutine(MoveSequence());
    }

	IEnumerator MoveSequence()
	{
		Vector3 dir = new Vector3(0, 1, 0);

		while (true)
		{			
			transform.position += dir * 2f * Time.deltaTime;
			yield return null;
		}		
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			//collision.GetComponent<EnemyBase>().TakeDmg(Atk);
			
			
			ObjectManager.Ins.Push<ProjectileController>(this);
		}
	}

}
