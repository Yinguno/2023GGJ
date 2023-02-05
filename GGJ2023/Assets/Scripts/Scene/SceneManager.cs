using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static PlaygroundMaker;

public class SceneManager : MonoBehaviour
{
	public GameObject CurrentRegionChecker;
	public GameObject LevelRoot;
	public GameObject CinemachineTargetGroup;

	public GameObject StateRoot;
	public GameObject SuccessObject;
	public GameObject FailObject;

	private CinemachineTargetGroup gTargetGroup;

	DateTime gBaseTime;
	private string gSerial;

	// Start is called before the first frame update
	void Start()
	{
		gBaseTime = DateTime.Now;
		gTargetGroup = CinemachineTargetGroup.GetComponent<CinemachineTargetGroup>();
		string aFirstRoundObjName = LevelRoot.transform.GetChild(0)?.name;
		if (!string.IsNullOrWhiteSpace(aFirstRoundObjName))
		{
			Match aMatch = Regex.Match(aFirstRoundObjName, @"\(([a-z|0-9]+?)\)");
			if (aMatch.Success) { gSerial = aMatch.Value; }
		}

		gStateObjectsMap = new Dictionary<int, List<GameObject>>();
	}

	private int gCurFinishedRound = -1;
	// Update is called once per frame
	void Update()
	{
		//LevelDescription aLevel = PlaygroundMaker.gDummyLevel;
		//if (aLevel != null && CurrentRegionChecker != null)
		//{
		//	// move region checker
		//	MoveRegionChecker((float)((DateTime.Now - gBaseTime).TotalSeconds));

		//	// move camera
		//	int iCurRound = Convert.ToInt32((DateTime.Now - gBaseTime).TotalSeconds);
		//	if (gCurFinishedRound != iCurRound && iCurRound < aLevel.TotalRound)
		//	{
		//		MoveCamera(iCurRound);
		//		gCurFinishedRound = iCurRound;
		//	}
		//}
	}

	public void MoveCamera(int iRound)
	{
		for (int aRound = iRound - 3; aRound <= iRound + 2; aRound++)
		{
			GameObject iRoundObj = LevelRoot.transform.Find($"Round[{aRound}]_{gSerial}")?.gameObject;
			if (iRoundObj != null)
			{
				if (aRound == iRound - 3)
				{
					// remove old
					for (int i = 0; i < iRoundObj.transform.childCount; i++)
					{
						Transform aRoundObj = iRoundObj.transform.GetChild(i);
						gTargetGroup.RemoveMember(aRoundObj);
					}
				}
				else if (aRound == iRound + 2)
				{
					// add new
					for (int i = 0; i < iRoundObj.transform.childCount; i++)
					{
						Transform aRoundObj = iRoundObj.transform.GetChild(i);
						gTargetGroup.AddMember(aRoundObj, 1, 0);
					}
				}
			}
		}
	}

	public void MoveRegionChecker(float iProgress)
	{
		CurrentRegionChecker.transform.position = new Vector3(
			CurrentRegionChecker.transform.position.x,
			CurrentRegionChecker.transform.position.y,
			iProgress);
	}

	private Dictionary<int, List<GameObject>> gStateObjectsMap;

	public void MoveThroughBeatEvent(bool iSuccess, int iRound)
	{
		if(!iSuccess)
		{
			GameObject iRoundObj = LevelRoot.transform.Find($"Round[{iRound}]_{gSerial}")?.gameObject;
			if (iRoundObj != null)
			{
				for (int i = 0; i < iRoundObj.transform.childCount; i++)
				{
					Transform aRoundObj = iRoundObj.transform.GetChild(i);
					if (aRoundObj.gameObject.name.Contains("Block"))
					{
						Vector3 aCreatedPos = aRoundObj.position;
						aCreatedPos.y += 2;
						GameObject aCreatedState = Instantiate(FailObject, aCreatedPos, new Quaternion());
						aCreatedState.transform.parent = StateRoot.transform;
					}
				}
			}
		}
	}

	public void UserInputJudgmentEvent(bool iSuccess, int iRound)
	{
		if (iSuccess)
		{
			GameObject iRoundObj = LevelRoot.transform.Find($"Round[{iRound}]_{gSerial}")?.gameObject;
			if (iRoundObj != null)
			{
				for (int i = 0; i < iRoundObj.transform.childCount; i++)
				{
					Transform aRoundObj = iRoundObj.transform.GetChild(i);
					if (aRoundObj.gameObject.name.Contains("Block"))
					{
						Vector3 aCreatedPos = aRoundObj.position;
						aCreatedPos.y += 2;
						GameObject aCreatedState = Instantiate(SuccessObject, aCreatedPos, new Quaternion());
						aCreatedState.transform.parent = StateRoot.transform;
					}
				}
			}
		}
	}

}
