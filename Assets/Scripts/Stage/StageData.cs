using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData", order = 1)]
public class StageData : ScriptableObject
{
    public int StageNo;
    public string Bg;
    public string Bgm;
    public List<AppearEnemy> AppearEnemies;
    public string AppearBoss;
}

[System.Serializable]
public class AppearEnemy
{
    public float AppearTime;
    public string Id;
    public int Count;
}