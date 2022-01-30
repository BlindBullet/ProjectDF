using UnityEngine;

public abstract class CoolTimer
{
	protected float _scaleChangedCorrection = 0;

	public float OriginalActiveTime { get; private set; }

	public float OriginalLength { get; private set; }

	public float OriginalScale { get; private set; } = 1f; //cooltime accelerator

	public bool IsCooltimeEnd
	{
		get
		{
			if (GetCurrentTime() > GetCoolEndTime())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public static T CreateCoolTimer<T>(float f_d, bool IsUsableAtStart) where T : CoolTimer, new()
	{
		var res = new T();
		if (IsUsableAtStart)
		{
			res.OriginalActiveTime = float.MinValue;//무조건 쿨타임이 끝나있다.
		}
		else
		{
			res.OriginalActiveTime = float.MaxValue - f_d - 1.0f;//리셋전에는 사용할 수 없게 해놓는다.
		}
		res.OriginalLength = f_d;
		return res;
	}

	protected abstract float GetCurrentTime();

	public void SetCoolFinished()
	{
		OriginalActiveTime = float.MinValue;
	}

	public void RestartCooltime()
	{
		OriginalActiveTime = GetCurrentTime();
		_scaleChangedCorrection = 0;
	}

	public void RestartCooltime(float withDifferentTime)
	{
		RestartCooltime();
		OriginalLength = withDifferentTime;
	}

	public void RestartCoolTimeWithPassedTime(float passedTime)
	{
		RestartCooltime();
		_scaleChangedCorrection = passedTime;
	}

	/// <summary>
	/// 잔여 시간과 전체 시간을 비율에 맞게 변경한다.
	/// </summary>
	public void ScaleTimer(float newScale)
	{
		var ratio = newScale / OriginalScale;
		var truePassedTime = GetCurrentTime() - OriginalActiveTime;
		//new correction value
		_scaleChangedCorrection = (_scaleChangedCorrection + truePassedTime) * ratio - (truePassedTime);

		OriginalScale = newScale;
	}

	public float GetRemainingCoolTime()
	{
		var scaledLength = OriginalLength * OriginalScale;
		var scaledStartTime = OriginalActiveTime - _scaleChangedCorrection;
		var currentTime = GetCurrentTime();
		var res = scaledLength + scaledStartTime - currentTime;
		return res;
	}

	public float GetRatio()
	{
		var scaledLength = OriginalLength * OriginalScale;
		var currentTime = GetCurrentTime();
		return Mathf.Clamp01((GetCoolEndTime() - currentTime) / scaledLength);
	}

	public float GetCoolEndTime()
	{
		return OriginalActiveTime + OriginalLength * OriginalScale;
	}

	public string GetTimerStatus()
	{
		var str = GetRemainingCoolTime();
		return str.ToString("F2") + "/" + (OriginalLength * OriginalScale).ToString("F2");
	}
}

public static class TimerExtension
{
	public static string ToString(this CoolTimer timer)
	{
		var str = timer.GetRemainingCoolTime();
		return str.ToString("F2") + "/" + (timer.OriginalLength * timer.OriginalScale).ToString("F2");
	}
}

public class RealCoolTimer : CoolTimer
{
	//private RealCoolTimer()
	//{

	//}
	protected override float GetCurrentTime()
	{
		return Time.realtimeSinceStartup;
	}
}

public class GameCoolTimer : CoolTimer
{
	//private GameCoolTimer()
	//{

	//}
	protected override float GetCurrentTime()
	{
		return Time.time;
	}
}
