using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoSingleton<SEManager>
{
	public List<SEData> SeList = new List<SEData>();

	public void Apply()
	{
		SeList.Clear();

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
					SEChart chart = CsvData.Ins.SEChart[heroCharts[k].CollectionEffect];
					SEData data = new SEData(chart, heroes[i].Grade);
					
					data.SetValue();

					SeList.Add(data);
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

			SEChart seChart = CsvData.Ins.SEChart[chart.Effect];
			SEData data = new SEData(seChart, relics[i].Lv);
							
			data.SetValue();
			
			SeList.Add(data);
		}

		//캐슬 효과 불러오기
		List<RelicData> castles = StageManager.Ins.PlayerData.Castles;

		for (int i = 0; i < castles.Count; i++)
		{
			if (!castles[i].isOwn)
				continue;

			RelicChart chart = CsvData.Ins.RelicChart[castles[i].Id];

			SEChart seChart = CsvData.Ins.SEChart[chart.Effect];
			SEData data = new SEData(seChart, castles[i].Lv);

			data.SetValue();

			SeList.Add(data);
		}

		InitAllStat();
		ApplyAllSe(SeList);
	}

	void InitAllStat()
	{
		//플레이어 스탯 초기화
		StageManager.Ins.PlayerStat.Init();

		List<SlotData> slots = StageManager.Ins.PlayerData.Slots;
		
		//배치 영웅 스탯 초기화		
		for (int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			for(int k = 0; k < slots.Count; k++)
			{
				if (HeroBase.Heroes[i].Data.SlotNo == slots[k].No)
					HeroBase.Heroes[i].Stat.InitData(HeroBase.Heroes[i].Data, slots[k].Lv);
			}
		}
	}

	void ApplyAllSe(List<SEData> seList)
	{
		for (int i = 0; i < seList.Count; i++)
		{	
			ApplySE(seList[i]);			
		}

		for(int i = 0; i < HeroBase.Heroes.Count; i++)
		{
			HeroBase.Heroes[i].BuffCon.CalcStat();
		}

		PlayerBuffManager.Ins.RunAllBuffs();

		for (int i = 0; i < StageManager.Ins.Slots.Count; i++)
		{
			StageManager.Ins.Slots[i].SetLvUpCost();
		}
	}

	void ApplySE(SEData data)
	{
		if (CheckSECon(data.Chart))
		{
			switch (data.Chart.TargetType)
			{
				case SETargetType.Player:
					ApplyPlayerTargetSE(data);
					break;
				case SETargetType.Hero:
					List<HeroBase> targets = new List<HeroBase>();

					switch (data.Chart.TParam1)
					{
						case "All":
							targets = HeroBase.Heroes;
							break;
						case "Attr":
							for(int i = 0; i < HeroBase.Heroes.Count; i++)
							{
								if (HeroBase.Heroes[i].Stat.Attr.ToString() == data.Chart.TParam2[0])
									targets.Add(HeroBase.Heroes[i]);
							}
							break;
						case "Slot":
							for(int i = 0; i < data.Chart.TParam2.Length; i++)
							{
								for (int k = 0; k < HeroBase.Heroes.Count; k++)
								{
									if (HeroBase.Heroes[k].Data.SlotNo == int.Parse(data.Chart.TParam2[i], System.Globalization.CultureInfo.InvariantCulture))
										targets.Add(HeroBase.Heroes[k]);
								}
							}
							break;
						case "Grade":
							for (int i = 0; i < HeroBase.Heroes.Count; i++)
							{
								switch (data.Chart.TParam2[0])
								{
									case "High":
										if (HeroBase.Heroes[i].Data.Grade >= int.Parse(data.Chart.TParam3, System.Globalization.CultureInfo.InvariantCulture))
											targets.Add(HeroBase.Heroes[i]);
										break;
									case "Low":
										if (HeroBase.Heroes[i].Data.Grade <= int.Parse(data.Chart.TParam3, System.Globalization.CultureInfo.InvariantCulture))
											targets.Add(HeroBase.Heroes[i]);
										break;
									default:
										if (HeroBase.Heroes[i].Data.Grade == int.Parse(data.Chart.TParam3, System.Globalization.CultureInfo.InvariantCulture))
											targets.Add(HeroBase.Heroes[i]);
										break;
								}
							}
							break;
					}

					for(int i = 0; i < targets.Count; i++)
					{						
						ApplyHeroTargetSE(targets[i], data);
					}
					break;
				case SETargetType.Enemy:
					ApplyEnemyTargetSE(data);
					break;
				case SETargetType.Minion:
					ApplyMinionTargetSE(data);
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
								if (gradeSum >= int.Parse(chart.CParam2[0], System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
							case "Low":
								if (gradeSum <= int.Parse(chart.CParam2[0], System.Globalization.CultureInfo.InvariantCulture))
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
													if (HeroBase.Heroes[k].Data.Grade >= int.Parse(chart.CParam5, System.Globalization.CultureInfo.InvariantCulture))
														count++;
													break;
												case "Low":
													if (HeroBase.Heroes[k].Data.Grade <= int.Parse(chart.CParam5, System.Globalization.CultureInfo.InvariantCulture))
														count++;
													break;
												default:
													if (HeroBase.Heroes[k].Data.Grade == int.Parse(chart.CParam5, System.Globalization.CultureInfo.InvariantCulture))
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
			case SEConType.OwnHero:
				List<HeroData> _heroes = StageManager.Ins.PlayerData.Heroes;

				switch (chart.CParam1)
				{
					case "Attr":						
						switch (chart.CParam2[0])
						{
							case "Red":
								for (int i = 0; i < _heroes.Count; i++)
								{
									for(int k = 0; k < CsvData.Ins.HeroChart[_heroes[i].Id].Count; k++)
									{
										if(CsvData.Ins.HeroChart[_heroes[i].Id][k].Grade == _heroes[i].Grade)
										{
											if (CsvData.Ins.HeroChart[_heroes[i].Id][k].Attr == Attr.Red)
												count++;
										}
									}									
								}
								break;
							case "Green":
								for (int i = 0; i < _heroes.Count; i++)
								{
									for (int k = 0; k < CsvData.Ins.HeroChart[_heroes[i].Id].Count; k++)
									{
										if (CsvData.Ins.HeroChart[_heroes[i].Id][k].Grade == _heroes[i].Grade)
										{
											if (CsvData.Ins.HeroChart[_heroes[i].Id][k].Attr == Attr.Green)
												count++;
										}
									}
								}
								break;
							case "Blue":
								for (int i = 0; i < _heroes.Count; i++)
								{
									for (int k = 0; k < CsvData.Ins.HeroChart[_heroes[i].Id].Count; k++)
									{
										if (CsvData.Ins.HeroChart[_heroes[i].Id][k].Grade == _heroes[i].Grade)
										{
											if (CsvData.Ins.HeroChart[_heroes[i].Id][k].Attr == Attr.Blue)
												count++;
										}
									}
								}
								break;
							default:
								
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
					case "Grade":
						for (int i = 0; i < _heroes.Count; i++)
						{
							if (_heroes[i].Grade >= int.Parse(chart.CParam2[0], System.Globalization.CultureInfo.InvariantCulture))
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
				}
				break;
		}

		return result;
	}

	void ApplyPlayerTargetSE(SEData data)
	{	
		switch (data.Chart.EffectType)
		{			
			case SEEffectType.Stage:
				switch (data.Chart.EParam2)
				{
					case "AutoLvUp":						
						StageManager.Ins.TopBar.SetAutoLvUpBtn();
						break;
					case "SoulStoneRate":						
						switch (data.Chart.EParam1)
						{
							case "Inc":								
								StageManager.Ins.PlayerStat.GetSoulStoneRate += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.GetSoulStoneRate -= (float)data.Value;
								break;
						}
						break;
					case "LvUpGold":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.LvUpGold += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.LvUpGold -= (float)data.Value;
								break;
						}
						break;
					case "TouchSkillCool":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.TouchSkillCoolDecProb += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.TouchSkillCoolDecProb -= (float)data.Value;
								break;
						}
						break;
					case "AutoSkill":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.UseAutoSkillRate += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.UseAutoSkillRate -= (float)data.Value;
								break;
						}
						break;
					case "Moat":						
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.MoatSlowRate += (float)data.Value;

								if(StageManager.Ins.PlayerStat.MoatSlowRate > 0f)
								{
									StageManager.Ins.MoatOn();
								}
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.MoatSlowRate -= (float)data.Value;
								break;
						}
						break;
				}
				break;
			case SEEffectType.Ascension:
				switch (data.Chart.EParam2)
				{
					case "StartStage":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.StartStage += (int)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.StartStage -= (int)data.Value;
								if (StageManager.Ins.PlayerStat.StartStage < 1)
									StageManager.Ins.PlayerStat.StartStage = 1;
								break;
						}
						break;
					case "Magicite":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.AscensionReward += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.AscensionReward -= (float)data.Value;
								break;
						}
						break;
					case "Gold":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.AscensionGold += data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.AscensionGold -= data.Value;
								break;
						}
						break;
				}
				break;
			case SEEffectType.OfflineReward:
				switch (data.Chart.EParam2)
				{
					case "Time":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.OfflineRewardLimitMin += (int)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.OfflineRewardLimitMin -= (int)data.Value;
								break;
						}
						break;
					case "Amount":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.OfflineRewardAdd += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.OfflineRewardAdd -= (float)data.Value;
								break;
						}
						break;
				}
				break;
			case SEEffectType.Quest:
				switch (data.Chart.EParam2)
				{
					case "Reward":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.QuestReward += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.QuestReward -= (float)data.Value;
								break;
						}
						break;
					case "Time":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.QuestTime += (float)data.Value;
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.QuestTime -= (float)data.Value;
								break;
						}
						break;						
				}
				break;
		}
	}

	void ApplyEnemyTargetSE(SEData data)
	{
		switch (data.Chart.EffectType)
		{
			case SEEffectType.StatChange:
				switch (data.Chart.TParam1)
				{
					case "All":
						switch (data.Chart.TParam2[0])
						{							
							case "Attr":
								switch (data.Chart.TParam3)
								{
									case "Red":
										switch (data.Chart.EParam2)
										{
											case "Hp":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += (float)data.Value;														
														StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += (float)data.Value;														
														StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += (float)data.Value;														
														break;
												}
												break;
											case "Spd":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += (float)data.Value;														
														StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += (float)data.Value;														
														StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += (float)data.Value;														
														break;
												}
												break;
											case "Def":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += (float)data.Value;														
														StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += (float)data.Value;														
														StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += (float)data.Value;														
														break;
												}
												break;
											case "Gold":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Red] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Red] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Red] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Red] += (float)data.Value;
														break;
												}
												break;
										}
										break;
									case "Blue":
										switch (data.Chart.EParam2)
										{
											case "Hp":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += (float)data.Value;
														break;
												}
												break;
											case "Spd":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += (float)data.Value;
														break;
												}
												break;
											case "Def":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += (float)data.Value;
														break;
												}
												break;
											case "Gold":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Blue] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Blue] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Blue] += (float)data.Value;
														break;
												}
												break;
										}
										break;
									case "Green":
										switch (data.Chart.EParam2)
										{
											case "Hp":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += (float)data.Value;
														break;
												}
												break;
											case "Spd":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += (float)data.Value;
														break;
												}
												break;
											case "Def":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += (float)data.Value;
														break;
												}
												break;
											case "Gold":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Green] += (float)data.Value;
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Green] += (float)data.Value;
														StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Green] += (float)data.Value;
														break;
												}
												break;
										}
										break;
								}
								break;
							default:
								switch (data.Chart.EParam2)
								{
									case "Hp":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
									case "Def":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
									case "Gold":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Green] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
								}
								break;								
						}						
						break;
					case "Normal":
						switch (data.Chart.TParam2[0])
						{
							case "Attr":
								switch (data.Chart.TParam3)
								{
									case "Red":
										switch (data.Chart.EParam2)
										{
											case "Hp":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += (float)data.Value;														
														break;
												}
												break;
											case "Spd":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += (float)data.Value;														
														break;
												}
												break;
											case "Def":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += (float)data.Value;														
														break;
												}
												break;
										}
										break;
									case "Blue":
										switch (data.Chart.EParam2)
										{
											case "Hp":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += (float)data.Value;														
														break;
												}
												break;
											case "Spd":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += (float)data.Value;														
														break;
												}
												break;
											case "Def":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += (float)data.Value;														
														break;
												}
												break;
										}
										break;
									case "Green":
										switch (data.Chart.EParam2)
										{
											case "Hp":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += (float)data.Value;														
														break;
												}
												break;
											case "Spd":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += (float)data.Value;														
														break;
												}
												break;
											case "Def":
												switch (data.Chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += (float)data.Value;														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += (float)data.Value;														
														break;
												}
												break;
										}
										break;
								}
								break;
							default:
								switch (data.Chart.EParam2)
								{
									case "Hp":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += (float)data.Value;												
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += (float)data.Value;												
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += (float)data.Value;												
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += (float)data.Value;												
												break;
										}
										break;
									case "Def":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += (float)data.Value;												
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += (float)data.Value;												
												break;
										}
										break;
								}
								break;
						}
						break;
					case "Boss":
						switch (data.Chart.TParam3)
						{
							case "Red":
								switch (data.Chart.EParam2)
								{
									case "Hp":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += (float)data.Value;
												break;
										}
										break;
									case "Def":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += (float)data.Value;
												break;
										}
										break;
								}
								break;
							case "Blue":
								switch (data.Chart.EParam2)
								{
									case "Hp":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += (float)data.Value;
												break;
										}
										break;
									case "Def":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += (float)data.Value;
												break;
										}
										break;
								}
								break;
							case "Green":
								switch (data.Chart.EParam2)
								{
									case "Hp":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
									case "Def":
										switch (data.Chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
								}
								break;
						}
						break;
					default:
						switch (data.Chart.EParam2)
						{
							case "Hp":
								switch (data.Chart.EParam1)
								{
									case "Inc":										
										StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += (float)data.Value;
										break;
									case "Dec":										
										StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += (float)data.Value;
										break;
								}
								break;
							case "Spd":
								switch (data.Chart.EParam1)
								{
									case "Inc":										
										StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += (float)data.Value;
										break;
									case "Dec":										
										StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += (float)data.Value;
										break;
								}
								break;
							case "Def":
								switch (data.Chart.EParam1)
								{
									case "Inc":										
										StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += (float)data.Value;
										break;
									case "Dec":										
										StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += (float)data.Value;
										break;
								}
								break;
						}
						break;
				}
				break;
				
		}
	}

	void ApplyHeroTargetSE(HeroBase target, SEData data)
	{
		switch (data.Chart.EffectType)
		{
			case SEEffectType.StatChange:
				switch (data.Chart.EParam2)
				{
					case "Atk":						
						switch (data.Chart.EParam1)
						{
							case "Inc":
								if (data.Chart.Id.Contains("ce"))
									target.Stat.AtkIncRate += (float)data.Value;
								else
									target.Stat.AtkInc += (float)data.Value;
								break;
							case "Dec":
								if (data.Chart.Id.Contains("ce"))
									target.Stat.AtkDecRate += (float)data.Value;
								else
									target.Stat.AtkDec += (float)data.Value;
								break;
						}
						break;
					case "Spd":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								target.Stat.SpdInc += (float)data.Value;
								break;
							case "Dec":
								target.Stat.SpdDec += (float)data.Value;
								break;
						}
						break;					
					case "CritChance":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								target.Stat.CritChanceInc += (float)data.Value;
								break;
							case "Dec":
								target.Stat.CritChanceDec += (float)data.Value;
								break;
						}
						break;
					case "CritDmg":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								target.Stat.CritDmgInc += (float)data.Value;
								break;
							case "Dec":
								target.Stat.CritDmgDec += (float)data.Value;
								break;
						}
						break;
					case "PenCount":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								target.Stat.PenCountInc += (int)data.Value;
								break;
							case "Dec":
								target.Stat.PenCountDec += (int)data.Value;
								break;
						}
						break;
					case "CoolTime":
						switch (data.Chart.EParam1)
						{
							case "Inc":
								target.Stat.CoolTimeInc += (int)data.Value;
								break;
							case "Dec":
								target.Stat.CoolTimeDec += (int)data.Value;
								break;
						}
						break;
				}
				break;
		}
	}

	void ApplyMinionTargetSE(SEData data)
	{
		switch (data.Chart.EffectType)
		{
			case SEEffectType.StatChange:
				switch (data.Chart.TParam1)
				{
					case "Attr":
						switch (data.Chart.TParam2[0])
						{
							case "Red":
								switch (data.Chart.EParam2)
								{
									case "Atk":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Red] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Red] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Red] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Red] += (float)data.Value;
												break;
										}
										break;
								}
								break;
							case "Blue":
								switch (data.Chart.EParam2)
								{
									case "Atk":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Blue] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Blue] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Blue] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Blue] += (float)data.Value;
												break;
										}
										break;
								}
								break;
							case "Green":
								switch (data.Chart.EParam2)
								{
									case "Atk":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
								}
								break;
							default:
								switch (data.Chart.EParam2)
								{
									case "Atk":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
									case "Spd":
										switch (data.Chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Green] += (float)data.Value;
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Red] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Blue] += (float)data.Value;
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Green] += (float)data.Value;
												break;
										}
										break;
								}
								break;
						}
						break;
					default:
						switch (data.Chart.EParam2)
						{
							case "Atk":
								switch (data.Chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Green] += (float)data.Value;
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Green] += (float)data.Value;
										break;
								}
								break;
							case "Spd":
								switch (data.Chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Green] += (float)data.Value;
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Red] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Blue] += (float)data.Value;
										StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Green] += (float)data.Value;
										break;
								}
								break;
						}
						break;
				}
				break;
		}
	}

}

