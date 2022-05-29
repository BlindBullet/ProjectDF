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

	public AttackData()
	{
		Front = 1;
		Diagonal = 0;
		Piercing = 0;
		Multi = 0;
		Boom = 0;
	}

}

public enum AtkUpgradeType
{
	Front,
	Diagonal,
	Piercing,
	Multi,
	Boom,

}