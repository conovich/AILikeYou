using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAction_New : MonoBehaviour {

	public PlayerController_New player;

/*	public int healthState{ get{ return myHealthController.currentHealth; } }
	public int turretHealth { get { return game.TurretOne.healthState; } }
	public int distanceToTurret { get { return CalculateDistanceToTurret(); } }
	public int distanceToBullet { get { return CalculateDistanceToBullet(); } }
	public int bulletHeight;
*/

	//0 -- health state
	//1 -- turret health
	//2 -- distance to turret
	//3 -- distance to bullet
	//4 -- bullet height

	//TODO: EACH MOVE HAS ONE OF THESE ARRAYS -- IN THAT INDEX IS WHERE THE PROBABILITY IS STORED FOR THAT SPECIFIC STATE.
	//then, we can access the probability of each action at a particular state, and pick the best action.

	/* ORDER IN PROBABILITY ARRAY
	int playerHealthIndex = 0; //0-10
	int turretHealthIndex = 1; //0-10
	int turretDistanceIndex = 2; //0-10 
	int bulletDistanceIndex = 3; //0-10
	int bulletHeightIndex = 4; //0-2
	*/

	float[ , , , , ] probabilityArray;


	// Use this for initialization
	void Start () {
		probabilityArray = new float[11,11,11,11,3];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	float GetCurrentProbability(){
		//TODO: MAKE SURE THESE DON'T GO OUT OF BOUNDS
		int healthIndex = (int)(player.myHealthController.currentHealth/10); //assuming maxHealth = 100

		int turretHealthIndex = (int)(player.turretHealth/10); //assuming maxHealth = 100

		int turretDistanceIndex = player.distanceToTurret;
		if(turretDistanceIndex > 10){
			turretDistanceIndex = 10;
		}

		int bulletDistanceIndex = player.distanceToBullet;
		if(bulletDistanceIndex > 10){
			bulletDistanceIndex = 10;
		}
		int bulletHeightIndex = player.bulletHeight;


		float qProbability = probabilityArray[healthIndex, turretHealthIndex, turretDistanceIndex, bulletDistanceIndex, bulletHeightIndex];

		return qProbability;
	}

	
}
