using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public HealthController myHealthController;
	public int healthState { get { return myHealthController.currentHealth; } }

	GameState game { get { return GameState.Instance; } }
	
	
	// Use this for initialization
	void Start () {
		
	}

	public void Reset(){
		myHealthController.Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider collider){
		bool isHit = false;
		//if(game.PlayerOne.IsPunchingLeft && collider.tag == "PuncherLeft"){
		//	isHit = true;
		//}
		
		
		if(isHit){
			GetComponent<HealthController>().RemoveHealthDelegate();
			//game.PlayerOne.SendMessage("HitEnemy");
		}
	}
}
