using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Track
{
    const float secondsInAMinute = 60f;
    public float bpm = 60;
    List<JudgmentZone> judgmentZoneList;
    int totalBeats = 0;
    public float musicStartOffset;
    public AudioClip music;
    public List<int> judgmentList;
    public Track(float bpm, List<int> judgmentList, int totalBeats, float musicStartOffset, AudioClip music)
    {
        this.judgmentList = judgmentList;
        this.bpm = bpm;
        var judgmentZoneInterval = secondsInAMinute / this.bpm;
        InitJudgmentZoneList(judgmentZoneInterval, judgmentList);
        this.totalBeats = totalBeats;
        this.musicStartOffset = musicStartOffset;
        this.music = music; 
    }

    void InitJudgmentZoneList(float interval, List<int> judgmentList)
    {
        judgmentZoneList = new List<JudgmentZone>();
        for (int i = 0; i < judgmentList.Count; i++)
        {
            var startTime = judgmentList[i] * interval;
            JudgmentZone zone = new()
            {
                CenterPercent = 0.5f,
                EarlierRangePercent = 0.49f,
                LaterRangePercent = 0.49f,
                StartTime = startTime,
                Interval= interval,
            };
            judgmentZoneList.Add(zone);
        }
    }
    public bool GetJudgment(float time)
    {
        JudgmentZone zone = judgmentZoneList.Find(e => e.IsTimeInJudgmentZone(time));
        if (zone == null) {
            Debug.Log("no zone");
            return false;
        }
        bool result = zone.GetJudgmentResult(time);        
        return result;
    }
    public (float percent,bool isHit,int index) GetJudgment_test(float time)
    {
        JudgmentZone zone = judgmentZoneList.Find(e => e.IsTimeInJudgmentZone(time));
        if (zone == null)
        {
            Debug.Log("no zone");
            return new(-1f,false,-1);
        }
        (float percent, bool isHit) = zone.GetJudgmentResult_test(time);
        int index = judgmentZoneList.FindIndex(e => e.IsTimeInJudgmentZone(time));
        return new (percent, isHit,  index);

    }
    public bool IsBeatIndexNeedHit(int index)
    {
        return judgmentList.Exists(e => e == index);
    }
}
