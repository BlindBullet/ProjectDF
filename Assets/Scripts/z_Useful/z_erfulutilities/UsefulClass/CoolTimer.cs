using System;
using UnityEngine;

public class CoolTimerReal
{
    private bool _useGameOrRealTime;
    private float _activatedTime;  //acvivated time memorize
    private float _cooltimeLength;  //colldown time

    public CoolTimerReal(float f_d, bool IsUsableAtStart, bool useGameTime)
    {
        _useGameOrRealTime = useGameTime;
        if (IsUsableAtStart)
        {
            _activatedTime = float.MinValue;//무조건 쿨타임이 끝나있다.
        }
        else
        {
            _activatedTime = float.MaxValue - f_d - 1.0f;//리셋전에는 사용할 수 없게 해놓는다.
        }
        _cooltimeLength = f_d;
    }

    private float GetCurrentTime()
    {
        return _useGameOrRealTime ? Time.time: Time.realtimeSinceStartup;
    }

    public void ForceEndCooltime()
    {
        _activatedTime = float.MinValue;
    }

    public void StartCooltime()
    {
        _activatedTime = GetCurrentTime();
    }

    public void StartCooltime(float withDifferentTime)
    {
        _activatedTime = GetCurrentTime();
        _cooltimeLength = withDifferentTime;
    }

    public void ModifyTimeLength(float newTimeLength)
    {
        _cooltimeLength = newTimeLength;
    }

    public float GetRemainingCoolTime()
    {
        return Mathf.Clamp(_cooltimeLength + _activatedTime - GetCurrentTime(), 0, float.MaxValue);
    }

    public bool IsCooltimeEnd
    {
        get
        {
            if (GetCurrentTime() > _activatedTime + _cooltimeLength)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public string GetDebugTimer()
    {
        return  GetRemainingCoolTime().ToString("F2")+ "/" +_cooltimeLength.ToString("F2");
    }
}