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
	void Awake()
	{
		//string aDummyLevelJson = File.ReadAllText(Path.Combine(Application.dataPath, @"Resources\Level\level_1.json"));
		string aDummyLevelJson = "{\"Name\":\"level_1\",\"Description\":\"\",\"BPM\":123,\"TotalRound\":76,\"MusicStartOffset\":0.245,\"Lines\":[{\"ID\":0,\"StartRoundID\":0,\"StartTurnType\":0,\"EndRoundID\":11,\"Turns\":[{\"RoundID\":3,\"TurnType\":2}]},{\"ID\":1,\"ForkFromLineID\":0,\"StartRoundID\":3,\"StartTurnType\":1,\"EndRoundID\":13,\"Turns\":[{\"RoundID\":5,\"TurnType\":0},{\"RoundID\":7,\"TurnType\":1},{\"RoundID\":10,\"TurnType\":0}]},{\"ID\":2,\"ForkFromLineID\":1,\"StartRoundID\":7,\"StartTurnType\":2,\"EndRoundID\":21,\"Turns\":[{\"RoundID\":10,\"TurnType\":0},{\"RoundID\":12,\"TurnType\":2}]},{\"ID\":3,\"ForkFromLineID\":2,\"StartRoundID\":12,\"StartTurnType\":1,\"EndRoundID\":23,\"Turns\":[{\"RoundID\":14,\"TurnType\":0},{\"RoundID\":18,\"TurnType\":1}]},{\"ID\":4,\"ForkFromLineID\":2,\"StartRoundID\":16,\"StartTurnType\":1,\"EndRoundID\":38,\"Turns\":[{\"RoundID\":18,\"TurnType\":0},{\"RoundID\":22,\"TurnType\":1},{\"RoundID\":24,\"TurnType\":0},{\"RoundID\":33,\"TurnType\":1},{\"RoundID\":37,\"TurnType\":0}]},{\"ID\":5,\"ForkFromLineID\":2,\"StartRoundID\":18,\"StartTurnType\":0,\"EndRoundID\":36,\"Turns\":[{\"RoundID\":22,\"TurnType\":2},{\"RoundID\":25,\"TurnType\":0},{\"RoundID\":30,\"TurnType\":2}]},{\"ID\":6,\"ForkFromLineID\":3,\"StartRoundID\":20,\"StartTurnType\":0,\"EndRoundID\":36,\"Turns\":[{\"RoundID\":23,\"TurnType\":2},{\"RoundID\":25,\"TurnType\":0},{\"RoundID\":28,\"TurnType\":1},{\"RoundID\":30,\"TurnType\":0},{\"RoundID\":33,\"TurnType\":1}]},{\"ID\":7,\"ForkFromLineID\":3,\"StartRoundID\":18,\"StartTurnType\":2,\"EndRoundID\":20,\"Turns\":[]},{\"ID\":8,\"ForkFromLineID\":4,\"StartRoundID\":22,\"StartTurnType\":2,\"EndRoundID\":27,\"Turns\":[]},{\"ID\":9,\"ForkFromLineID\":8,\"StartRoundID\":25,\"StartTurnType\":0,\"EndRoundID\":36,\"Turns\":[{\"RoundID\":30,\"TurnType\":1}]},{\"ID\":10,\"ForkFromLineID\":5,\"StartRoundID\":30,\"StartTurnType\":1,\"EndRoundID\":35,\"Turns\":[]},{\"ID\":11,\"ForkFromLineID\":5,\"StartRoundID\":33,\"StartTurnType\":0,\"EndRoundID\":45,\"Turns\":[{\"RoundID\":37,\"TurnType\":2},{\"RoundID\":39,\"TurnType\":0}]},{\"ID\":12,\"ForkFromLineID\":10,\"StartRoundID\":33,\"StartTurnType\":0,\"EndRoundID\":44,\"Turns\":[{\"RoundID\":37,\"TurnType\":2},{\"RoundID\":39,\"TurnType\":0}]},{\"ID\":13,\"ForkFromLineID\":9,\"StartRoundID\":33,\"StartTurnType\":0,\"EndRoundID\":41,\"Turns\":[]},{\"ID\":14,\"ForkFromLineID\":6,\"StartRoundID\":33,\"StartTurnType\":0,\"EndRoundID\":52,\"Turns\":[{\"RoundID\":41,\"TurnType\":1},{\"RoundID\":44,\"TurnType\":0}]},{\"ID\":15,\"ForkFromLineID\":11,\"StartRoundID\":37,\"StartTurnType\":1,\"EndRoundID\":42,\"Turns\":[{\"RoundID\":41,\"TurnType\":0}]},{\"ID\":16,\"ForkFromLineID\":12,\"StartRoundID\":37,\"StartTurnType\":1,\"EndRoundID\":40,\"Turns\":[{\"RoundID\":39,\"TurnType\":0}]},{\"ID\":17,\"ForkFromLineID\":13,\"StartRoundID\":37,\"StartTurnType\":2,\"EndRoundID\":65,\"Turns\":[{\"RoundID\":39,\"TurnType\":0},{\"RoundID\":58,\"TurnType\":1},{\"RoundID\":60,\"TurnType\":0},{\"RoundID\":64,\"TurnType\":2}]},{\"ID\":18,\"ForkFromLineID\":11,\"StartRoundID\":41,\"StartTurnType\":1,\"EndRoundID\":70,\"Turns\":[{\"RoundID\":44,\"TurnType\":0},{\"RoundID\":49,\"TurnType\":2},{\"RoundID\":52,\"TurnType\":0},{\"RoundID\":65,\"TurnType\":1},{\"RoundID\":69,\"TurnType\":2}]},{\"ID\":19,\"ForkFromLineID\":12,\"StartRoundID\":41,\"StartTurnType\":2,\"EndRoundID\":45,\"Turns\":[{\"RoundID\":44,\"TurnType\":0}]},{\"ID\":20,\"ForkFromLineID\":14,\"StartRoundID\":41,\"StartTurnType\":2,\"EndRoundID\":53,\"Turns\":[{\"RoundID\":45,\"TurnType\":0}]},{\"ID\":21,\"ForkFromLineID\":20,\"StartRoundID\":46,\"StartTurnType\":2,\"EndRoundID\":50,\"Turns\":[{\"RoundID\":49,\"TurnType\":0}]},{\"ID\":22,\"ForkFromLineID\":17,\"StartRoundID\":44,\"StartTurnType\":2,\"EndRoundID\":47,\"Turns\":[]},{\"ID\":23,\"ForkFromLineID\":14,\"StartRoundID\":44,\"StartTurnType\":2,\"EndRoundID\":48,\"Turns\":[]},{\"ID\":24,\"ForkFromLineID\":18,\"StartRoundID\":49,\"StartTurnType\":1,\"EndRoundID\":76,\"Turns\":[{\"RoundID\":54,\"TurnType\":0},{\"RoundID\":65,\"TurnType\":1},{\"RoundID\":68,\"TurnType\":0},{\"RoundID\":75,\"TurnType\":1}]},{\"ID\":25,\"ForkFromLineID\":14,\"StartRoundID\":49,\"StartTurnType\":2,\"EndRoundID\":71,\"Turns\":[{\"RoundID\":52,\"TurnType\":0},{\"RoundID\":55,\"TurnType\":1},{\"RoundID\":57,\"TurnType\":0},{\"RoundID\":62,\"TurnType\":2},{\"RoundID\":65,\"TurnType\":0},{\"RoundID\":69,\"TurnType\":1}]},{\"ID\":26,\"ForkFromLineID\":18,\"StartRoundID\":52,\"StartTurnType\":1,\"EndRoundID\":57,\"Turns\":[]},{\"ID\":27,\"ForkFromLineID\":17,\"StartRoundID\":52,\"StartTurnType\":2,\"EndRoundID\":55,\"Turns\":[{\"RoundID\":54,\"TurnType\":0}]},{\"ID\":28,\"ForkFromLineID\":25,\"StartRoundID\":55,\"StartTurnType\":2,\"EndRoundID\":70,\"Turns\":[{\"RoundID\":59,\"TurnType\":0}]},{\"ID\":29,\"ForkFromLineID\":18,\"StartRoundID\":58,\"StartTurnType\":1,\"EndRoundID\":65,\"Turns\":[{\"RoundID\":62,\"TurnType\":0}]},{\"ID\":30,\"ForkFromLineID\":29,\"StartRoundID\":62,\"StartTurnType\":1,\"EndRoundID\":65,\"Turns\":[]},{\"ID\":31,\"ForkFromLineID\":28,\"StartRoundID\":62,\"StartTurnType\":2,\"EndRoundID\":64,\"Turns\":[]},{\"ID\":32,\"ForkFromLineID\":28,\"StartRoundID\":68,\"StartTurnType\":2,\"EndRoundID\":72,\"Turns\":[{\"RoundID\":71,\"TurnType\":0}]},{\"ID\":33,\"ForkFromLineID\":28,\"StartRoundID\":59,\"StartTurnType\":1,\"EndRoundID\":61,\"Turns\":[]},{\"ID\":34,\"ForkFromLineID\":24,\"StartRoundID\":60,\"StartTurnType\":1,\"EndRoundID\":62,\"Turns\":[]},{\"ID\":35,\"ForkFromLineID\":24,\"StartRoundID\":65,\"StartTurnType\":2,\"EndRoundID\":72,\"Turns\":[]},{\"ID\":36,\"ForkFromLineID\":35,\"StartRoundID\":68,\"StartTurnType\":0,\"EndRoundID\":75,\"Turns\":[{\"RoundID\":69,\"TurnType\":1}]},{\"ID\":37,\"ForkFromLineID\":25,\"StartRoundID\":69,\"StartTurnType\":2,\"EndRoundID\":75,\"Turns\":[]},{\"ID\":38,\"ForkFromLineID\":18,\"StartRoundID\":65,\"StartTurnType\":2,\"EndRoundID\":66,\"Turns\":[]}]}";
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

	public LevelDescription GetLevel()
	{
		return gDummyLevel;
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
		public double MusicStartOffset { get; set; }
		public List<LineDescription> Lines { get; set; }

		public List<JudgeDescription> GetJudgeDes()
		{
			List<JudgeDescription> aRtnJudges = new List<JudgeDescription>();
			for(int aRound = 0; aRound < TotalRound; aRound++)
			{
				List<LineDescription> aJudgeLines = Lines.Where(aLine => aLine.HasJudge(aRound)).ToList();
				if (aJudgeLines.Count > 0)
				{
					foreach (LineDescription aJudgeLine in aJudgeLines)
					{
						aRtnJudges.Add(new JudgeDescription()
						{
							RoundID = aRound,
							LineID = aJudgeLine.ID,
							Position = aJudgeLine.GetBlockPosition(aRound)
						});
					}
				}
			}

			return aRtnJudges;
		}
	}

	public class JudgeDescription
	{
		public int RoundID { get; set; }
		public int LineID { get; set; }
		public Vector3 Position { get; set; }
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
					aCreatedLink.transform.localScale = new Vector3(1, 1, (aLastBlockPostion - aCurBlockPosition).magnitude * (aLastTurnType == TurnType.NONE ? 1.055f : 1.045f));
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
