using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
	public EnemyStat Stat;
	public GameObject Model;
	public Rigidbody2D Rb;

	private void Start()
	{
		Stat = new EnemyStat();
		Stat.MaxHp = 10;
		Stat.CurHp = 10;
		Stat.Atk = 1;
		Stat.Spd = 1f;

		StartCoroutine(MoveSequence());
	}

	IEnumerator MoveSequence()
	{
		float scaleX = Model.transform.localScale.x;

		while (true)
		{
			if(GameManager.Ins.State == GameState.StageStart)
			{			
				Vector3 dir = (PlayerBase.Player.transform.position - transform.position).normalized;
				
				if(dir.x >= 0)
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


}

public class EnemyStat
{
	public int MaxHp;
	public int CurHp;
	public int Atk;
	public float Spd;

}