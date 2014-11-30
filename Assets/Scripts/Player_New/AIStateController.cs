using UnityEngine;
using System.Collections;

public class AIStateController : MonoBehaviour {

	GameState_TurretTag game { get { return GameState_TurretTag.Instance; } }
	
	//AI state values
	public int[] stateArray { get { return GetStateArray(); } }
	int healthIndex { get { return GetHealthIndex(); } }
	int turretHealthIndex { get { return GetTurretHealthIndex(); } }
	int turretDistanceIndex { get { return GetTurretDistanceIndex(); } }
	int bulletDistanceIndex { get { return GetBulletDistanceIndex(); } }
	int bulletHeightIndex { get { return GetBulletHeightIndex(); } }


	public int numStateIndices;


	int[] GetStateArray(){
		int[] theStateArray = new int[] {healthIndex, turretHealthIndex, turretDistanceIndex, bulletDistanceIndex, bulletHeightIndex};
		for(int i = 0; i < theStateArray.Length; i++){
			//Debug.Log(theStateArray[i]);
		}
		return theStateArray;
	}

	int GetHealthIndex(){
		int index = (int)(game.PlayerOne.myHealthController.currentHealth/10);
		return index; //assuming maxHealth = 100
	}
	
	int GetTurretHealthIndex(){
		int index = (int)(game.PlayerOne.turretHealth/10);
		return index; //assuming maxHealth = 100
	}
	
	int GetTurretDistanceIndex(){
		int distanceIndex = game.PlayerOne.distanceToTurret;
		if(distanceIndex > 10){
			distanceIndex = 10;
		}
		return distanceIndex;
	}
	
	int GetBulletDistanceIndex(){
		int distanceIndex = game.PlayerOne.distanceToBullet;
		if(distanceIndex > 10){
			distanceIndex = 10;
		}
		if(distanceIndex < 0){ // if there is not bullet, treat it as though the bullet is the max distance away?
			distanceIndex = 10;		//TODO: or should i treat it as though the bullet is at the turret? or zero away?
		}
		return distanceIndex;
	}
	
	int GetBulletHeightIndex(){
		return game.PlayerOne.bulletHeight;
	}

}
