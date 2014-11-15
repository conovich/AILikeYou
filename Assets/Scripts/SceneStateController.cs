using UnityEngine;
using System.Collections;

public class SceneStateController : MonoBehaviour {

	public Vector3 stagePosition;
	public Vector3 queuedPosition;
	public Vector3 donePosition;

	public GameObject StartScreen;
	public GameObject InstructionsScreen;
	public GameObject GameScreen;
	public GameObject EndScreen;

	// Use this for initialization
	void Start () {
		InitPositions();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitPositions(){
		StartScreen.transform.position = queuedPosition;
		GameScreen.transform.position = queuedPosition;
		EndScreen.transform.position = donePosition;
	}

	void SetStart(){
		//TODO: set start screen in queued position, lerp to stage
		StartScreen.transform.position = queuedPosition;
		StartScreen.GetComponent<PositionLerper>().SetTarget(stagePosition);
		GameScreen.transform.position = queuedPosition;
		InstructionsScreen.transform.position = queuedPosition;
		EndScreen.transform.position = donePosition;
	}

	void SetInstructions(){
		InstructionsScreen.transform.position = queuedPosition;
		InstructionsScreen.GetComponent<PositionLerper>().SetTarget(stagePosition);
		StartScreen.GetComponent<PositionLerper>().SetTarget(donePosition);
	}

	void SetGameFromStart(){
		StartScreen.GetComponent<PositionLerper>().SetTarget(donePosition);
		GameScreen.transform.position = queuedPosition;
		GameScreen.GetComponent<PositionLerper>().SetTarget(stagePosition);
	}

	void SetGameFromInstructions(){
		InstructionsScreen.GetComponent<PositionLerper>().SetTarget(donePosition);
		GameScreen.transform.position = queuedPosition;
		GameScreen.GetComponent<PositionLerper>().SetTarget(stagePosition);
	}

	void RestartGameFromEnd(){
		StartScreen.transform.position = donePosition;
		GameScreen.transform.position = queuedPosition;
		GameScreen.GetComponent<PositionLerper>().SetTarget(stagePosition);
		EndScreen.GetComponent<PositionLerper>().SetTarget(donePosition);
	}

	void SetEnd(){
		GameScreen.GetComponent<PositionLerper>().SetTarget(donePosition);
		EndScreen.transform.position = queuedPosition;
		EndScreen.GetComponent<PositionLerper>().SetTarget(stagePosition);
	}

	void ResetStart(){

	}

	void ResetGame(){

	}
}
