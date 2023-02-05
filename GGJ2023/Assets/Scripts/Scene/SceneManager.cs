using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static PlaygroundMaker;

public class SceneManager : MonoBehaviour
{
	public GameObject CurrentRegionChecker;
	public GameObject LevelRoot;
	public GameObject LevelDecorationRoot;
	public GameObject CinemachineTargetGroup;

	public GameObject StateRoot;
	public GameObject SuccessObject;
	public GameObject FailObject;

	public GameObject FireImpactFx;

	[SerializeField] PlaygroundMaker PlaygroundMaker;

	private CinemachineTargetGroup gTargetGroup;

	DateTime gBaseTime;
	private string gSerial;

	private List<Transform> gDecorations;
	private Dictionary<int, List<Transform>> gDecorationAroundJudgeMap;
	private Dictionary<int, List<Vector3>> gJudgePositionMap;
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

		gDecorations = new List<Transform>();
		for (int i=0; i< LevelDecorationRoot.transform.childCount; i++)
		{
			gDecorations.Add(LevelDecorationRoot.transform.GetChild(i));
		}

		gStateObjectsMap = new Dictionary<int, List<GameObject>>();

		LevelDescription aLevel = PlaygroundMaker.GetLevel();
		gJudgePositionMap = new Dictionary<int, List<Vector3>>();
		foreach (var aJudge in aLevel.GetJudgeDes())
		{
			if (!gJudgePositionMap.ContainsKey(aJudge.RoundID))
			{
				gJudgePositionMap.Add(aJudge.RoundID, new List<Vector3>());
			}
			gJudgePositionMap[aJudge.RoundID].Add(aJudge.Position);
		}

		gDecorationAroundJudgeMap = new Dictionary<int, List<Transform>>();
		foreach (var aKvp in gJudgePositionMap)
		{
			int aJudgeID = aKvp.Key;
			if (!gDecorationAroundJudgeMap.ContainsKey(aJudgeID))
			{
				gDecorationAroundJudgeMap.Add(aJudgeID, new List<Transform>());
			}

			List<Vector3> aJudgePosList = aKvp.Value;
			foreach(var aJudgePos in aJudgePosList)
			{
				gDecorationAroundJudgeMap[aJudgeID].AddRange(gDecorations.Where(aTransform => (aTransform.position - aJudgePos).magnitude < 1));
			}
		}
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
			foreach(var aRoundJudgePos in gJudgePositionMap[iRound])
			{
				Vector3 aCreatedPos = aRoundJudgePos;
				aCreatedPos.y += 2;
				GameObject aCreatedState = Instantiate(FailObject, aCreatedPos, new Quaternion());
				aCreatedState.transform.parent = StateRoot.transform;
			}
		}
	}

	public void UserInputJudgmentEvent(bool iSuccess, int iRound)
	{
		if (iSuccess)
		{
			foreach (var aRoundJudgePos in gJudgePositionMap[iRound])
			{
				Vector3 aExplosionPos = aRoundJudgePos;
				Instantiate(FireImpactFx, aExplosionPos, new Quaternion());

				Vector3 aCreatedPos = aRoundJudgePos;
				aCreatedPos.y += 2;
				GameObject aCreatedState = Instantiate(SuccessObject, aCreatedPos, new Quaternion());
				aCreatedState.transform.parent = StateRoot.transform;
			}

			BlowChips(iRound);
		}
	}

	private void BlowChips(int iRound)
	{
		if (gDecorationAroundJudgeMap.ContainsKey(iRound))
		{
			foreach (var aChip in gDecorationAroundJudgeMap[iRound])
			{
				//Destroy(aChip.gameObject);
				Rigidbody aRigidbody = aChip.gameObject.GetComponent<Rigidbody>();
				aRigidbody.useGravity = true;

				Vector3 aExplosionPos = aChip.position;
				aExplosionPos.x += UnityEngine.Random.Range(-0.5f, 0.5f);
				aExplosionPos.y -= 0.3f;
				aExplosionPos.z += UnityEngine.Random.Range(-0.5f, 0.5f);
				aRigidbody.AddExplosionForce(300f, aExplosionPos, 500f);
				aRigidbody.AddTorque(
					new Vector3(
						UnityEngine.Random.Range(-180f, 180f),
						UnityEngine.Random.Range(-180f, 180f),
						UnityEngine.Random.Range(-180f, 180f)));
			}
		}
	}
}
