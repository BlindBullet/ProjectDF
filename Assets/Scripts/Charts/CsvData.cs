using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvData : SingletonObject<CsvData>
{
	public Dictionary<string, StringChart> StringChart = new Dictionary<string, StringChart>();
	public Dictionary<string, List<HeroChart>> HeroChart = new Dictionary<string, List<HeroChart>>();
	public Dictionary<string, AttackChart> AttackChart = new Dictionary<string, AttackChart>();
	public Dictionary<string, FxChart> FxChart = new Dictionary<string, FxChart>();
	public Dictionary<string, List<ProjectileChart>> ProjectileChart = new Dictionary<string, List<ProjectileChart>>();
	public Dictionary<string, SkillChart> SkillChart = new Dictionary<string, SkillChart>();
	public Dictionary<string, List<ResultGroupChart>> ResultGroupChart = new Dictionary<string, List<ResultGroupChart>>();
	public Dictionary<string, List<HitresultChart>> HitresultChart = new Dictionary<string, List<HitresultChart>>();
	public Dictionary<int, List<StageChart>> StageChart = new Dictionary<int, List<StageChart>>();
	public Dictionary<string, EnemyChart> EnemyChart = new Dictionary<string, EnemyChart>();
	public Dictionary<string, SEChart> SEChart = new Dictionary<string, SEChart>();

	protected override void OnCreate()
	{
		base.OnCreate();
				
		StringChart = CsvParser.Ins.ParseToDict(StringChart, "DataTables/StringChart");
		HeroChart = CsvParser.Ins.ParseDupeKeyToDict(HeroChart, "DataTables/HeroChart");
		AttackChart = CsvParser.Ins.ParseToDict(AttackChart, "DataTables/AttackChart");
		FxChart = CsvParser.Ins.ParseToDict(FxChart, "DataTables/FxChart");
		ProjectileChart = CsvParser.Ins.ParseDupeKeyToDict(ProjectileChart, "DataTables/ProjectileChart");
		SkillChart = CsvParser.Ins.ParseToDict(SkillChart, "DataTables/SkillChart");
		ResultGroupChart = CsvParser.Ins.ParseDupeKeyToDict(ResultGroupChart, "DataTables/ResultGroupChart");
		HitresultChart = CsvParser.Ins.ParseDupeKeyToDict(HitresultChart, "DataTables/HitresultChart");
		StageChart = CsvParser.Ins.ParseDupeKeyToDict(StageChart, "DataTables/StageChart");
		EnemyChart = CsvParser.Ins.ParseToDict(EnemyChart, "DataTables/EnemyChart");
		SEChart = CsvParser.Ins.ParseToDict(SEChart, "DataTables/StatusEffectChart");

	}



}
