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
    public int DeploySlotNo;
    
    public void Init(List<HeroChart> chart)
    {
        HeroChart data = chart[0];

        Id = chart[0].Id;
        Grade = chart[0].Grade;
        IsOwn = false;
        DeploySlotNo = -1;
    }

    public void Upgrade()
    {
        Grade++;
    }

}
