using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Metronome : MonoBehaviour,IMetronome
{

    const float secondsInAMinute = 60f;
    float bpm = 123;
    float currentTime = 0;
    Track track;
    int currentBeatIndex = 0;

    public bool IsStop { get;private set; } = true;

    public event Action<int> NextBeatEvent;

    private void Update()
    {
        if (IsStop) return;

        UpdateCurrentTrackTime();

        var newerBeatIndex = GetCurrentBeatIndex();
        if (newerBeatIndex != currentBeatIndex)
        {
            currentBeatIndex = newerBeatIndex;
            NextBeatEvent?.Invoke(currentBeatIndex);
        }
    }
    void UpdateCurrentTrackTime()
    {
        currentTime += Time.deltaTime;
    }
    public void StartTrack(Track track)
    {
        this.track = track;
        this.bpm = track.bpm;
        currentTime = 0;
        IsStop = false;
    }
    public void Stop()
    {
        IsStop = true;
    }
    public int GetCurrentBeatIndex()
    {
        var interval = secondsInAMinute / bpm;
        return Mathf.FloorToInt(currentTime / interval);
    }
    public bool GetHitResult()
    {
        return track.GetJudgment(currentTime);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
