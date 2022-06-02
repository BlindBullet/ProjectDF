using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileChart
{
    public string Id { get; set; }
    public bool OnlyHitTarget { get; set; }
    public MoveType MoveType { get; set; }   
    public int PenCount { get; set; }
    public int Bounce { get; set; }
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
    public string HitDestroyFx { get; set; }
    public string DestroyFx { get; set; }    
    public string HitDestroyResult { get; set; }
    public string DestroyResult { get; set; }
}

public enum MoveType
{
    None,
    Direct,
    Curve,
	Beam,
	HomingBeam,
}