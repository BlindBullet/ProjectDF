using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentSaveData
{
	public List<EquipmentData> Datas = new List<EquipmentData>();

	public void Init()
	{
		Datas.Clear();

		foreach(KeyValuePair<string, EquipmentChart> elem in CsvData.Ins.EquipmentChart)
		{
			EquipmentData data = new EquipmentData();
			data.Init(elem.Key, elem.Value.Type);
			Datas.Add(data);
		}

		Save();
	}

	public void CheckAddData()
	{
		foreach (KeyValuePair<string, EquipmentChart> elem in CsvData.Ins.EquipmentChart)
		{
			bool alreadyHave = false;
			for(int i = 0; i < Datas.Count; i++)
			{
				if (Datas[i].Id == elem.Key)
				{
					alreadyHave = true;
					break;
				}	
			}

			if (alreadyHave)
				continue;

			EquipmentData data = new EquipmentData();
			data.Init(elem.Key, elem.Value.Type);
			Datas.Add(data);
		}

		Save();
	}

	public void Get(string id, int count)
	{
		for(int i = 0; i < Datas.Count; i++)
		{
			if(Datas[i].Id == id)
			{
				if (Datas[i].isOpen)
				{					
					Datas[i].AddCount(count);
				}
				else
				{
					Datas[i].Open();
					Datas[i].AddCount(count);
					SEManager.Ins.Apply();
				}
			}
		}

		Save();
	}

	public void Equip(string id)
	{
		for(int i = 0; i < Datas.Count; i++)
		{
			if(Datas[i].Id == id)
			{
				Datas[i].Equip();
			}
			else
			{
				Datas[i].UnEquip();
			}
		}

		Save();
	}

	public void LvUp(string id)
	{
		for(int i = 0; i < Datas.Count; i++)
		{
			if (Datas[i].Id == id)
				Datas[i].EnchantLvUp();
		}

		Save();
	}

	public void Upgrade(string id)
	{
		EquipmentData data = null;

		for(int i = 0; i < Datas.Count; i++)
		{
			if(Datas[i].Id == id)
			{
				data = Datas[i];
			}
		}

		if(data != null)
		{
			EquipmentChart chart = CsvData.Ins.EquipmentChart[data.Id];

			if(data.Count >= 5)
			{
				data.AddCount(-5);

				for(int i = 0; i < Datas.Count; i++)
				{
					EquipmentChart _chart = CsvData.Ins.EquipmentChart[Datas[i].Id];

					if(chart.Level == 5)
					{						
						if (_chart.Grade == chart.Grade + 1 && _chart.Level == 1)
						{
							if (Datas[i].isOpen)
							{
								Datas[i].AddCount(1);
							}
							else
							{
								Datas[i].Open();
								Datas[i].AddCount(1);
							}
						}	
					}
					else
					{
						if(_chart.Grade == chart.Grade && _chart.Level == chart.Level + 1)
						{
							if (Datas[i].isOpen)
							{
								Datas[i].AddCount(1);
							}
							else
							{
								Datas[i].Open();
								Datas[i].AddCount(1);
							}
						}
					}
				}
			}
		}

		Save();
	}


	public void Save()
	{
		ES3.Save<EquipmentSaveData>("EquipmentData", this);
	}

	public void Load()
	{
		EquipmentSaveData data = ES3.Load<EquipmentSaveData>("EquipmentData", defaultValue: null);

		if (data == null)
		{
			Init();			
		}
		else
		{
			Datas = data.Datas;
		}

		CheckAddData();
		Save();
	}
}
