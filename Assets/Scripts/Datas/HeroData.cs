using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

[System.Serializable]
public class HeroData
{
    public string Id;
    public int Grade;    
    public bool IsOwn = false;
    
    public void Init(string id)
    {
        Id = id;
        Grade = 1;
        IsOwn = false;
    }

}
