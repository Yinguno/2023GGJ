using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int modifySerial = 0;

    Metronome metronome;

    [SerializeField] float musicOffset;
    [SerializeField] TextMeshProUGUI hitStateText;
    [SerializeField] TextMeshProUGUI failedTimesText;

    public event Action<int> BeatJudgmentEvent;

    List<int> jList = new() { 2, 6,
        7, 12, 13, 14, 15 };
    Dictionary<int, bool> judgementResults = new();
    AudioSource audioSource;

    int failTimes;

    Track currentTrack;
    [SerializeField] PlaygroundMaker playgroundMaker;
    [SerializeField] SceneManager sceneManager;
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

		PlaygroundMaker.LevelDescription level = playgroundMaker.GetLevel();

		currentTrack = new Track(
			bpm: level.BPM,
			judgmentList: level.GetJudgeDes().Select(aJudge => aJudge.RoundID).ToList(),
			totalBeats: level.TotalRound,
			musicStartOffset: (float)level.MusicStartOffset,
			music);

		StartCoroutine(CountToStart());
    }

    private void OnNextBeat(int index)
    {
        //audioSource.PlayOneShot(beat,0.1f);
        //modifySerial++;
        //playgroundMaker.DrawRoute(index, modifySerial);
        //Debug.Log($"i:{index},next beat delta Time :{Time.time - ti},trackTime={metronome.GetCurrentTime()}");
        //ti = Time.time;
        sceneManager.MoveCamera(index);

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
            hitStateText.text = "miss";
            failTimes++;
            failedTimesText.text = failTimes.ToString();
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
        //metronome.GetHitResult_test();

        // 
        PlaygroundMaker.LevelDescription level = playgroundMaker.GetLevel();
        float curProgress = metronome.GetCurrentTime() * (level.BPM / 60f);
        sceneManager.MoveRegionChecker(curProgress);
    }
    private void CheckAHit()
    {
        if (metronome.GetHitResult())
        {
            Debug.Log("hit");
            if (!judgementResults.ContainsKey(metronome.GetCurrentBeatIndex()))
            {
                judgementResults.Add(metronome.GetCurrentBeatIndex(), true);
                hitStateText.text = "success";
                Debug.Log("success");
                //audioSource.PlayOneShot(hit);
            }
            else
            {
                hitStateText.text = "more than one click";
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
        hitStateText.text = "3";
        yield return new WaitForSeconds(beatLength);
        hitStateText.text = "2";
        yield return new WaitForSeconds(beatLength);
        hitStateText.text = "1";
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
