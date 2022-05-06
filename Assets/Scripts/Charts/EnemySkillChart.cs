using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillChart
{
    public string Id { get; set; }
    public EnemySkillUseConType UseConType { get; set; }
    public float UseConValue { get; set; }
    public EnemySkillTargetType Target { get; set; }
    public int TargetCount { get; set; }
    public string BeginFx { get; set; }
    public float TotalFrame { get; set; }
    public float FireFrame { get; set; }
    public string HitFx { get; set; }
    public EnemySkillHitType HitType { get; set; }
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
    public string Param4 { get; set; }
    public string DurationFx { get; set; }

}

public enum EnemySkillUseConType
{
    None,
    Die,
    CoolTime,

}

public enum EnemySkillTargetType
{
    None,
    Me,
    Close,
    All,
    AllNotMe,

}

public enum EnemySkillHitType
{
    Summon,
    Immune,
    Buff,
    Heal,

}