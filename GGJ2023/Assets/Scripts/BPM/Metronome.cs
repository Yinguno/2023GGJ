using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour, IMetronome
{

    const float secondsInAMinute = 60f;
    float bpm = 123;
    float currentTime = 0;
    Track track;
    int currentBeatIndex = 0;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform displayCube;
    [SerializeField] AudioSource musicAudioSource;

    public bool IsStop { get; private set; } = true;

    public event Action<int> NextBeatEvent;

    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
    }
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
    private void StartMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = track.music;
        musicAudioSource.time = track.musicStartOffset;
        musicAudioSource.Play();
    }
    public void StartTrack(Track track)
    {
        this.track = track;
        this.bpm = track.bpm;
        currentTime = 0;
        StartMusic();
        IsStop = true;
    }
    public void Go()
    {
        IsStop = false;
    }
    public void Stop()
    {
        musicAudioSource.Stop();
        IsStop = true;
    }
    public int GetCurrentBeatIndex()
    {
        var interval = secondsInAMinute / bpm;
        return Mathf.CeilToInt(currentTime / interval)-1;
    }
    public bool GetHitResult()
    {
        return track?.GetJudgment(currentTime) ?? false;
    }
    public bool GetHitResult_test()
    {
        if (track == null)
        {
            return false;
        }

        (float percent, bool isHit, int index) = track.GetJudgment_test(currentTime);
        text.text = $"index:{index}\npercent:{percent}\nisHit:{isHit}";
        displayCube.localScale = new Vector3(percent, displayCube.localScale.y, displayCube.localScale.z);
        return isHit;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
        
}
