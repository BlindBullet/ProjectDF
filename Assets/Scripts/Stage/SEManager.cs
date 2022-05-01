using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoSingleton<SEManager>
{
	//게임 시작시 모든 캐릭터 콜렉션 효과 및 유물 효과를 적용.
	//캐릭터 배치 변경시 모든 캐릭터 콜렉션 효과 및 유물 효과를 재적용.
	//캐릭터 등급업시 모든 캐릭터 콜렉션 효과 및 유물 효과를 재적용.
		
	//플레이어 스탯 초기화
	//배치 영웅 스탯 초기화
	//효과 적용

	public void Apply()
	{
		List<SEChart> seList = new List<SEChart>();

		//영웅 콜렉션 효과 불러오기
		List<HeroData> heroes = StageManager.Ins.PlayerData.Heroes;

		for (int i = 0; i < heroes.Count; i++)
		{
			if (!heroes[i].IsOwn)
				continue;

			List<HeroChart> heroCharts = CsvData.Ins.HeroChart[heroes[i].Id];

			for(int k = 0; k < heroCharts.Count; k++)
			{
				if(heroCharts[k].Grade == heroes[i].Grade)
				{
					List<SEChart> charts = CsvData.Ins.SEChart[heroCharts[k].CollectionEffect];
					SEChart se = null;

					for(int j = 0; j < charts.Count; j++)
					{
						if (charts[j].Lv == heroes[i].Grade)
							se = charts[j];
					}
					
					seList.Add(se);
				}
			}
		}

		//유물 효과 불러오기
		List<RelicData> relics = StageManager.Ins.PlayerData.Relics;

		for(int i = 0; i < relics.Count; i++)
		{
			if (!relics[i].isOwn)
				continue;

			RelicChart chart = CsvData.Ins.RelicChart[relics[i].Id];

			List<SEChart> charts = CsvData.Ins.SEChart[chart.Effect];
			SEChart se = null;

			for (int j = 0; j < charts.Count; j++)
			{
				if (charts[j].Lv == relics[i].Lv)
					se = charts[j];
			}

			seList.Add(se);
		}

		InitAllStat();
		ApplyAllSe(seList);
	}

	void InitAllStat()
	{
		//플레이어 스탯 초기화
		StageManager.Ins.PlayerStat.Init();

		List<SlotData> slots = StageManager.Ins.PlayerData.Slots;
		
		//배치 영웅 스탯 초기화		
		for (int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			HeroBase.Heroes[i].Stat.InitData(HeroBase.Heroes[i].Data, slots[i].Lv);
		}
	}

	void ApplyAllSe(List<SEChart> charts)
	{
		for(int i = 0; i < charts.Count; i++)
		{
			ApplySE(charts[i]);
		}

		for(int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			HeroBase.Heroes[i].Stat.CalcStat();
		}
	}

	void ApplySE(SEChart chart)
	{
		if (CheckSECon(chart))
		{
			switch (chart.TargetType)
			{
				case SETargetType.Player:
					ApplyPlayerTargetSE(chart);
					break;
				case SETargetType.Hero:
					List<HeroBase> targets = new List<HeroBase>();

					switch (chart.TParam1)
					{
						case "All":
							targets = HeroBase.Heroes;
							break;
						case "Attr":
							for(int i = 0; i < HeroBase.Heroes.Count; i++)
							{
								if (HeroBase.Heroes[i].Stat.Attr.ToString() == chart.TParam2[0])
									targets.Add(HeroBase.Heroes[i]);
							}
							break;
						case "Slot":
							for(int i = 0; i < chart.TParam2.Length; i++)
							{
								for (int k = 0; k < HeroBase.Heroes.Count; k++)
								{
									if (HeroBase.Heroes[k].Data.SlotNo == int.Parse(chart.TParam2[i]))
										targets.Add(HeroBase.Heroes[k]);
								}
							}
							break;
						case "Grade":
							for (int i = 0; i < HeroBase.Heroes.Count; i++)
							{
								switch (chart.TParam2[0])
								{
									case "High":
										if (HeroBase.Heroes[i].Data.Grade >= int.Parse(chart.TParam3))
											targets.Add(HeroBase.Heroes[i]);
										break;
									case "Low":
										if (HeroBase.Heroes[i].Data.Grade <= int.Parse(chart.TParam3))
											targets.Add(HeroBase.Heroes[i]);
										break;
									default:
										if (HeroBase.Heroes[i].Data.Grade == int.Parse(chart.TParam3))
											targets.Add(HeroBase.Heroes[i]);
										break;
								}
							}
							break;
					}

					for(int i = 0; i < targets.Count; i++)
					{
						ApplyHeroTargetSE(targets[i], chart);
					}
					break;
				case SETargetType.Enemy:
					ApplyEnemyTargetSE(chart);
					break;
			}
		}		
	}

	bool CheckSECon(SEChart chart)
	{		
		bool result = false;
		int count = 0;

		switch (chart.ConType)
		{
			case SEConType.None:
				return true;
			case SEConType.DeployHero:
				switch (chart.CParam1)
				{
					case "Attr":
						switch (chart.CParam2[0])
						{	
							case "Red":
								for (int i = 0; i < HeroBase.Heroes.Count; i++)
								{
									if (HeroBase.Heroes[i].Stat.Attr == Attr.Red)
										count++;
								}
								break;
							case "Green":
								for (int i = 0; i < HeroBase.Heroes.Count; i++)
								{
									if (HeroBase.Heroes[i].Stat.Attr == Attr.Green)
										count++;
								}
								break;
							case "Blue":
								for (int i = 0; i < HeroBase.Heroes.Count; i++)
								{
									if (HeroBase.Heroes[i].Stat.Attr == Attr.Blue)
										count++;
								}
								break;
							default:
								for (int i = 0; i < HeroBase.Heroes.Count; i++)
								{
									if (HeroBase.Heroes[i].Stat.Attr == Attr.None)
										count++;
								}
								break;
						}

						switch (chart.CParam3)
						{
							case "High":
								if (count >= int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
							case "Low":
								if (count <= int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
							default:
								if (count == int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;									
						}						
					case "AttrAll":
						bool red = false;
						bool green = false;
						bool blue = false;

						for(int i = 0; i < HeroBase.Heroes.Count; i++)
						{
							if (HeroBase.Heroes[i].Stat.Attr == Attr.Red)
								red = true;
							if (HeroBase.Heroes[i].Stat.Attr == Attr.Green)
								green = true;
							if (HeroBase.Heroes[i].Stat.Attr == Attr.Blue)
								blue = true;
						}

						if (red && green && blue)
							return true;
						else
							return false;
					case "Grade":
						for (int i = 0; i < HeroBase.Heroes.Count; i++)
						{
							if (HeroBase.Heroes[i].Data.Grade >= int.Parse(chart.CParam2[0], System.Globalization.CultureInfo.InvariantCulture))
								count++;
						}

						switch (chart.CParam3)
						{
							case "High":
								if (count >= int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
							case "Low":
								if (count <= int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
							default:
								if (count == int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
						}						
					case "GradeAvr":
						float gradeSum = 0;

						for (int i = 0; i < HeroBase.Heroes.Count; i++)
						{
							gradeSum += HeroBase.Heroes[i].Data.Grade;
						}

						gradeSum = gradeSum / 5f;

						switch (chart.CParam3)
						{
							case "High":
								if (gradeSum >= int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
							case "Low":
								if (gradeSum <= int.Parse(chart.CParam4, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
						}
						break;
					case "Slot":
						for(int i = 0; i < chart.CParam2.Length; i++)
						{
							for(int k = 0; k < HeroBase.Heroes.Count; k++)
							{
								if(HeroBase.Heroes[k].Data.SlotNo == int.Parse(chart.CParam2[i]))
								{
									switch (chart.CParam3)
									{
										case "Attr":
											switch (chart.CParam4)
											{
												case "Red":
													if (HeroBase.Heroes[k].Stat.Attr == Attr.Red)
														count++;
													break;
												case "Green":
													if (HeroBase.Heroes[k].Stat.Attr == Attr.Green)
														count++;
													break;
												case "Blue":
													if (HeroBase.Heroes[k].Stat.Attr == Attr.Blue)
														count++;
													break;
												default:
													if (HeroBase.Heroes[k].Stat.Attr == Attr.None)
														count++;
													break;
											}
											break;
										case "Grade":
											switch (chart.CParam4)
											{
												case "High":
													if (HeroBase.Heroes[k].Data.Grade >= int.Parse(chart.CParam5))
														count++;
													break;
												case "Low":
													if (HeroBase.Heroes[k].Data.Grade <= int.Parse(chart.CParam5))
														count++;
													break;
												default:
													if (HeroBase.Heroes[k].Data.Grade == int.Parse(chart.CParam5))
														count++;
													break;
											}											
											break;
									}
								}
							}
						}

						if (count >= chart.CParam2.Length)
							return true;
						else
							return false;						
						
				}
				break;
		}

		return result;
	}

	void ApplyPlayerTargetSE(SEChart chart)
	{
		switch (chart.EffectType)
		{
			case SEEffectType.AddCurrency:
				switch (chart.EParam2)
				{
					case "Gold":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.AddGold += float.Parse(chart.EParam3);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.AddGold -= float.Parse(chart.EParam3);
								break;
						}
						break;
					case "SoulStone":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.GetSoulStoneRate += float.Parse(chart.EParam3);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.GetSoulStoneRate -= float.Parse(chart.EParam3);
								break;
						}
						break;
					case "Magicite":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.AddMagicite += float.Parse(chart.EParam3);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.AddMagicite -= float.Parse(chart.EParam3);
								break;
						}
						break;
				}
				break;
			case SEEffectType.StartStage:
				switch (chart.EParam1)
				{
					case "Inc":
						StageManager.Ins.PlayerStat.StartStage += int.Parse(chart.EParam2);						
						break;
					case "Dec":
						StageManager.Ins.PlayerStat.StartStage -= int.Parse(chart.EParam2);
						if (StageManager.Ins.PlayerStat.StartStage < 1)
							StageManager.Ins.PlayerStat.StartStage = 1;
						break;
				}
				break;
		}
	}

	void ApplyEnemyTargetSE(SEChart chart)
	{
		switch (chart.EffectType)
		{
			case SEEffectType.StatChange:
				switch (chart.TParam1)
				{
					case "All":
						switch (chart.EParam2)
						{
							case "Hp":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.EnemyHpInc += float.Parse(chart.EParam3);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.EnemyHpDec += float.Parse(chart.EParam3);
										break;
								}
								break;
							case "Spd":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.EnemySpdInc += float.Parse(chart.EParam3);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.EnemySpdDec += float.Parse(chart.EParam3);
										break;
								}
								break;
						}
						break;
					case "Minion":
						switch (chart.EParam2)
						{
							case "Hp":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.MinionHpInc += float.Parse(chart.EParam3);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.MinionHpDec += float.Parse(chart.EParam3);
										break;
								}
								break;
							case "Spd":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.MinionSpdInc += float.Parse(chart.EParam3);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.MinionSpdDec += float.Parse(chart.EParam3);
										break;
								}
								break;
						}
						break;
					case "Boss":
						switch (chart.EParam2)
						{
							case "Hp":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.BossHpInc += float.Parse(chart.EParam3);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.BossHpDec += float.Parse(chart.EParam3);
										break;
								}
								break;
							case "Spd":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.BossSpdInc += float.Parse(chart.EParam3);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.BossSpdDec += float.Parse(chart.EParam3);
										break;
								}
								break;
						}
						break;
				}
				break;
		}
	}

	void ApplyHeroTargetSE(HeroBase target, SEChart chart)
	{
		switch (chart.EffectType)
		{
			case SEEffectType.StatChange:
				switch (chart.EParam2)
				{
					case "Atk":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.AtkInc += float.Parse(chart.EParam3);
								break;
							case "Dec":
								target.Stat.AtkDec += float.Parse(chart.EParam3);
								break;
						}
						break;
					case "Spd":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.SpdInc += float.Parse(chart.EParam3);
								break;
							case "Dec":
								target.Stat.SpdDec += float.Parse(chart.EParam3);
								break;
						}
						break;					
					case "CritChance":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.CritChance += float.Parse(chart.EParam3);
								break;
							case "Dec":
								target.Stat.CritChance -= float.Parse(chart.EParam3);
								break;
						}
						break;
					case "CritDmg":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.CritDmg += float.Parse(chart.EParam3);
								break;
							case "Dec":
								target.Stat.CritDmg -= float.Parse(chart.EParam3);
								break;
						}
						break;
				}
				break;
		}
	}

}
