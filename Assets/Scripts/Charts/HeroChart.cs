using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroChart
{
	public string Id { get; set; }
	public string StrName { get; set; }
	public string StrDesc { get; set; }
	public string Model { get; set; }
	public Attr Attr { get; set; }
	public int Grade { get; set; }
	public string Origin { get; set; }
	public double Atk { get; set; }
	public float Spd { get; set; }
	public float Range { get; set; }	
	public string BasicAttack { get; set; }
	public string Skill { get; set; }	
	public double Cost { get; set; }
	public string CollectionEffect { get; set; }

}

public enum Attr
{
	None,
	Red,
	Green,
	Blue,

}

public enum CostType
{
	None,
	Gold,
	Gem,
	Magicite,
}