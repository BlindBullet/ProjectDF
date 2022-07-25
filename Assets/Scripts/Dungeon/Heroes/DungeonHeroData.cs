using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHeroData
{
	public string Id;
	public int Grade;
	public int EnchantLv;

	public void SetData(HeroData data)
	{
		Id = data.Id;
		Grade = data.Grade;
		EnchantLv = data.EnchantLv;
	}

}
