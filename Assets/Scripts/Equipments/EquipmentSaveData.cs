using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentSaveData
{
	public List<EquipmentData> Datas = new List<EquipmentData>();
	public int WeaponLv;
	public int WeaponExp;
	public int AccLv;
	public int AccExp;
	public double EnchantStone;

	public void Init()
	{
		Datas.Clear();

		foreach(KeyValuePair<string, EquipmentChart> elem in CsvData.Ins.EquipmentChart)
		{
			EquipmentData data = new EquipmentData();
			data.Init(elem.Key, elem.Value.Type);
			Datas.Add(data);
		}

		WeaponLv = 1;
		WeaponExp = 0;
		AccLv = 1;
		AccExp = 0;
		EnchantStone = 0f;

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

	public void Equip(EquipmentData data)
	{
		switch (data.Type)
		{
			case EquipmentType.Weapon:
				for (int i = 0; i < Datas.Count; i++)
				{
					if (Datas[i] == data)
					{
						Datas[i].Equip();
					}
					else if(Datas[i] != data && Datas[i].Type == EquipmentType.Weapon)
					{
						Datas[i].UnEquip();
					}
				}
				break;
			case EquipmentType.Acc:
				for (int i = 0; i < Datas.Count; i++)
				{
					if (Datas[i] == data)
					{
						Datas[i].Equip();
					}
					else if (Datas[i] != data && Datas[i].Type == EquipmentType.Acc)
					{
						Datas[i].UnEquip();
					}
				}
				break;
		}

		Save();
	}

	public void EnchantLvUp(EquipmentData data)
	{		
		data.EnchantLvUp();
		Save();
	}

	public void Fusion(EquipmentData data)
	{		
		EquipmentChart chart = CsvData.Ins.EquipmentChart[data.Id];

		if(data.Count >= 5)
		{
			int count = data.Count / 5;

			data.AddCount(-5 * count);

			for(int i = 0; i < Datas.Count; i++)
			{
				EquipmentChart _chart = CsvData.Ins.EquipmentChart[Datas[i].Id];

				if(chart.Level == 1)
				{						
					if (_chart.Grade == chart.Grade + 1 && _chart.Level == 5)
					{
						if (Datas[i].isOpen)
						{
							Datas[i].AddCount(count);
						}
						else
						{
							Datas[i].Open();
							Datas[i].AddCount(count);
						}
					}	
				}
				else
				{
					if(_chart.Grade == chart.Grade && _chart.Level == chart.Level - 1)
					{
						if (Datas[i].isOpen)
						{
							Datas[i].AddCount(count);
						}
						else
						{
							Datas[i].Open();
							Datas[i].AddCount(count);
						}
					}
				}
			}
		}

		Save();
	}

	public void WeaponExpUp(int count)
	{
		WeaponExp += count;

		var chart = CsvData.Ins.EquipmentLvChart[WeaponLv.ToString()];

		if(WeaponExp >= chart.NeedCount)
		{
			WeaponLv++;
			WeaponExp = WeaponExp - chart.NeedCount;
		}

		Save();
	}

	public void AccExpUp(int count)
	{
		AccExp += count;

		var chart = CsvData.Ins.EquipmentLvChart[AccLv.ToString()];

		if (AccExp >= chart.NeedCount)
		{
			AccLv++;
			AccExp = AccExp - chart.NeedCount;
		}

		Save();
	}

	public List<string> GachaWeapon(int count)
	{
		List<string> result = new List<string>();
		var chart = CsvData.Ins.EquipmentLvChart[WeaponLv.ToString()];

		for(int i = 0; i < count; i++)
		{
			int lotteryNo = LotteryCalculator.LotteryIntWeight(chart.Probs) + 1;
			int lvLotteryNo = LotteryCalculator.LotteryIntWeight(ConstantData.EquipmentGachaLvProbs) + 1;

			if (lotteryNo == 5 && lvLotteryNo == 1)
			{
				lvLotteryNo = 2;
			}

			foreach (KeyValuePair<string, EquipmentChart> elem in CsvData.Ins.EquipmentChart)
			{
				if (elem.Value.Grade == lotteryNo && elem.Value.Level == lvLotteryNo && elem.Value.Type == EquipmentType.Weapon)
				{
					result.Add(elem.Key);
					Get(elem.Key, 1);
					break;
				}	
			}
		}

		WeaponExpUp(count);
		return result;
	}

	public List<string> GachaAcc(int count)
	{
		List<string> result = new List<string>();
		var chart = CsvData.Ins.EquipmentLvChart[AccLv.ToString()];

		for (int i = 0; i < count; i++)
		{
			int lotteryNo = LotteryCalculator.LotteryIntWeight(chart.Probs) + 1;
			int lvLotteryNo = LotteryCalculator.LotteryIntWeight(ConstantData.EquipmentGachaLvProbs) + 1;

			if (lotteryNo == 5 && lvLotteryNo == 1)
			{
				lvLotteryNo = 2;
			}

			foreach (KeyValuePair<string, EquipmentChart> elem in CsvData.Ins.EquipmentChart)
			{
				if (elem.Value.Grade == lotteryNo && elem.Value.Level == lvLotteryNo && elem.Value.Type == EquipmentType.Acc)
				{
					result.Add(elem.Key);
					Get(elem.Key, 1);
					break;
				}
			}
		}

		AccExpUp(count);
		return result;
	}

	public void GetEnchantStone(double amount)
	{
		EnchantStone += amount;
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
			WeaponLv = data.WeaponLv;
			WeaponExp = data.WeaponExp;
			AccLv = data.AccLv;
			AccExp = data.AccExp;
			EnchantStone = data.EnchantStone;			
		}

		CheckAddData();
		Save();
	}
}
