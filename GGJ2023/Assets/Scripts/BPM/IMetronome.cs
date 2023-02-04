using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMetronome
{
    float GetCurrentTime();
    int GetCurrentBeatIndex();
    void StartTrack(Track track);
    event Action<int> NextBeatEvent;
}