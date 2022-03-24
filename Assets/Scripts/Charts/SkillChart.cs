using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChart
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public SkillType Type { get; set; }
    public float CoolTime { get; set; }
    public float CoolDealy { get; set; }
    public string Anim { get; set; }
    public string BeginFx { get; set; }
    public bool OnHit { get; set; }
    public float[] TotalFrame { get; set; }
    public float[] FireFrame { get; set; }
    public float[] Prob { get; set; }
    public string[] ResultGroup { get; set; }

}

public enum SkillType
{
    None,
    Attack,
    Active,
}
