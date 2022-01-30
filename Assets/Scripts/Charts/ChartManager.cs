using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartManager : SingletonObject<ChartManager>
{
    public Dictionary<string, StringData> StringData = new Dictionary<string, StringData>();

	protected override void OnCreate()
	{
		base.OnCreate();
				
		StringData = CsvParser.Ins.ParseToDict(StringData, "Charts/StringData");
		
	}



}
