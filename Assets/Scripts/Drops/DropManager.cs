using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoSingleton<DropManager>
{
	
	public void DropGold(Vector2 pos, int count)
	{
		for (int i = 0; i < count; i++)
		{
			DropGold obj = ObjectManager.Ins.Pop<DropGold>(Resources.Load("Prefabs/Drops/Gold") as GameObject);
			float randX = Random.Range(pos.x - 1f, pos.x + 1f);
			float randY = Random.Range(pos.y - 1f, pos.y + 1f);
			obj.transform.position = new Vector2(randX, randY);
			obj.Setup();
		}
	}

	public void DropSoulStone(Vector2 pos, int count)
	{
		for(int i = 0; i < count; i++)
		{
			DropSoulStone obj = ObjectManager.Ins.Pop<DropSoulStone>(Resources.Load("Prefabs/Drops/SoulStone") as GameObject);
			float randX = Random.Range(pos.x - 1f, pos.x + 1f);
			float randY = Random.Range(pos.y - 1f, pos.y + 1f);
			obj.transform.position = new Vector2(randX, randY);
			obj.Setup();
		}
	}

}
