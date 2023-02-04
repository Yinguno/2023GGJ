using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Main : MonoBehaviour
{
    static int modifySerial = 0;
    Metronome metronome;
    [SerializeField] float musicOffset;
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] GameObject obj;

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

        StartTrack();


    }
    float ti;
    private void OnNextBeat(int index)
    {
        //audioSource.PlayOneShot(beat,0.1f);
        modifySerial++;
        playgroundMaker.DrawRoute(index, modifySerial);
        //Debug.Log($"i:{index},next beat delta Time :{Time.time - ti},trackTime={metronome.GetCurrentTime()}");
        ti = Time.time;

        if (!judgementResults.ContainsKey(index))
        {
            textMeshPro.text = "miss";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartTrack();
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
              CheckAHit();
            //Debug.Log(metronome.GetHitResult());
        }

        //Debug.Log(metronome.GetHitResult());
    }
    private void StartTrack() {
        judgementResults.Clear();
        audioSource.Stop();
        audioSource.time = musicOffset;
        audioSource.Play();
        metronome.StartTrack(new Track(bpm: 123, jList, totalBeats: 50));
    }
    
    private void CheckAHit()
    {
        if (metronome.GetHitResult())
        {
            Debug.Log("hit");
            if (!judgementResults.ContainsKey(metronome.GetCurrentBeatIndex()))
            {
                judgementResults.Add(metronome.GetCurrentBeatIndex(), true);
                textMeshPro.text = "success";
                Debug.Log("success");
                //audioSource.PlayOneShot(hit);
            }
            else
            {
                textMeshPro.text = "more than one click";
                Debug.Log("more than one click");
            }
        }
        else
        {
            //Debug.Log("not target to hit");
        }
    }
}
