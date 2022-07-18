using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonObject<EnemySpawner>
{
	public void Spawn(int stageNo)
	{		
		int _stageNo = StageManager.Ins.GetStageNo(stageNo);
		
		StageChart data = CsvData.Ins.StageChart[_stageNo.ToString()];		
		
		for(int i = 0; i < data.Count.Length; i++)
		{
			StartCoroutine(SpawnSequence(data, stageNo, i, i * 2f, i == data.Count.Length - 1 ? true : false));
		}

		if (data.Boss != null)
			StartCoroutine(SpawnSequence(data, stageNo, -1, 3f, false));
	}

	public void StopSpawn()
	{
		StopAllCoroutines();
	}

	IEnumerator SpawnSequence(StageChart chart, int stageNo, int no, float spawnTime, bool isLast = false)
	{
		yield return new WaitForSeconds(spawnTime);

		if(no == -1)
		{
			SpawnEnemy(chart.Boss, stageNo, true);
		}
		else
		{
			for (int k = 0; k < chart.Count[no]; k++)
			{
				int randNo = LotteryCalculator.LotteryIntWeight(chart.Probs);
				SpawnEnemy(chart.Enemies[randNo], stageNo);
			}
		}
		
		if (isLast)
		{
			StageManager.Ins.LastEnemiesSpawned = true;
		}
	}

	public void SpawnEnemy(string id, int stageNo, bool isBoss = false)
	{		
		EnemyChart chart = CsvData.Ins.EnemyChart[id];
		EnemyBase enemy = ObjectPooler.SpawnFromPool<EnemyBase>("EnemyObj" + chart.Shape.ToString(), CalcSpawnPos());
		enemy.transform.localScale = new Vector2(chart.Size, chart.Size);
		//enemy.transform.position = CalcSpawnPos();
		enemy.Setup(chart, stageNo, isBoss);		
	}

	public void SpawnSummonEnemy(string id, Vector2 pos)
	{
		EnemyChart chart = CsvData.Ins.EnemyChart[id];
		EnemyBase enemy = ObjectPooler.SpawnFromPool<EnemyBase>("EnemyObj" + chart.Shape.ToString(), pos);
		enemy.transform.localScale = new Vector2(chart.Size, chart.Size);
		enemy.transform.position = new Vector2(pos.x + Random.Range(-1f,1f), pos.y + Random.Range(0.2f, 2f));
		enemy.Setup(chart, StageManager.Ins.PlayerData.Stage);
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
				randY = Random.Range(max.y + 1f, max.y + 2f);				
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
