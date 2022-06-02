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
	public int Size;
	public int Push;
	

	public AttackData()
	{
		Front = 0;
		Diagonal = 0;
		Piercing = 0;
		Multi = 0;
		Boom = 0;
		Bounce = 0;
		AtkUp = 0;
		Size = 0;
		Push = 0;
		
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
	Size,	
	Push,

}