using System;
using System.Collections;
using UnityEngine;

public class EffectManager : SingletonObject<EffectManager>
{	
	float GetParticleDurationTime(GameObject obj)
	{
		float longest = float.MinValue;

		foreach (var elem in obj.GetComponentsInChildren<ParticleSystem>())
		{
			if (elem.main.duration > longest && !elem.main.loop)
			{
				longest = elem.main.duration;
			}
		}

		return longest;
	}

	public void ShowFx(string id, CharacterAnchor targetAnchor)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		FxController fx = ObjectPooler.SpawnFromPool<FxController>(fxData.FxResource, targetAnchor.transform.position);		
		fx.Setup(GetParticleDurationTime(fx.gameObject));
		fx.transform.rotation = Quaternion.Euler(fxData.SpawnAngle, 0, 0);
		fx.transform.localScale = fx.transform.localScale * fxData.Size;

		if (fxData.SpawnAnchor == "" || fxData.SpawnAnchor == "None" || fxData.SpawnAnchor == null)
		{
			fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);
		}
		else
		{
			Transform fxTrf = targetAnchor.GetAnchor(fxData.SpawnAnchor);			
			fx.transform.position = fxTrf.position;
			fx.transform.position = new Vector3(fx.transform.position.x + fxData.SpawnPosX, fx.transform.position.y + fxData.SpawnPosY, fx.transform.position.z);			
		}

		if (fxData.SoundResource != "" && fxData.SoundResource != "HitFx")
		{
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
		}
		else if(fxData.SoundResource == "HitFx")
		{
			SoundManager.Ins.PlayNASFX("HitFx");
		}	
	}

	public void ShowFx(string id, CharacterAnchor targetAnchor, float durationTime)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		FxController fx = ObjectPooler.SpawnFromPool<FxController>(fxData.FxResource, Vector3.zero);		
		fx.Setup(durationTime);
		fx.transform.rotation = Quaternion.Euler(fxData.SpawnAngle, 0, 0);
		fx.transform.localScale = fx.transform.localScale * fxData.Size;
		if (fxData.SpawnAnchor == "" || fxData.SpawnAnchor == "None" || fxData.SpawnAnchor == null)
		{
			fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);
		}
		else
		{
			Transform fxTrf = targetAnchor.GetAnchor(fxData.SpawnAnchor);

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

		if (fxData.SoundResource != "" && fxData.SoundResource != "HitFx")
		{
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
		}
		else if (fxData.SoundResource == "HitFx")
		{
			SoundManager.Ins.PlayNASFX("HitFx");
		}
	}

	public void ShowFx(string id, Transform fxTrf)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		FxController fx = ObjectPooler.SpawnFromPool<FxController>(fxData.FxResource, Vector3.zero);		
		fx.Setup(GetParticleDurationTime(fx.gameObject));
		fx.transform.rotation = Quaternion.Euler(fxData.SpawnAngle, 0, 0);
		fx.transform.localScale = fx.transform.localScale * fxData.Size;
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
				fx.transform.localScale = new Vector3(fx.transform.localScale.x, fx.transform.localScale.y, 1);
			}
			else
			{
				fx.transform.position = fxTrf.position;
				fx.transform.position = new Vector3(fx.transform.position.x + fxData.SpawnPosX, fx.transform.position.y + fxData.SpawnPosY, fx.transform.position.z);
				fx.transform.localScale = new Vector3(fx.transform.localScale.x, fx.transform.localScale.y, 1);
			}
		}

		if (fxData.SoundResource != "" && fxData.SoundResource != "HitFx")
		{
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
		}
		else if (fxData.SoundResource == "HitFx")
		{
			SoundManager.Ins.PlayNASFX("HitFx");
		}
	}

	public void ShowFx(string id, Vector3 pos)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		FxController fx = ObjectPooler.SpawnFromPool<FxController>(fxData.FxResource, pos);		
		fx.Setup(GetParticleDurationTime(fx.gameObject));
		fx.transform.rotation = Quaternion.Euler(fxData.SpawnAngle, 0, 0);
		fx.transform.localScale = fx.transform.localScale * fxData.Size;

		if (fxData.SpawnAnchor == "" || fxData.SpawnAnchor == "None")
		{
			fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);
		}
		else
		{
			fx.transform.position = new Vector3(pos.x + fxData.SpawnPosX, pos.y + fxData.SpawnPosY, pos.z);			
		}

		if (fxData.SoundResource != "" && fxData.SoundResource != "HitFx")
		{
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
		}
		else if (fxData.SoundResource == "HitFx")
		{
			SoundManager.Ins.PlayNASFX("HitFx");
		}
	}

	public void ShowFx(string id)
	{
		if (!CsvData.Ins.FxChart.ContainsKey(id))
		{
			Debug.Log("FxData 테이블에는 해당 " + id + "가 없습니다.");
			return;
		}

		FxChart fxData = CsvData.Ins.FxChart[id];
		FxController fx = ObjectPooler.SpawnFromPool<FxController>(fxData.FxResource, Vector3.zero);		
		fx.Setup(GetParticleDurationTime(fx.gameObject));
		fx.transform.localScale = fx.transform.localScale * fxData.Size;
		fx.transform.position = new Vector3(fxData.SpawnPosX, fxData.SpawnPosY, 0);

		if (fxData.SoundResource != "" && fxData.SoundResource != "HitFx")
		{
			SoundManager.Ins.PlaySFX(fxData.SoundResource);
		}
		else if (fxData.SoundResource == "HitFx")
		{
			SoundManager.Ins.PlayNASFX("HitFx");
		}
	}

}