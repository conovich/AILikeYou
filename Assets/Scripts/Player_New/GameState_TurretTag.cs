using UnityEngine;
using System.Collections;

public class GameState_TurretTag : MonoBehaviour {

	public PlayerController_New PlayerOne;
	public Turret TurretOne;

	public AIStateController myAIStateController;
	public SceneStateController mySceneStateController;
	public GameObject GUIBackgroundPlane;

	public State currentState = State.InStart;
	
	public enum State{
		InStart,
		Instructions,
		InGame,
		InEnd
	}

	
	public static GameState_TurretTag _instance;
	
	public static GameState_TurretTag Instance {
		get {
			return _instance;
		}
	}
	
	void Awake(){
		if(_instance != null){
			Debug.Log("Instance already exists!");
		}
		else{
			_instance = this;
		}
	}
	
	
	// Use this for initialization
	void Start () {
		Init ();
	}
	
	void Init(){
		currentState = State.InStart;
		mySceneStateController.SendMessage("SetStart");
		//ResetLevel();
		GUIBackgroundPlane.GetComponent<ColorLerper_Picker>().StartLerp2();
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState){
		case State.InStart:

		break;
		case State.InGame:
			UpdateInGame();
		break;
		case State.InEnd:

		break;
		}

	}

	void UpdateInGame(){
		if(CheckIfTurretTagged()){
			StartCoroutine(EndGame());
		}
		if(PlayerOne.healthState <= 0){
			StartCoroutine(EndGame());
		}
	}

	void StartGame(){
		if(currentState == State.InStart){
			mySceneStateController.SendMessage("SetGameFromStart");
		}
		else if(currentState == State.Instructions){
			mySceneStateController.SendMessage("SetGameFromInstructions");
		}
		else if(currentState == State.InEnd){
			mySceneStateController.SendMessage("RestartGameFromEnd");
		}
		
		GUIBackgroundPlane.GetComponent<ColorLerper_Picker>().StartLerp1();


		Reset();
		
		currentState = State.InGame;
	}

	void Reset(){
		PlayerOne.Reset();
		TurretOne.Reset();
	}
	
	void StartInstructions(){
		currentState = State.Instructions;
		mySceneStateController.SendMessage("SetInstructions");
	}
	
	IEnumerator EndGame(){
		if(currentState != State.InEnd){
			currentState = State.InEnd;
			GUIBackgroundPlane.GetComponent<ColorLerper_Picker>().StartLerp2();
			yield return new WaitForSeconds(1f);
			mySceneStateController.SendMessage("SetEnd");
		}
		yield return 0;
	}
	
	bool CheckIfTurretTagged(){
		return PlayerOne.hasTaggedTurret;
	}
}
