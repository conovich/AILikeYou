using UnityEngine;
using System.Collections;

public class GameState_TurretTag : MonoBehaviour {
	
	public PlayerController_New PlayerOne;
	
	public State currentState = State.inStart;
	
	public enum State{
		inStart,
		inGame,
		inEnd
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
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState){
		case State.inStart:

		break;
		case State.inGame:
			UpdateInGame();
		break;
		case State.inEnd:

		break;
		}

	}

	void UpdateInGame(){
		if(CheckIfTurretTagged()){
			currentState = State.inEnd;
		}
	}
	
	bool CheckIfTurretTagged(){
		return PlayerOne.hasTaggedTurret;
	}
}
