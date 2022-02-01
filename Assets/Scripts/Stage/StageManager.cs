using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public StageState State = StageState.None;





}

public enum StageState
{
    None,
    Start,
    Win,
    Lose,
}