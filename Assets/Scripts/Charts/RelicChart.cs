using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicChart
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Icon { get; set; }
    public int MaxLv { get; set; }
    public string Effect { get; set; }
    public CostType PriceCostType { get; set; }
    public double Price { get; set; }
    public CostType LvUpCostType { get; set; }
    public double LvUpCost { get; set; }
    public double LvUpCostIncValue { get; set; }
    public float LvUpCostIncRate { get; set; }

}
