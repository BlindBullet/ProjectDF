using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoSingleton<DropManager>
{
	public Transform GoldTrf;
	public Transform SoulStoneTrf;
	public void DropGold(Vector2 pos, int count)
	{
		for (int i = 0; i < count; i++)
		{
			float randX = Random.Range(pos.x - 1f, pos.x + 1f);
			float randY = Random.Range(pos.y - 1f, pos.y + 1f);
			DropGold obj = ObjectPooler.SpawnFromPool<DropGold>("Gold", new Vector2(randX, randY));
			obj.Setup();			
		}
	}

	public void DropSoulStone(Vector2 pos, int count)
	{
		for(int i = 0; i < count; i++)
		{
			float randX = Random.Range(pos.x - 1f, pos.x + 1f);
			float randY = Random.Range(pos.y - 1f, pos.y + 1f);
			DropSoulStone obj = ObjectPooler.SpawnFromPool<DropSoulStone>("SoulStone", new Vector2(randX, randY));
			obj.Setup();
		}
	}

}
