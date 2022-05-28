using System;
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
			HeroBase.Heroes[i].BuffCon.CalcStat();
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
				case SETargetType.Minion:
					ApplyMinionTargetSE(chart);
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

						switch (chart.CParam2[0])
						{
							case "High":
								if (gradeSum >= int.Parse(chart.CParam3, System.Globalization.CultureInfo.InvariantCulture))
									return true;
								else
									return false;
							case "Low":
								if (gradeSum <= int.Parse(chart.CParam3, System.Globalization.CultureInfo.InvariantCulture))
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
									if (CsvData.Ins.HeroChart[_heroes[i].Id][_heroes[i].Grade - 1].Attr == Attr.Red)
										count++;
								}
								break;
							case "Green":
								for (int i = 0; i < HeroBase.Heroes.Count; i++)
								{
									if (CsvData.Ins.HeroChart[_heroes[i].Id][_heroes[i].Grade - 1].Attr == Attr.Green)
										count++;
								}
								break;
							case "Blue":
								for (int i = 0; i < HeroBase.Heroes.Count; i++)
								{
									if (CsvData.Ins.HeroChart[_heroes[i].Id][_heroes[i].Grade - 1].Attr == Attr.Blue)
										count++;
								}
								break;
							default:
								for (int i = 0; i < HeroBase.Heroes.Count; i++)
								{
									if (CsvData.Ins.HeroChart[_heroes[i].Id][_heroes[i].Grade - 1].Attr == Attr.None)
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

	void ApplyPlayerTargetSE(SEChart chart)
	{
		switch (chart.EffectType)
		{			
			case SEEffectType.Stage:
				switch (chart.EParam2)
				{					
					case "SoulStoneRate":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.GetSoulStoneRate += float.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.GetSoulStoneRate -= float.Parse(chart.EParam5);
								break;
						}
						break;
				}
				break;
			case SEEffectType.Ascension:
				switch (chart.EParam2)
				{
					case "StartStage":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.StartStage += int.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.StartStage -= int.Parse(chart.EParam5);
								if (StageManager.Ins.PlayerStat.StartStage < 1)
									StageManager.Ins.PlayerStat.StartStage = 1;
								break;
						}
						break;
					case "Magicite":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.AscensionReward += float.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.AscensionReward -= float.Parse(chart.EParam5);
								break;
						}
						break;
					case "Gold":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.AscensionGold += double.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.AscensionGold -= double.Parse(chart.EParam5);
								break;
						}
						break;
				}
				break;
			case SEEffectType.OfflineReward:
				switch (chart.EParam2)
				{
					case "LimitTime":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.OfflineRewardLimitMin += int.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.OfflineRewardLimitMin -= int.Parse(chart.EParam5);
								break;
						}
						break;
					case "Amount":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.OfflineRewardAdd += float.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.OfflineRewardAdd -= float.Parse(chart.EParam5);
								break;
						}
						break;
				}
				break;
			case SEEffectType.Quest:
				switch (chart.EParam2)
				{
					case "Reward":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.QuestReward += float.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.QuestReward -= float.Parse(chart.EParam5);
								break;
						}
						break;
					case "Time":
						switch (chart.EParam1)
						{
							case "Inc":
								StageManager.Ins.PlayerStat.QuestTime += float.Parse(chart.EParam5);
								break;
							case "Dec":
								StageManager.Ins.PlayerStat.QuestTime -= float.Parse(chart.EParam5);
								break;
						}
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
						switch (chart.TParam2[0])
						{							
							case "Attr":
								switch (chart.TParam3)
								{
									case "Red":
										switch (chart.EParam2)
										{
											case "Hp":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);														
														StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);														
														StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Spd":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);														
														StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);														
														StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Def":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);														
														StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);														
														StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Gold":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Red] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Red] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Red] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Red] += float.Parse(chart.EParam5);
														break;
												}
												break;
										}
										break;
									case "Blue":
										switch (chart.EParam2)
										{
											case "Hp":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);
														break;
												}
												break;
											case "Spd":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);
														break;
												}
												break;
											case "Def":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);
														break;
												}
												break;
											case "Gold":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Blue] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Blue] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Blue] += float.Parse(chart.EParam5);
														break;
												}
												break;
										}
										break;
									case "Green":
										switch (chart.EParam2)
										{
											case "Hp":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);
														break;
												}
												break;
											case "Spd":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);
														break;
												}
												break;
											case "Def":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);
														break;
												}
												break;
											case "Gold":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Green] += float.Parse(chart.EParam5);
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Green] += float.Parse(chart.EParam5);
														StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Green] += float.Parse(chart.EParam5);
														break;
												}
												break;
										}
										break;
								}
								break;
							default:
								switch (chart.EParam2)
								{
									case "Hp":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Def":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Gold":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyGoldInc[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyGoldInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyGoldDec[Attr.Green] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.BossEnemyGoldDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;								
						}						
						break;
					case "Normal":
						switch (chart.TParam2[0])
						{
							case "Attr":
								switch (chart.TParam3)
								{
									case "Red":
										switch (chart.EParam2)
										{
											case "Hp":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Spd":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Def":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);														
														break;
												}
												break;
										}
										break;
									case "Blue":
										switch (chart.EParam2)
										{
											case "Hp":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Spd":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Def":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);														
														break;
												}
												break;
										}
										break;
									case "Green":
										switch (chart.EParam2)
										{
											case "Hp":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Spd":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);														
														break;
												}
												break;
											case "Def":
												switch (chart.EParam1)
												{
													case "Inc":
														StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);														
														break;
													case "Dec":
														StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);														
														break;
												}
												break;
										}
										break;
								}
								break;
							default:
								switch (chart.EParam2)
								{
									case "Hp":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);												
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);												
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);												
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);												
												break;
										}
										break;
									case "Def":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);												
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.NormalEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);												
												break;
										}
										break;
								}
								break;
						}
						break;
					case "Boss":
						switch (chart.TParam3)
						{
							case "Red":
								switch (chart.EParam2)
								{
									case "Hp":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Def":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;
							case "Blue":
								switch (chart.EParam2)
								{
									case "Hp":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Def":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;
							case "Green":
								switch (chart.EParam2)
								{
									case "Hp":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Def":
										switch (chart.EParam1)
										{
											case "Inc":												
												StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":												
												StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;
						}
						break;
					default:
						switch (chart.EParam2)
						{
							case "Hp":
								switch (chart.EParam1)
								{
									case "Inc":										
										StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyHpInc[Attr.Green] += float.Parse(chart.EParam5);
										break;
									case "Dec":										
										StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyHpDec[Attr.Green] += float.Parse(chart.EParam5);
										break;
								}
								break;
							case "Spd":
								switch (chart.EParam1)
								{
									case "Inc":										
										StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemySpdInc[Attr.Green] += float.Parse(chart.EParam5);
										break;
									case "Dec":										
										StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemySpdDec[Attr.Green] += float.Parse(chart.EParam5);
										break;
								}
								break;
							case "Def":
								switch (chart.EParam1)
								{
									case "Inc":										
										StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyDefInc[Attr.Green] += float.Parse(chart.EParam5);
										break;
									case "Dec":										
										StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.BossEnemyDefDec[Attr.Green] += float.Parse(chart.EParam5);
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
								target.Stat.AtkInc += float.Parse(chart.EParam5);
								break;
							case "Dec":
								target.Stat.AtkDec += float.Parse(chart.EParam5);
								break;
						}
						break;
					case "Spd":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.SpdInc += float.Parse(chart.EParam5);
								break;
							case "Dec":
								target.Stat.SpdDec += float.Parse(chart.EParam5);
								break;
						}
						break;					
					case "CritChance":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.CritChanceInc += float.Parse(chart.EParam5);
								break;
							case "Dec":
								target.Stat.CritChanceDec += float.Parse(chart.EParam5);
								break;
						}
						break;
					case "CritDmg":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.CritDmgInc += float.Parse(chart.EParam5);
								break;
							case "Dec":
								target.Stat.CritDmgDec += float.Parse(chart.EParam5);
								break;
						}
						break;
					case "PenCount":
						switch (chart.EParam1)
						{
							case "Inc":
								target.Stat.PenCountInc += int.Parse(chart.EParam5);
								break;
							case "Dec":
								target.Stat.PenCountDec += int.Parse(chart.EParam5);
								break;
						}
						break;
				}
				break;
		}
	}

	void ApplyMinionTargetSE(SEChart chart)
	{
		switch (chart.EffectType)
		{
			case SEEffectType.StatChange:
				switch (chart.TParam1)
				{
					case "Attr":
						switch (chart.TParam2[0])
						{
							case "Red":
								switch (chart.EParam2)
								{
									case "Atk":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Red] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Red] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Red] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Red] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;
							case "Blue":
								switch (chart.EParam2)
								{
									case "Atk":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Blue] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Blue] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Blue] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Blue] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;
							case "Green":
								switch (chart.EParam2)
								{
									case "Atk":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;
							default:
								switch (chart.EParam2)
								{
									case "Atk":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
									case "Spd":
										switch (chart.EParam1)
										{
											case "Inc":
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Green] += float.Parse(chart.EParam5);
												break;
											case "Dec":
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Red] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Blue] += float.Parse(chart.EParam5);
												StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Green] += float.Parse(chart.EParam5);
												break;
										}
										break;
								}
								break;
						}
						break;
					default:
						switch (chart.EParam2)
						{
							case "Atk":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionAtkInc[Attr.Green] += float.Parse(chart.EParam5);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionAtkDec[Attr.Green] += float.Parse(chart.EParam5);
										break;
								}
								break;
							case "Spd":
								switch (chart.EParam1)
								{
									case "Inc":
										StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionSpdInc[Attr.Green] += float.Parse(chart.EParam5);
										break;
									case "Dec":
										StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Red] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Blue] += float.Parse(chart.EParam5);
										StageManager.Ins.PlayerStat.MinionSpdDec[Attr.Green] += float.Parse(chart.EParam5);
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
