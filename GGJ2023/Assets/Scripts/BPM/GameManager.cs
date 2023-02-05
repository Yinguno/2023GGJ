using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int modifySerial = 0;

    Metronome metronome;

    [SerializeField] float musicOffset;
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] TextMeshProUGUI countDownText;

    public event Action<int> BeatJudgmentEvent;

    List<int> jList = new() { 2, 6,
        7, 12, 13, 14, 15 };
    Dictionary<int, bool> judgementResults = new();
    AudioSource audioSource;

    int failTimes;

    Track currentTrack;
    [SerializeField] PlaygroundMaker playgroundMaker;
    [SerializeField] AudioClip beat;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip music;
    [SerializeField] GameObject SuccessGameObject;
    [SerializeField] GameObject FailGameObject;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        metronome = GetComponent<Metronome>();
        metronome.GetCurrentTime();
        metronome.NextBeatEvent += OnNextBeat;

        currentTrack = new Track(bpm: 30, judgmentList: jList, totalBeats: 50, musicStartOffset: 0.245f, music);

        StartCoroutine(CountToStart());
    }

    private void OnNextBeat(int index)
    {
        //audioSource.PlayOneShot(beat,0.1f);
        modifySerial++;
        playgroundMaker.DrawRoute(index, modifySerial);
        //Debug.Log($"i:{index},next beat delta Time :{Time.time - ti},trackTime={metronome.GetCurrentTime()}");
        //ti = Time.time;

        int previousBeatIndex = index - 1;
        bool success =
            (
            currentTrack.IsBeatIndexNeedHit(previousBeatIndex)
            && judgementResults.ContainsKey(previousBeatIndex)
            )||
            (
            !currentTrack.IsBeatIndexNeedHit(previousBeatIndex)
            )
            ;
        if (!success)
        {
            textMeshPro.text = "miss";
            failTimes++;
            Debug.Log(failTimes);

        }
        ShowSuccessOrFail(success);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(CountToStart());
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
            CheckAHit();
        }
        countDownText.text = metronome.GetCurrentBeatIndex().ToString();
        //metronome.GetHitResult_test();
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
    IEnumerator CountToStart()
    {
        judgementResults.Clear();
        float beatLength = 60f / currentTrack.bpm;
        //textMeshPro.gameObject.SetActive(true);
        modifySerial++;
        playgroundMaker.DrawRoute(0, modifySerial);
        metronome.Stop();

        metronome.StartTrack(currentTrack);
        textMeshPro.text = "3";
        yield return new WaitForSeconds(beatLength);
        textMeshPro.text = "2";
        yield return new WaitForSeconds(beatLength);
        textMeshPro.text = "1";
        yield return new WaitForSeconds(beatLength);
        //textMeshPro.gameObject.SetActive(false);
        metronome.Go();
    }
    void ShowSuccessOrFail(bool? isSuccess)
    {
        switch (isSuccess)
        {
            case true:
                SuccessGameObject.SetActive(true);
                FailGameObject.SetActive(false);
                break;
            case false:
                SuccessGameObject.SetActive(false);
                FailGameObject.SetActive(true);
                break;
            case null:
                SuccessGameObject.SetActive(false);
                FailGameObject.SetActive(false);
                break;
        }
    }
}
