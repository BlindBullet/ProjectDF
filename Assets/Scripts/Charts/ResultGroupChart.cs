using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultGroupChart
{
    public string Id { get; set; }    
    public float Prob { get; set; }
    public TargetType TargetType { get; set; }
    public TargetDetail TargetDetail { get; set; }
    public int TargetCount { get; set; }
    public bool AddDelayTarget { get; set; }
    public RangeType RangeType { get; set; }
    public float[] RnageSize { get; set; }
    public float DelayTime { get; set; }
    public string DelayBeginFx { get; set; }
    public string Hitresult { get; set; }
    public string Projectile { get; set; }

}

public enum TargetType
{
    None,
    Enemy,
    Hero,
    Me,

}

public enum TargetDetail
{
    None,
    All,
    Closest,
    
}

public enum RangeType
{
    None,
    Circle,

}