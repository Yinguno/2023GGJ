using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    Metronome metronome;

    public event Action<int> BeatJudgmentEvent;

    List<int> jList = new List<int>() {2,3, 5, 8, 10, };
    Dictionary<int, bool> judgementResults = new();
    AudioSource audio;
    [SerializeField] AudioClip beat;
    [SerializeField] AudioClip hit;
    // Start is called before the first frame update
    void Start()
    {
        audio= GetComponent<AudioSource>();
        metronome = GetComponent<Metronome>();
        metronome.NextBeatEvent += OnNextBeat;

        metronome.StartTrack(new Track(bpm: 90, jList, totalBeats: 50));
    }

    private void OnNextBeat(int index)
    {
        audio.PlayOneShot(beat);
        Debug.Log($"i:{index},time={Time.time}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            metronome.StartTrack(new Track(bpm: 90, jList, totalBeats: 50));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(metronome.GetHitResult());
        }
            CheckAHit();

        //Debug.Log(metronome.GetHitResult());
    }

    private void CheckAHit()
    {
        if (metronome.GetHitResult())
        {
            Debug.Log("hit");
            if (!judgementResults.ContainsKey(metronome.GetCurrentBeatIndex()))
            {
                judgementResults.Add(metronome.GetCurrentBeatIndex(), true);
                Debug.Log("success");
                audio.PlayOneShot(hit);
            }
            else
            {
                Debug.Log("more than one click");
            }
        }
        else
        {
            //Debug.Log("not target to hit");
        }
    }
}
