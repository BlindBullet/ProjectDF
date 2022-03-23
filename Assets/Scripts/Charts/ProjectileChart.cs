using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileChart
{
    public string Id { get; set; }
    public PMoveType MoveType { get; set; }
    public bool HitOnlyTarget { get; set; }
    public int PenCount { get; set; }
    public float PosX { get; set; }
    public float Angle { get; set; }
    public float Speed { get; set; }
    public float DelayTime { get; set; }
    public float Lifetime { get; set; }
    public string Model { get; set; }
    public string BeginFx { get; set; }
    public string DurationFx { get; set; }
    public string HitDestroyFx { get; set; }
    public string DestroyFx { get; set; }
    public string HitresultId { get; set; }
    public string HitResultGroupId { get; set; }
    public string DestroyResultGroupId { get; set; }
}

public enum PMoveType
{
    None,
    Direct,

}