using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlaygroundMaker : MonoBehaviour
{
	public GameObject LevelRoot;
	public GameObject BaseBlock;
	public GameObject Left60Block;
	public GameObject Left90Block;
	public GameObject Right60Block;
	public GameObject Right90Block;
	public GameObject BaseLink;
	
	public static LevelDescription gDummyLevel = null;

	// Start is called before the first frame update
	void Start()
	{
		string aDummyLevelJson = File.ReadAllText(Path.Combine(Application.dataPath, @"Resources\Level\level_1.json"));
		gDummyLevel = JsonConvert.DeserializeObject<LevelDescription>(aDummyLevelJson);
		List<JudgeDescription> aJudges = gDummyLevel.GetJudgeDes();
	}

	private int gCurRound = 0;
	private bool gIsDrawCompleted = false;
	private int gModifySerial = 0;
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			gModifySerial++;
			gCurRound++;
			gIsDrawCompleted = false;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			gModifySerial++;
			gCurRound--;
			gIsDrawCompleted = false;
		}

		if (!gIsDrawCompleted && gDummyLevel != null && LevelRoot != null)
		{
			DrawRoute(gCurRound, gModifySerial);
			gIsDrawCompleted = true;
		}
	}

	public void DrawRoute(int iCurRound, int iModifySerial)
	{
		//lock round number
		if (iCurRound < 0) { iCurRound = 0; }
		if (iCurRound >= gDummyLevel.TotalRound) { iCurRound = gDummyLevel.TotalRound; }

		//Debug.Log($"Current round: {iCurRound}");

		//reset
		for (int i = 0; i < LevelRoot.transform.childCount; i++) { Destroy(LevelRoot.transform.GetChild(i).gameObject); }

		//
		for (int aRound = 0; aRound <= iCurRound; aRound++)
		{
			foreach (var aDrawedLine in gDummyLevel.Lines)
			{
				if (aRound <= aDrawedLine.EndRoundID)
				{
					LineDescription aForkFromLine = gDummyLevel.Lines.Where(aLine => aLine.ID == aDrawedLine.ForkFromLineID).FirstOrDefault();
					aDrawedLine.CreateBlock(aRound, aForkFromLine, LevelRoot, BaseBlock, Left60Block, Left90Block, Right60Block, Right90Block, BaseLink, iModifySerial);
				}
			}
		}
	}

	public class LevelDescription
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int BPM { get; set; }
		public int TotalRound { get; set; }
		public List<LineDescription> Lines { get; set; }

		public List<JudgeDescription> GetJudgeDes()
		{
			List<JudgeDescription> aRtnJudges = new List<JudgeDescription>();
			for(int aRound = 0; aRound < TotalRound; aRound++)
			{
				bool iHasJudge = Lines.Where(aLine => aLine.HasJudge(aRound)).Any();
				if (iHasJudge) { aRtnJudges.Add(new JudgeDescription() { RoundID = aRound }); }
			}

			return aRtnJudges;
		}
	}

	public class JudgeDescription
	{
		public int RoundID { get; set; }
	}


	public class LineDescription
	{
		public int ID { get; set; }
		public int? ForkFromLineID { get; set; }
		public int StartRoundID { get; set; }
		public TurnType StartTurnType { get; set; }
		public int EndRoundID { get; set; }
		public List<TurnDescription> Turns { get; set; }
		
		public GameObject CreateBlock(
			int iRound, 
			LineDescription iForkFromLine, 
			GameObject iLevelRoot, 
			GameObject iBaseBlock, 
			GameObject iLeft60Block, 
			GameObject iLeft90Block, 
			GameObject iRight60Block, 
			GameObject iRight90Block,
			GameObject iBaseLink,
			int iModifySerial)
		{
			GameObject aCreatedBlock = null;
			if (iRound >= StartRoundID)
			{
				bool aIsStartBlock = iRound == StartRoundID;

				TurnType aTurnType = GetTurnTypeAtRound(iRound);
				GameObject aTargetPrefab = null;
				switch (aTurnType)
				{
					case TurnType.NONE:
						aTargetPrefab = iBaseBlock;
						break;
					case TurnType.LEFT_60:
						aTargetPrefab = iLeft60Block;
						break;
					case TurnType.LEFT_90:
						aTargetPrefab = iLeft90Block;
						break;
					case TurnType.RIGHT_60:
						aTargetPrefab = iRight60Block;
						break;
					case TurnType.RIGHT_90:
						aTargetPrefab = iRight90Block;
						break;
				}

				string aRoundRootName = $"Round[{iRound}]_({iModifySerial})";
				GameObject aRoundRoot = iLevelRoot.transform.Find(aRoundRootName)?.gameObject;
				if (aRoundRoot == null) { aRoundRoot = new GameObject(aRoundRootName); }

				Vector3 aCurBlockPosition = GetBlockPosition(iRound);
				aCreatedBlock = GameObject.Instantiate(aTargetPrefab, aCurBlockPosition, new Quaternion());
				aCreatedBlock.name = $"{ID}_{aTargetPrefab.name}";
				aCreatedBlock.transform.parent = aRoundRoot.transform;
				aCreatedBlock.transform.Find("Block").gameObject.name = $"{iRound}_{ID}_Block";
				aRoundRoot.transform.parent = iLevelRoot.transform;

				// create link
				if (!aIsStartBlock)
				{
					Vector3 aLastBlockPostion = GetBlockPosition(iRound - 1);
					TurnType aLastTurnType = GetTurnTypeAtRound(iRound - 1);
					float aRotationAngle = 0;
					switch (aLastTurnType)
					{
						case TurnType.NONE:
							aRotationAngle = 0;
							break;
						case TurnType.LEFT_60:
							aRotationAngle = -45;
							break;
						case TurnType.RIGHT_60:
							aRotationAngle = 45;
							break;
						case TurnType.LEFT_90:
							aRotationAngle = -90;
							break;
						case TurnType.RIGHT_90:
							aRotationAngle = 90;
							break;
					}
					GameObject aCreatedLink = GameObject.Instantiate(iBaseLink, (aCurBlockPosition + aLastBlockPostion) / 2, new Quaternion());
					aCreatedLink.transform.eulerAngles = new Vector3(0, aRotationAngle, 0);
					aCreatedLink.transform.localScale = new Vector3(1, 1, (aLastBlockPostion - aCurBlockPosition).magnitude);
					aCreatedLink.transform.Find("Link").gameObject.name = $"{iRound}_{ID}_Link";
					aCreatedLink.transform.parent = aRoundRoot.transform;
				}
			}

			return aCreatedBlock;
		}
		public Vector3 GetBlockPosition(int iRound)
		{
			Vector3 aRtnPosition = new Vector3();
			Vector3 aStartPosition = new Vector3();
			if (ForkFromLineID != null)
			{
				LineDescription aForkFromLine = gDummyLevel.Lines.Where(aLine => aLine.ID == ForkFromLineID).FirstOrDefault();
				aStartPosition = aForkFromLine.GetBlockPosition(StartRoundID);
			}

			aRtnPosition.x = aStartPosition.x + GetOffsetX(iRound);
			aRtnPosition.z = aStartPosition.z + (iRound - StartRoundID);

			return aRtnPosition;
		}
		private int GetOffsetX(int iEndRound)
		{
			int aOffest = 0;

			for (int aRound = StartRoundID; aRound < iEndRound; aRound++)
			{
				TurnType aTurnType = GetTurnTypeAtRound(aRound);
				if (aTurnType == TurnType.LEFT_60 || aTurnType == TurnType.LEFT_90) { aOffest--; }
				if (aTurnType == TurnType.RIGHT_60 || aTurnType == TurnType.RIGHT_90) { aOffest++; }
			}

			return aOffest;
		}

		private TurnType GetTurnTypeAtRound(int iRound)
		{
			TurnType aRtnTurnType = StartTurnType;

			foreach (var aTurnDes in Turns.OrderBy(aTurn => aTurn.RoundID))
			{
				if (aTurnDes.RoundID <= iRound)
				{
					aRtnTurnType = aTurnDes.TurnType;
				}
				else
				{
					break;
				}
			}

			return aRtnTurnType;
		}
		public bool HasJudge(int iRound)
		{
			return (
				Turns.Where(aTurn => aTurn.RoundID == iRound).Any() ||
				(StartRoundID == iRound && iRound != 0)
			);
		}
	}

	public class TurnDescription
	{
		public int RoundID { get; set; }
		public TurnType TurnType { get; set; }
	}

	public enum TurnType
	{
		NONE,
		LEFT_60,
		RIGHT_60,
		LEFT_90,
		RIGHT_90
	}
}
