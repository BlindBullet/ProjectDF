using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestChart
{
    public string Id { get; set; }
    public int Lv { get; set; }
    public string Name { get; set; }    
    public int Time { get; set; }
    public int NeedGrade { get; set; }
    public int NeedGradeCount { get; set; }
    public Attr[] NeedAttr { get; set; }
    public QuestReward RewardType { get; set; }
    public float RewardValue { get; set; }

}

public enum QuestReward
{
    None,
    Gold,
    SoulStone,
    GameSpeed,

}