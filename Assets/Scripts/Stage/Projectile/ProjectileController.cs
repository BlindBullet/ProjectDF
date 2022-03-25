using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProjectileController : MonoBehaviour
{	
	ProjectileChart data;
	List<HitresultChart> hitresults = new List<HitresultChart>();
	HeroBase caster;
	int penCount;

	public void Setup(ProjectileChart data, List<HitresultChart> hitresults, HeroBase caster)
    {
		this.data = data;
		this.hitresults = hitresults;
		this.caster = caster;
		penCount = data.PenCount;

		StartCoroutine(MoveSequence());
    }

	IEnumerator MoveSequence()
	{
		float time = 0;

		while (true)
		{
			switch (data.MoveType)
			{
				case PMoveType.Direct:
					transform.Translate(Vector3.up * data.Speed * Time.deltaTime);
					break;

			}

			time += Time.deltaTime;

			if(time >= data.Lifetime)
            {
				if (data.DestroyFx != null)
					EffectManager.Ins.ShowFx(data.DestroyFx, this.transform.position);

				if(data.DestroyResultGroupId != null)
                {
					List<ResultGroupChart> destroyResultGroups = CsvData.Ins.ResultGroupChart[data.DestroyResultGroupId];
				}

				this.transform.position = new Vector3(0, -100f, 0);
				ObjectManager.Ins.Push<ProjectileController>(this);
			}
				

			yield return null;
		}		
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{	
			EnemyBase enemyBase = collision.GetComponent<EnemyBase>();
			HitresultManager.Ins.SendHitresult(hitresults, enemyBase, caster);
			
			if(data.HitResultGroupId != null)
            {
				List<ResultGroupChart> hitResultGroups = CsvData.Ins.ResultGroupChart[data.HitResultGroupId];

			}

			if(penCount == 0)
            {
				if (data.DestroyFx != null)
					EffectManager.Ins.ShowFx(data.DestroyFx, this.transform.position);

				if (data.DestroyResultGroupId != null)
                {
					List<ResultGroupChart> destroyResultGroups = CsvData.Ins.ResultGroupChart[data.DestroyResultGroupId];

				}

				penCount--;

				this.transform.position = new Vector3(0, -100f, 0);
				ObjectManager.Ins.Push<ProjectileController>(this);
			}	
		}
	}


}
