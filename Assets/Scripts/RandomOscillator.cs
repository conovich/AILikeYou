using UnityEngine;
using System.Collections;

public class RandomOscillator : MonoBehaviour {
	public float rotationSpeed;
	public int upperTimeBound;
	public int lowerTimeBound;

	int turnCounter = 0;
	int turnCounterMax;
	int direction = 1;
	

	// Use this for initialization
	void Start () {
		ResetTurnCounter();
	}

	void ResetTurnCounter(){
		turnCounter = 0;
		turnCounterMax = Random.Range(lowerTimeBound, upperTimeBound);

		//SendMessage("FireBullet", SendMessageOptions.DontRequireReceiver); //in case a bullet needs to be fired when it turns.

	}
	
	// Update is called once per frame
	void Update () {
		Turn ();
	}

	void Turn(){
		turnCounter++;
		if(turnCounter >= turnCounterMax){
			direction *= -1;
			ResetTurnCounter();
		}
		transform.RotateAround(transform.position, Vector3.up, Time.deltaTime*rotationSpeed*direction);
	}
}
