using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentChart
{
    public string Id { get; set; }
    public string Name { get; set; }
    public EquipmentType Type { get; set; }
    public string Icon { get; set; }
    public int Grade { get; set; }
    public int Level { get; set; }
    public string EquipEffect { get; set; }
    public string EquipEffectDesc { get; set; }
    public string CollectionEffect { get; set; }
    public string CollectionEffectDesc { get; set; }

}

public enum EquipmentType
{
    None,
    Red,
    Blue,
    Green,
    Acc,
}
