using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChart
{
    public string Id { get; set; }
    public string StrName { get; set; }
    public string StrDesc { get; set; }
    public string Model { get; set; }
    public Shape Shape { get; set; }
    public Attr Attr { get; set; }
    public float Size { get; set; }
    public float Weight { get; set; }
    public double Hp { get; set; }
    public float Spd { get; set; }
    public float Def { get; set; }
    public double Gold { get; set; }
    public string[] Skill { get; set; }


}

public enum Shape
{
    None,
    Circle,
    Pentagon,
    PentagonO,
    Polygon,
    PolygonO,
    Octagon,

}