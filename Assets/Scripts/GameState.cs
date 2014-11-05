using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public PlayerController PlayerOne;


	public static GameState _instance;

	public static GameState Instance {
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
	
	}
}
