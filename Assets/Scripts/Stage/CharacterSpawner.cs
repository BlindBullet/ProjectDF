using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : SingletonObject<CharacterSpawner>
{
    
    public void SpawnEnemy(string id)
	{
		EnemyBase enemy = ObjectManager.Ins.Pop<EnemyBase>(Resources.Load("Prefabs/Characters/Enemies/" + id) as GameObject);
		enemy.transform.position = CalcSpawnPos();
		enemy.Setup();
	}

	Vector2 CalcSpawnPos()
	{
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		int randNo = Random.Range(0, 4);
		float randX = 0;
		float randY = 0;
		randNo = 0;

		switch (randNo)
		{
			case 0:				
				randX = Random.Range(min.x + 1f, max.x - 1f);
				randY = Random.Range(max.y, max.y + 3f);				
				break;
			case 1:
				randX = Random.Range(min.x - 10f, min.x);
				randY = Random.Range(min.y - 10f, max.y + 10f);
				break;
			case 2:
				randX = Random.Range(min.x - 10f, max.x + 10f);
				randY = Random.Range(min.y, min.y - 10f);
				break;
			case 3:
				randX = Random.Range(max.x, max.x + 10f);
				randY = Random.Range(min.y - 10f, max.y + 10f);
				break;
		}

		return new Vector2(randX, randY);
	}



}
