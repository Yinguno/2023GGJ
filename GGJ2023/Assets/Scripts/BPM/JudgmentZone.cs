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
    /// �P�w�������I��m(�۹��Interval���ʤ���)
    /// </summary>
    public float CenterPercent { get => centerPercent; set => centerPercent = value; }
    /// <summary>
    /// �H�P�w�������I���зǡA���\���e���q(�۹��Interval���ʤ���)
    /// </summary>
    public float EarlierRangePercent { get => earlierRangePercent; set => earlierRangePercent = value; }
    /// <summary>
    /// �H�P�w�������I���зǡA���\���𪺶q(�۹��Interval���ʤ���)
    /// </summary>
    public float LaterRangePercent { get => laterRangePercent; set => laterRangePercent = value; }
    /// <summary>
    /// ��ӧP�w�ϰ쪺�ɶ�����
    /// </summary>
    public float Interval { get => interval; set => interval = value; }
    /// <summary>
    /// �P�w�ϰ쪺�ɶ��_�I
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
