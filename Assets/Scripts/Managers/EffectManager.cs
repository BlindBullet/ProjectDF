using System;
using System.Collections;
using UnityEngine;
//using Effekseer;

public class EffectManager : SingletonObject<EffectManager>
{
	private const string EffectDirectory = "Prefabs/Fxs/";

	public GameObject SpawnEffect(string key)
	{
		var obj = ObjectManager.Ins.Pop<Transform>(GetEffectPrefab(key)).gameObject;
        var longest = float.MinValue;

        foreach (var elem in obj.GetComponentsInChildren<ParticleSystem>())
        {
            if (elem.main.duration > longest && !elem.main.loop)
            {
                longest = elem.main.duration;
            }
        }
        if (longest > 0)
        {
            StartCoroutine(EffectConditionSequence(obj.transform, longest + 0.01f));
        }
        return obj;
	}

	public GameObject SpawnEffect(string key, float duration)
	{
		var obj = ObjectManager.Ins.Pop<Transform>(GetEffectPrefab(key));
		if (duration > 0f)
		{
			StartCoroutine(EffectConditionSequence(obj, duration));
		}
		return obj.gameObject;
	}

	public T SpawnEffect<T>(string key, float duration) where T : Component
	{
		var obj = ObjectManager.Ins.Pop<T>(GetEffectPrefab(key));
		if (duration > 0f)
		{
			StartCoroutine(EffectConditionSequence(obj.transform, duration));
		}
		return obj;
	}

	private GameObject GetEffectPrefab(string path)
	{
		var obj = Resources.Load<GameObject>(EffectDirectory + path);
		if (obj == null)
		{
			throw new NullReferenceException("No Object Found");
		}
		return obj;
	}

	private IEnumerator EffectConditionSequence(Transform trf, float duration)
	{
		yield return new WaitForSeconds(duration);
		trf.PushToPool();
	}
		
	//public void ShowFx(string id, CharacterAnchor targetAnchor)
	//{
	//	if (!LoadData.Ins.FxData.ContainsKey(id))
	//	{
	//		Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
	//		return;
	//	}

	//	FxData fxData = LoadData.Ins.FxData[id];
	//	GameObject fx = SpawnEffect(fxData.FxResource);
				
	//	if(fxData.SpawnAnchor == "" || fxData.SpawnAnchor == "None")
	//	{
	//		fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);
	//	}
	//	else
	//	{
	//		Transform fxTrf = targetAnchor.GetAnchor(fxData.SpawnAnchor);

	//		if (fxData.Binding)
	//		{
	//			fx.transform.position = fxTrf.position;
	//			fx.transform.SetParent(fxTrf.transform);
	//			fx.transform.position = new Vector3(fx.transform.position.x + fxData.SpawnPosX, fx.transform.position.y + fxData.SpawnPosY, fx.transform.position.z);
	//		}
	//		else
	//		{
	//			fx.transform.position = fxTrf.position;
	//			fx.transform.position = new Vector3(fx.transform.position.x + fxData.SpawnPosX, fx.transform.position.y + fxData.SpawnPosY, fx.transform.position.z);
	//		}
	//	}

	//	if (fxData.SoundResource != "")
	//		SoundManager.Ins.PlaySFX(fxData.SoundResource);
	//}

	public void ShowFx(string id, Transform fxTrf)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		GameObject fx = SpawnEffect(fxData.FxResource);

		if (fxData.SpawnAnchor == "" || fxData.SpawnAnchor == "None")
		{
			fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);
		}
		else
		{
			if (fxData.Binding)
			{
				fx.transform.position = fxTrf.position;
				fx.transform.SetParent(fxTrf.transform);
				fx.transform.position = new Vector3(fx.transform.position.x + fxData.SpawnPosX, fx.transform.position.y + fxData.SpawnPosY, fx.transform.position.z);
			}
			else
			{
				fx.transform.position = fxTrf.position;
				fx.transform.position = new Vector3(fx.transform.position.x + fxData.SpawnPosX, fx.transform.position.y + fxData.SpawnPosY, fx.transform.position.z);
			}
		}

		if (fxData.SoundResource != "")
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
	}

	public void ShowFx(string id, Vector3 pos)
    {
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		GameObject fx = SpawnEffect(fxData.FxResource);

		if (fxData.SpawnAnchor == "" || fxData.SpawnAnchor == "None")
		{
			fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);
		}
		else
		{
			fx.transform.position = new Vector3(pos.x + fxData.SpawnPosX, pos.y + fxData.SpawnPosY, pos.z);			
		}

		if (fxData.SoundResource != "")
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
	}

	public void ShowFx(string id)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		GameObject fx = SpawnEffect(fxData.FxResource);

		fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);

		if (fxData.SoundResource != "")
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
	}
	
	public GameObject ShowDurationFx(string id)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return null;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		GameObject fx = SpawnEffect(fxData.FxResource);

		fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);

		if (fxData.SoundResource != "")
			SoundManager.Ins.PlaySFX(fxData.SoundResource);

		return fx;
	}

}