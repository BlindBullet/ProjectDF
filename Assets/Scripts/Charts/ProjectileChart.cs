using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileChart
{
    public string Id { get; set; }
    public PMoveType MoveType { get; set; }        
    public float PosX { get; set; }
    public float Angle { get; set; }
    public float Speed { get; set; }    
    public float[] BPos1 { get; set; }
    public float[] BPos1R { get; set; }
    public float[] BPos2 { get; set; }
    public float[] BPos2R { get; set; }
    public float Lifetime { get; set; }
    public string Model { get; set; }
    public string BeginFx { get; set; }
    public string DurationFx { get; set; }
    public string HitDestroyFx { get; set; }
    public string DestroyFx { get; set; }    
    public string HitResultGroupId { get; set; }
    public string DestroyResultGroupId { get; set; }
}

public enum PMoveType
{
    None,
    Direct,
    Curve,

}