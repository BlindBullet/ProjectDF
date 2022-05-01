using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{		
	HeroBase heroBase;
	public bool isTargeting = false;
	Coroutine cSearch = null;
		
	public List<EnemyBase> Targets = new List<EnemyBase>();

	Vector2 min;
	Vector2 max;

	public void SearchTarget()
	{
		Targets.Clear();

		for(int i = 0; i < EnemyBase.Enemies.Count; i++)
		{
			if(EnemyBase.Enemies[i].transform.position.x > min.x && EnemyBase.Enemies[i].transform.position.x < max.x
				&& EnemyBase.Enemies[i].transform.position.y > min.y && EnemyBase.Enemies[i].transform.position.y < max.y)
			{
				if(EnemyBase.Enemies[i].Stat.CurHp > 0f)
					Targets.Add(EnemyBase.Enemies[i]);
			}
		}

		Targets.Sort(delegate (EnemyBase A, EnemyBase B)
		{
			float distA = Vector3.Distance(transform.position, A.transform.position);
			float distB = Vector3.Distance(transform.position, B.transform.position);

			if (distA >= distB)
				return 1;
			else
				return -1;
		});
	}
		
	public void StartSearch(HeroBase data)
	{
		heroBase = data;

		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		if (cSearch != null)
		{
			StopCoroutine(cSearch);
		}

		cSearch = StartCoroutine(Search());
	}

	public IEnumerator Search()
	{	
		while (true)
		{
			SearchTarget();
			
			if(Targets.Count > 0)
			{
				isTargeting = true;
			}
			else
			{
				isTargeting = false;
			}

			yield return null;
		}		
	}

	public List<EnemyBase> SearchTarget(ResultGroupChart data, EnemyBase enemy)
	{
		List<EnemyBase> result = new List<EnemyBase>();
		Collider2D[] _colls = null;

		switch (data.RangeType)
		{
			case RangeType.Circle:
				_colls = Physics2D.OverlapCircleAll(enemy.transform.position, data.RnageSize[0]);
								
				break;
		}

		if(_colls != null)
		{
			foreach (Collider2D member in _colls)
			{
				if (member.CompareTag("Enemy"))
				{
					result.Add(member.GetComponent<EnemyBase>());
				}
			}
		}

		return result;
	}

}
