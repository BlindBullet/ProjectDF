using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionChart
{
    public string Id { get; set; }
    public string Model { get; set; }
    public float Size { get; set; }
    public MoveType MoveType { get; set; }
    public float MoveSpd { get; set; }
    public float[] BPos1 { get; set; }
    public float[] BPos1R { get; set; }
    public float[] BPos2 { get; set; }
    public float[] BPos2R { get; set; }
    public float Range { get; set; }
    public float AtkP { get; set; }
    public float Spd { get; set; }
    public int PenCount { get; set; }
    public Attr Attr { get; set; }
    public string Hitresult { get; set; }
    public string Projectile { get; set; }
    
}
