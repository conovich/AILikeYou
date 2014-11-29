using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIAction_New : MonoBehaviour {
	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }
	PlayerController_New player { get { return GameState_TurretTag.Instance.PlayerOne; } }
	

	//TODO: EACH MOVE HAS ONE OF THESE ARRAYS -- IN THAT INDEX IS WHERE THE PROBABILITY IS STORED FOR THAT SPECIFIC STATE.
	//then, we can access the probability of each action at a particular state, and pick the best action.

	/* ORDER IN PROBABILITY ARRAY
	int playerHealthIndex = 0; //0-10
	int turretHealthIndex = 1; //0-10
	int turretDistanceIndex = 2; //0-10 
	int bulletDistanceIndex = 3; //0-10
	int bulletHeightIndex = 4; //0-2
	*/

	public int healthIndex { get { return GetHealthIndex(); } }
	public int turretHealthIndex { get { return GetTurretHealthIndex(); } }
	public int turretDistanceIndex { get { return GetTurretDistanceIndex(); } }
	public int bulletDistanceIndex { get { return GetBulletDistanceIndex(); } }
	public int bulletHeightIndex { get { return GetBulletHeightIndex(); } }

	public float[ , , , , ] probabilityArray;

	float alphaWeight = 0.5f;


	// Use this for initialization
	void Start () {
		probabilityArray = new float[11,11,11,11,3];
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: MAKE SURE THESE DON'T GO OUT OF BOUNDS
		//healthIndex = (int)(player.myHealthController.currentHealth/10); //assuming maxHealth = 100
		
		//turretHealthIndex = (int)(player.turretHealth/10); //assuming maxHealth = 100
		
		/*turretDistanceIndex = player.distanceToTurret;
		if(turretDistanceIndex > 10){
			turretDistanceIndex = 10;
		}*/
		
		/*bulletDistanceIndex = player.distanceToBullet;
		if(bulletDistanceIndex > 10){
			bulletDistanceIndex = 10;
		}*/
		//bulletHeightIndex = player.bulletHeight;
	}

	public float GetCurrentProbability(){

		float q_probability = probabilityArray[healthIndex, turretHealthIndex, turretDistanceIndex, bulletDistanceIndex, bulletHeightIndex];

		return q_probability;
	}

	//Qk=Qvk-1+a[rk-Qk-1]    ---> Qk=Qvk-1+a[rk-(maxOfFutureState)Qk-1]
	public void UpdateWeightedProbability(float reward){

		float q_probability = GetCurrentProbability();

		Debug.Log ("before: " + q_probability);

		//k_iteration++;
		q_probability = q_probability + alphaWeight*(reward - q_probability);

		probabilityArray[healthIndex, turretHealthIndex, turretDistanceIndex, bulletDistanceIndex, bulletHeightIndex] = q_probability;

		Debug.Log ("after: " + q_probability);
	}

	int GetHealthIndex(){
		int index = (int)(player.myHealthController.currentHealth/10);
		return index; //assuming maxHealth = 100
	}

	int GetTurretHealthIndex(){
		int index = (int)(player.turretHealth/10);
		return index; //assuming maxHealth = 100
	}

	int GetTurretDistanceIndex(){
		int distanceIndex = player.distanceToTurret;
		if(distanceIndex > 10){
			distanceIndex = 10;
		}
		return distanceIndex;
	}

	int GetBulletDistanceIndex(){
		int distanceIndex = player.distanceToBullet;
		if(distanceIndex > 10){
			distanceIndex = 10;
		}
		if(distanceIndex < 0){ // if there is not bullet, treat it as though the bullet is the max distance away?
			distanceIndex = 10;		//TODO: or should i treat it as though the bullet is at the turret? or zero away?
		}
		return distanceIndex;
	}

	int GetBulletHeightIndex(){
		return player.bulletHeight;
	}


	
}
