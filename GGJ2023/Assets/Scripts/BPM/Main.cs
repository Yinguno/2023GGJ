using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    Metronome metronome;

    public event Action<int> BeatJudgmentEvent;

    List<int> jList = new List<int>() {1,2,3,4, 5,6,7,8,9,10,11,12,13,14,15};
    Dictionary<int, bool> judgementResults = new();
    AudioSource audioSource;
    [SerializeField] PlaygroundMaker playgroundMaker;
    [SerializeField] AudioClip beat;
    [SerializeField] AudioClip hit;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        metronome = GetComponent<Metronome>();
        metronome.NextBeatEvent += OnNextBeat;

        metronome.StartTrack(new Track(bpm: 45, jList, totalBeats: 50));


    }
    float ti;

    private int serial = 0;
    private void OnNextBeat(int index)
    {
        audioSource.PlayOneShot(beat);
        playgroundMaker.DrawRoute(index, serial++);
        Debug.Log($"i:{index},next beat delta Time :{Time.time - ti},trackTime={metronome.GetCurrentTime()}");
        ti = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            metronome.StartTrack(new Track(bpm: 90, jList, totalBeats: 50));
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
              CheckAHit();
            //Debug.Log(metronome.GetHitResult());
        }

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
                audioSource.PlayOneShot(hit);
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
