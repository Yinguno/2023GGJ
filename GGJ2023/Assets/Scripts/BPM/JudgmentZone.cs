using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum JudgmentResult
{
    success, fail
}
public class JudgmentZone
{
    float centerPercent;
    float earlierRangePercent;
    float laterRangePercent;
    float interval;
    float startTime;
    /// <summary>
    /// 判定的中心點位置(相對於Interval的百分比)
    /// </summary>
    public float CenterPercent { get => centerPercent; set => centerPercent = value; }
    /// <summary>
    /// 以判定的中心點為標準，允許提前的量(相對於Interval的百分比)
    /// </summary>
    public float EarlierRangePercent { get => earlierRangePercent; set => earlierRangePercent = value; }
    /// <summary>
    /// 以判定的中心點為標準，允許延遲的量(相對於Interval的百分比)
    /// </summary>
    public float LaterRangePercent { get => laterRangePercent; set => laterRangePercent = value; }
    /// <summary>
    /// 整個判定區域的時間長度
    /// </summary>
    public float Interval { get => interval; set => interval = value; }
    /// <summary>
    /// 判定區域的時間起點
    /// </summary>
    public float StartTime { get => startTime; set => startTime = value; }
    public bool GetJudgmentResult(float time)
    {
        //var center = startTime + interval * centerPercent;
        var min = startTime + interval * (centerPercent - earlierRangePercent);
        var max = startTime + interval * (centerPercent + laterRangePercent);
        return time > min && time < max;
    }
    public bool IsTimeInJudgmentZone(float time)
    {
        return time > startTime && time < startTime + interval;
    }
}
