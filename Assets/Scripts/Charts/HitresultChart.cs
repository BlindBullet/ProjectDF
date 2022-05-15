using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitresultChart
{
    public string Id { get; set; } 
    public float Prob { get; set; }
    public HitType Type{ get; set; }
    public string HitFx { get; set; }
    public string DurationFx { get; set; }
    public FactorOwner FactorOwner { get; set; }
    public StatType FactorType { get; set; }
    public StatType StatType { get; set; }
    public float Value { get; set; }
    public float ValuePercent { get; set; }
    public float DurationTime { get; set; }
    public float StiffTime { get; set; }
    public string[] SummonIds { get; set; }
    public float[] SummonPosX { get; set; }
    public float[] SummonPosY { get; set; }
}

public enum HitType
{
    None,
    Dmg,
    Buff,
    Debuff,
    Push,
    Stun,
    Summon,

}

public enum FactorOwner
{
    None,
    Caster,
    Target,

}

public enum StatType
{
    None,
    Atk,
    Spd,
    MaxHp,
    CurHp,
    CritChance,
    CritDmg,
    PenCount,

}