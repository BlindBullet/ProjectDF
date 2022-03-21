using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvData : SingletonObject<CsvData>
{
    public Dictionary<string, StringChart> StringChart = new Dictionary<string, StringChart>();
	public Dictionary<string, HeroChart> HeroChart = new Dictionary<string, HeroChart>();
	public Dictionary<string, AttackChart> AttackChart = new Dictionary<string, AttackChart>();
	public Dictionary<string, FxChart> FxChart = new Dictionary<string, FxChart>();

	protected override void OnCreate()
	{
		base.OnCreate();
				
		StringChart = CsvParser.Ins.ParseToDict(StringChart, "DataTables/StringChart");
		HeroChart = CsvParser.Ins.ParseToDict(HeroChart, "DataTables/HeroChart");
		AttackChart = CsvParser.Ins.ParseToDict(AttackChart, "DataTables/AttackChart");
		FxChart = CsvParser.Ins.ParseToDict(FxChart, "DataTables/FxChart");


	}



}
