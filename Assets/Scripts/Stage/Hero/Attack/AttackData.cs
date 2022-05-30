using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
	public int Front;
	public int Diagonal;
	public int Piercing;
	public int Multi;
	public int Boom;
	public int Bounce;
	public int AtkUp;

	public AttackData()
	{
		Front = 1;
		Diagonal = 2;
		Piercing = 1;
		Multi = 0;
		Boom = 0;
		Bounce = 5;
		AtkUp = 0;
	}

}

public enum AtkUpgradeType
{
	Front,
	Diagonal,
	Piercing,
	Multi,
	Boom,
	Bounce,
	AtkUp,

}