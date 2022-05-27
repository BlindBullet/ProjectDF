using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEChart
{
	public string Id { get; set; }
	public int Lv { get; set; }
	public string Desc { get; set; }
	public SEConType ConType { get; set; }
	public string CParam1 { get; set; }
	public string[] CParam2 { get; set; }
	public string CParam3 { get; set; }
	public string CParam4 { get; set; }
	public string CParam5 { get; set; }
	public SETargetType TargetType { get; set; }
	public string TParam1 { get; set; }
	public string[] TParam2 { get; set; }
	public string TParam3 { get; set; }
	public SEEffectType EffectType { get; set; }
	public string EParam1 { get; set; }
	public string EParam2 { get; set; }
	public string EParam3 { get; set; }
	public string EParam4 { get; set; }
	public string EParam5 { get; set; }

}

public enum SEConType
{
	None,
	DeployHero,
	OwnHero,

}

public enum SETargetType
{
	None,
	Hero,
	Enemy,
	Player,
	Minion,
}

public enum SEEffectType
{
	None,
	StatChange,
	Stage,
	Ascension,
	OfflineReward,

}