public class SEData
{
	public SEChart Chart;
	public int Lv;
	public double Value;	

	public SEData(SEChart chart, int lv)
	{
		Chart = chart;
		Lv = lv;
	}

	public double SetValue()
	{
		if (Lv == 0)
			return 0f;

		if (Chart.TargetType == SETargetType.Enemy && Chart.EffectType == SEEffectType.StatChange && Chart.EParam2 == "Gold")
		{						
			return Value = ConstantData.CalcValue(double.Parse(Chart.EParam5, System.Globalization.CultureInfo.InvariantCulture), Chart.LvUpIncRate, Lv);
		}

		if (Chart.TargetType == SETargetType.Hero && Chart.EffectType == SEEffectType.StatChange && Chart.EParam2 == "Atk")
		{
			return Value = ConstantData.CalcValue(double.Parse(Chart.EParam5, System.Globalization.CultureInfo.InvariantCulture), Chart.LvUpIncRate, Lv);
		}

		return Value = (double.Parse(Chart.EParam5, System.Globalization.CultureInfo.InvariantCulture) + (Chart.LvUpIncValue * (Lv - 1))) * (Chart.LvUpIncRate > 0 ? Mathf.Pow(Chart.LvUpIncRate, (Lv - 1)) : 1f);
	}

	public double NextSetValue()
	{
		if (Chart.TargetType == SETargetType.Enemy && Chart.EffectType == SEEffectType.StatChange && Chart.EParam2 == "Gold")
		{				
			return Value = ConstantData.CalcValue(double.Parse(Chart.EParam5, System.Globalization.CultureInfo.InvariantCulture), Chart.LvUpIncRate, Lv + 1);
		}

		if (Chart.TargetType == SETargetType.Hero && Chart.EffectType == SEEffectType.StatChange && Chart.EParam2 == "Atk")
		{
			return Value = ConstantData.CalcValue(double.Parse(Chart.EParam5, System.Globalization.CultureInfo.InvariantCulture), Chart.LvUpIncRate, Lv + 1);
		}

		return Value = (double.Parse(Chart.EParam5, System.Globalization.CultureInfo.InvariantCulture) + (Chart.LvUpIncValue * Lv)) * (Chart.LvUpIncRate > 0 ? Mathf.Pow(Chart.LvUpIncRate, Lv) : 1f);
	}
}