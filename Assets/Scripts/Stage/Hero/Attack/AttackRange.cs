using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{	
	Vector3 RadarPos;
	public float Size;
	bool isBinding = true;
	float offsetY = 0.5f;

	HeroBase heroBase;
	public bool isTargeting = false;
	Coroutine cSearch = null;

	[SerializeField]
	Collider2D[] allColl;
	
	public List<Collider2D> AllTargetColl = new List<Collider2D>();

	public void SetRange(float size)
	{		
		this.Size = size;
	}

	public void CalcRange()
	{	
		RadarPos = transform.position.WithY(transform.position.y + offsetY);
		Vector3 startPos = RadarPos;

		allColl = Physics2D.OverlapCircleAll(startPos, Size);		
	}

	public void SearchTarget()
	{
		SetRange(heroBase.Data.Range);
		CalcRange();
		AllTargetColl.Clear();

		foreach (Collider2D member in allColl)
		{	
			if (member.gameObject.CompareTag("Enemy"))
			{
				if (member.GetComponent<EnemyBase>().Stat.CurHp > 0)
				{					
					AllTargetColl.Add(member);
				}
			}						
		}

		AllTargetColl.Sort(delegate (Collider2D A, Collider2D B)
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
			
			if (AllTargetColl.Count > 0)
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

	public void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position.WithY(transform.position.y + offsetY), Size);
	}

}
