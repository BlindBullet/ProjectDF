using System;

namespace TimeUtillity
{
    public static class TimeExtension
    {
        public static string ToTimeString(this TimeSpan time)
        {
            return ((int)time.TotalHours).ToString("D2") + ":" + time.Minutes.ToString("D2") + ":" + time.Seconds.ToString("D2");
        }
    }
}