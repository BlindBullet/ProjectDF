using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonObject<EnemySpawner>
{


	public void Spawn(int stageNo)
	{
		List<StageChart> datas = new List<StageChart>();
		int _stageNo = StageManager.Ins.GetStageNo(stageNo);
		
		datas = CsvData.Ins.StageChart[_stageNo];
		
		for(int i = 0; i < datas.Count; i++)
		{
			StartCoroutine(SpawnSequence(datas[i], stageNo, i == datas.Count - 1 ? true : false));
		}
	}

	IEnumerator SpawnSequence(StageChart chart, int stageNo, bool isLast)
	{
		yield return new WaitForSeconds(chart.Time);

		if(chart.Enemies != null)
		{
			for (int i = 0; i < chart.Enemies.Length; i++)
			{
				for (int k = 0; k < chart.Count[i]; k++)
				{
					SpawnEnemy(chart.Enemies[i], stageNo);
				}
			}
		}

		if (chart.Boss != null)
			SpawnEnemy(chart.Boss, stageNo, true);

		if (isLast)
		{
			StageManager.Ins.LastEnemiesSpawned = true;
		}
	}


	public void SpawnEnemy(string id, int stageNo, bool isBoss = false)
	{
		EnemyChart chart = CsvData.Ins.EnemyChart[id];
		EnemyBase enemy = ObjectManager.Ins.Pop<EnemyBase>(Resources.Load("Prefabs/Characters/Enemies/EnemyObj") as GameObject);
		enemy.transform.localScale = new Vector2(chart.Size, chart.Size);
		enemy.transform.position = CalcSpawnPos();
		enemy.Setup(chart, stageNo, isBoss);
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
				randX = Random.Range(min.x, max.x);
				randY = Random.Range(max.y + 3f, max.y + 6f);				
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
