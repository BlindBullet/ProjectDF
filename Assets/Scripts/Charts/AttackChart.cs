using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChart
{
	public string Id { get; set; }
	public AttackType Type { get; set; }
	public string BeginFx { get; set; }
	public string HitFx { get; set; }
	public string Projectile { get; set; }
}


public enum AttackType
{
	Normal,
	Projectile,

}