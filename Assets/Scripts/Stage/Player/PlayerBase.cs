using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public static PlayerBase Player;

	private void OnEnable()
	{
		Player = this;		
	}

	private void Start()
	{
		StartCoroutine(TestProjectile());
	}

	IEnumerator TestProjectile()
	{
		while (true)
		{
			if (EnemyBase.Enemies.Count > 0)
			{
				ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/TestProjectile") as GameObject);
				projectile.transform.position = transform.position;
				projectile.Setup(EnemyBase.Enemies[0].transform);
			}

			yield return new WaitForSeconds(1f);
		}
	}





}
