using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvData : SingletonObject<CsvData>
{
	public Dictionary<string, StringChart> StringChart = new Dictionary<string, StringChart>();
	public Dictionary<string, List<HeroChart>> HeroChart = new Dictionary<string, List<HeroChart>>();	
	public Dictionary<string, FxChart> FxChart = new Dictionary<string, FxChart>();
	public Dictionary<string, List<ProjectileChart>> ProjectileChart = new Dictionary<string, List<ProjectileChart>>();
	public Dictionary<string, List<SkillChart>> SkillChart = new Dictionary<string, List<SkillChart>>();
	public Dictionary<string, List<ResultGroupChart>> ResultGroupChart = new Dictionary<string, List<ResultGroupChart>>();
	public Dictionary<string, List<HitresultChart>> HitresultChart = new Dictionary<string, List<HitresultChart>>();
	public Dictionary<string, StageChart> StageChart = new Dictionary<string, StageChart>();
	public Dictionary<string, EnemyChart> EnemyChart = new Dictionary<string, EnemyChart>();
	public Dictionary<string, SEChart> SEChart = new Dictionary<string, SEChart>();
	public Dictionary<string, RelicChart> RelicChart = new Dictionary<string, RelicChart>();
	public Dictionary<string, QuestChart> QuestChart = new Dictionary<string, QuestChart>();
	public Dictionary<string, EnemySkillChart> EnemySkillChart = new Dictionary<string, EnemySkillChart>();
	public Dictionary<string, MinionChart> MinionChart = new Dictionary<string, MinionChart>();
	public Dictionary<string, SuppliesChart> SuppliesChart = new Dictionary<string, SuppliesChart>();
	public Dictionary<int, List<SlotPowerUpChart>> SlotPowerUpChart = new Dictionary<int, List<SlotPowerUpChart>>();
	public Dictionary<string, AttendanceChart> AttendanceChart = new Dictionary<string, AttendanceChart>();
	public Dictionary<string, EquipmentChart> EquipmentChart = new Dictionary<string, EquipmentChart>();


	protected override void OnCreate()
	{
		base.OnCreate();
				
		StringChart = CsvParser.Ins.ParseToDict(StringChart, "DataTables/StringChart");
		HeroChart = CsvParser.Ins.ParseDupeKeyToDict(HeroChart, "DataTables/HeroChart");		
		FxChart = CsvParser.Ins.ParseToDict(FxChart, "DataTables/FxChart");
		ProjectileChart = CsvParser.Ins.ParseDupeKeyToDict(ProjectileChart, "DataTables/ProjectileChart");
		SkillChart = CsvParser.Ins.ParseDupeKeyToDict(SkillChart, "DataTables/SkillChart");
		ResultGroupChart = CsvParser.Ins.ParseDupeKeyToDict(ResultGroupChart, "DataTables/ResultGroupChart");
		HitresultChart = CsvParser.Ins.ParseDupeKeyToDict(HitresultChart, "DataTables/HitresultChart");
		StageChart = CsvParser.Ins.ParseToDict(StageChart, "DataTables/StageChart");
		EnemyChart = CsvParser.Ins.ParseToDict(EnemyChart, "DataTables/EnemyChart");
		SEChart = CsvParser.Ins.ParseToDict(SEChart, "DataTables/StatusEffectChart");
		RelicChart = CsvParser.Ins.ParseToDict(RelicChart, "DataTables/RelicChart");
		QuestChart = CsvParser.Ins.ParseToDict(QuestChart, "DataTables/QuestChart");
		EnemySkillChart = CsvParser.Ins.ParseToDict(EnemySkillChart, "DataTables/EnemySkillChart");
		MinionChart = CsvParser.Ins.ParseToDict(MinionChart, "DataTables/MinionChart");
		SuppliesChart = CsvParser.Ins.ParseToDict(SuppliesChart, "DataTables/SuppliesChart");
		SlotPowerUpChart = CsvParser.Ins.ParseDupeKeyToDict(SlotPowerUpChart, "DataTables/SlotPowerUpChart");
		AttendanceChart = CsvParser.Ins.ParseToDict(AttendanceChart, "DataTables/AttendanceChart");
		EquipmentChart = CsvParser.Ins.ParseToDict(EquipmentChart, "DataTables/EquipmentChart");

	}



}
